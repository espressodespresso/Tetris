namespace Game;

public class PlayfieldDetection
{
    // DIRECTION
    // RIGHT, 0
    // LEFT,  1
    // DOWN   2
    // ROTATE 3
    
    private enum pLocation
    {
        CENTER, // 0
        RIGHT,  // 1
        LEFT    // 2
    }

    public List<int> InputDetection(BlockType temp, int direction)
    {
        switch (direction)
        {
            case (int) Direction.RIGHT:
                temp.Center++;
                break;
            case (int) Direction.LEFT:
                temp.Center--;
                break;
            case (int) Direction.DOWN:
                temp.Center += 10;
                break;
            case (int) Direction.ROTATE:
                temp.VaryCurrentPos();
                break;
        }
        List<int> activeFieldPos = new List<int>();
        activeFieldPos.Add(temp.Center);
        foreach (var fieldPos in temp.Pos[temp.CurrentPos])
        {
            activeFieldPos.Add(temp.Center + fieldPos);
        }

        return activeFieldPos;
    }

    // Checks if the active BlockTypes next move will put them outside of the 'grid' boundaries
    public bool BoundaryDetection(List<BlockType> field, BlockType active, int direction)
    {
        BlockType temp = new BlockType();
        temp.Duplicate(active);
        List<int> activeFieldPos = InputDetection(temp, direction);

        if (direction == (int) Direction.DOWN) // Down
        {
            foreach (var fieldPos in activeFieldPos)
            {
                if (fieldPos > 149)
                {
                    active.Center += 10;
                    LineDetection(field);
                    Program.playfield.NewActive();
                    return true;
                }
            }
        }
        
        List<int> fieldpLocations = GetpLocation(activeFieldPos);
        if (fieldpLocations.Contains(1) && fieldpLocations.Contains(2) && !fieldpLocations.Contains(0))
        {
            return true;
        }

        // Special check for I character as it's 1 tile thick in pos 0 hence regular detection does not apply
        if (temp.Character == 'I')
        {
            List<int> postActiveFieldPos = new List<int>();
            postActiveFieldPos.Add(active.Center);
            foreach (var fieldPos in active.Pos[active.CurrentPos])
            {
                postActiveFieldPos.Add(active.Center + fieldPos);
            }
            
            List<int> postFieldpLocations = GetpLocation(postActiveFieldPos);
            // https://stackoverflow.com/questions/12795882/quickest-way-to-compare-two-generic-lists-for-differences
            if (!Enumerable.SequenceEqual(fieldpLocations, postFieldpLocations) 
                && !fieldpLocations.Contains(0) && !postFieldpLocations.Contains(0))
            {
                return true;
            } 
        }

        return false;
    }

    public List<int> GetpLocation(List<int> activeFieldPos)
    {
        List<int> fieldpLocations = new List<int>();
        foreach (var fieldPos in activeFieldPos)
        {
            for (int i = 0; i < 5; i++)
            {
                if (fieldPos % 10 == i)
                {
                    if (i == 4)
                    {
                        fieldpLocations.Add((int) pLocation.CENTER);
                        break;
                    }
                    
                    fieldpLocations.Add((int) pLocation.LEFT);
                    break;
                    
                }
            }

            for (int i = 5; i < 10; i++)
            {
                if (fieldPos % 10 == i)
                {
                    if (i == 5)
                    {
                        fieldpLocations.Add((int) pLocation.CENTER);
                        break;
                    }
                    
                    fieldpLocations.Add((int) pLocation.RIGHT);
                    break;
                }
            }
        }

        return fieldpLocations;
    }

    // Checks if the active BlockType will collide with a BlockType on the field (center & corresponding pos)
    public bool InterTDetection(List<BlockType> field, BlockType active, int direction)
    {
        BlockType temp = new BlockType();
        temp.Duplicate(active);
        if (direction == (int) Direction.DOWN) // Down
        {
            temp.Center += 10;
        }
        List<int> activeFieldPos = InputDetection(temp, direction);

        List<int> bRenderField = new List<int>();
        foreach (var i in field)
        {
            bRenderField.Add(i.Center);
            if (i.Pos.Any())
            {
                foreach (var id in i.Pos[i.CurrentPos])
                {
                    bRenderField.Add(i.Center + id);
                }
            }
        }

        foreach (var i in activeFieldPos)
        {
            if (bRenderField.Contains(i))
            {
                if (direction == (int) Direction.DOWN)
                {
                    active.Center += 10;
                    LineDetection(field);
                    Program.playfield.NewActive();
                }
                return true;
            }
        }

        return false;
    }
    
    public void LineDetection(List<BlockType> field)
    {
        Dictionary<int, List<int>> positions = new Dictionary<int, List<int>>();
        foreach (var i in field)
        {
            List<int> pos = new List<int>();
            if (i.Pos.Any())
            {
                foreach (var j in i.Pos[i.CurrentPos])
                {
                    pos.Add(i.Center + j);
                }
            }
            positions.Add(i.Center, pos);
        }

        int lines = 0;
        int lineCharCount = 0;
        List<int> replaceLines = new List<int>();
        for (int i = Program.playfield.GetFieldSize() - 1; i > 0; i--)
        {
            if (positions.Keys.Contains(i))
            {
                lineCharCount++;
            }

            foreach (var l in positions.Values)
            {
                if (l.Contains(i))
                {
                    lineCharCount++;
                }
            }

            if (lineCharCount == 10)
            {
                lines++;
                for (int j = i; j < i+10; j++)
                {
                    replaceLines.Add(j);
                }
            }

            if (i % 10 == 0)
            {
                lineCharCount = 0;
            }
        }
        
        Program.playfield.AddLines(lines);
        switch (lines)
        {
            case 1:
                Program.playfield.AddScore(40);
                break;
            case 2:
                Program.playfield.AddScore(100);
                break;
            case 3:
                Program.playfield.AddScore(300);
                break;
            case 4:
                Program.playfield.AddScore(1200);
                break;
        }

        if (replaceLines.Any())
        {
            List<int> toRemove = new List<int>();
            foreach (var key in positions.Keys)
            {
                if (replaceLines.Contains(key))
                {
                    BlockType temp = new BlockType();
                    foreach (BlockType type in field)
                    {
                        if (type.Center == key)
                        {
                            temp.Duplicate(type);
                            field.Remove(type);
                            toRemove.Add(key);
                            break;
                        }
                    }

                    if (temp.Pos.Any())
                    {
                        foreach (var i in temp.Pos[temp.CurrentPos])
                        {
                            if (!replaceLines.Contains(temp.Center + i))
                            {
                                Program.playfield.AddUndefinedBlock(new BlockType(temp.Center + i, new List<int[]>(), temp.Character, temp.Color));
                            }
                        }
                    }
                }
            }

            if (toRemove.Any())
            {
                foreach (var key in toRemove)
                {
                    positions.Remove(key);
                }
            }

            // In the case that the BlockTypes key isn't in the line to be deleted but a pos from the center is
            foreach (var valueList in positions.Values)
            {
                int center = 0;
                foreach (var value in valueList)
                {
                    if (replaceLines.Contains(value))
                    {
                        center = positions.FirstOrDefault(val => val.Value == valueList).Key;
                    }
                }

                if (center != 0)
                {
                    BlockType temp = new BlockType();
                    foreach (BlockType type in field)
                    {
                        if (type.Center == center)
                        {
                            temp.Duplicate(type);
                            field.Remove(type);
                            break;
                        }
                    }

                    if (!replaceLines.Contains(temp.Center))
                    {
                        Program.playfield.AddUndefinedBlock(new BlockType(temp.Center, new List<int[]>(), temp.Character, temp.Color));
                    }

                    if (temp.Pos.Any())
                    {
                        foreach (var i in temp.Pos[temp.CurrentPos])
                        {
                            if (!replaceLines.Contains(temp.Center + i))
                            {
                                Program.playfield.AddUndefinedBlock(new BlockType(temp.Center + i, new List<int[]>(), temp.Character, temp.Color));
                            }
                        }
                    }
                }
            }

            // Gets all BlockTypes above the lines removed and brings them 'down' in the grid
            foreach (BlockType type in field)
            {
                if (type.Center > replaceLines.Min())
                {
                    continue;
                }

                if (type.Pos.Any())
                {
                    bool valid = true;
                    foreach (var j in type.Pos[type.CurrentPos])
                    {
                        if (type.Center + j > replaceLines.Min())
                        {
                            valid = false;
                            break;
                        }
                    }

                    if (!valid)
                    {
                        continue;
                    }
                }

                switch (lines)
                {
                    case 1:
                        type.Center += 10;
                        break;
                    case 2:
                        type.Center += 20;
                        break;
                    case 3:
                        type.Center += 30;
                        break;
                    case 4:
                        type.Center += 40;
                        break;
                }
            }
        }
    }
}