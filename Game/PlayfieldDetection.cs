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
                temp.center++;
                break;
            case (int) Direction.LEFT:
                temp.center--;
                break;
            case (int) Direction.DOWN:
                temp.center += 10;
                break;
            case (int) Direction.ROTATE:
                temp.VaryCurrentPos();
                break;
        }
        List<int> activeFieldPos = new List<int>();
        activeFieldPos.Add(temp.center);
        foreach (var fieldPos in temp.pos[temp.currentPos])
        {
            activeFieldPos.Add(temp.center + fieldPos);
        }

        return activeFieldPos;
    }

    public bool BoundaryDetection(BlockType active, int direction)
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
                    active.center += 10;
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
        if (temp.character == 'I')
        {
            List<int> postActiveFieldPos = new List<int>();
            postActiveFieldPos.Add(active.center);
            foreach (var fieldPos in active.pos[active.currentPos])
            {
                postActiveFieldPos.Add(active.center + fieldPos);
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

    public bool InterTDetection(List<BlockType> field, BlockType active, int direction)
    {
        BlockType temp = new BlockType();
        temp.Duplicate(active);
        if (direction == (int) Direction.DOWN) // Down
        {
            temp.center += 10;
        }
        List<int> activeFieldPos = InputDetection(temp, direction);

        List<int> bRenderField = new List<int>();
        foreach (var i in field)
        {
            bRenderField.Add(i.center);
            foreach (var id in i.pos[i.currentPos])
            {
                bRenderField.Add(i.center + id);
            }
        }

        foreach (var i in activeFieldPos)
        {
            if (bRenderField.Contains(i))
            {
                Console.WriteLine("run");
                Thread.Sleep(2000);
                if (direction == (int) Direction.DOWN)
                {
                    active.center += 10;
                    Program.playfield.NewActive();
                }
                return true;
            }
        }

        return false;
    }
}