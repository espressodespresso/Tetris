namespace Game;

public class Playfield
{
    private const int x = 10;
    private const int y = 16;
    private const int refreshRate = 1;
    private Block _block = new Block();
    private List<BlockType> field = new List<BlockType>();
    private BlockType active;
    private PlayfieldDetection _playfieldDetection = new PlayfieldDetection();

    public void Reset()
    {
        
    }

    public void NewActive()
    {
        active = _block.GetType(0);
    }
    
    public void Draw()
    {
        Console.Clear();
        Dictionary<int, char> renderField = Render();

        for (int i = 0; i < x * y; i++)
        {
            if (i != 0 && i % 10 == 9)
            {
                Console.WriteLine(ReturnCharacter(renderField, i));
            }
            else
            {
                Console.Write(ReturnCharacter(renderField, i));
            }
        }
    }

    public Dictionary<int, char> Render()
    {
        Dictionary<int, char> renderField = new Dictionary<int, char>();
        foreach (var iBlockType in field)
        {
            renderField.Add(iBlockType.center, iBlockType.character);
            foreach (var iFieldPos in iBlockType.pos[iBlockType.currentPos])
            {
                int fieldPos = iBlockType.center + iFieldPos;
                renderField.Add(fieldPos, iBlockType.character);
            }
        }
        
        renderField.Add(active.center, active.character);
        
        foreach (var iActive in active.pos[active.currentPos])
        {
            int fieldPos = active.center + iActive;
            renderField.Add(fieldPos, active.character);
        }

        return renderField;
    }

    public char ReturnCharacter(Dictionary<int, char> renderField, int i)
    {
        if (renderField.ContainsKey(i))
        {
            return renderField[i];
        }

        return '-';
    }

    private enum Direction
    {
        RIGHT, // 0
        LEFT,  // 1
        DOWN,  // 2
        ROTATE // 3
    }
    
    public void PlayerInput()
    {
        while (true)
        {
            switch (Console.ReadKey(true).Key)
            {
                case ConsoleKey.UpArrow:
                    if (!_playfieldDetection.BoundaryDetection(active, (int) Direction.ROTATE))
                    {
                        active.VaryCurrentPos();
                    }
                    Draw();
                    break;
                case ConsoleKey.RightArrow:
                    if (!_playfieldDetection.BoundaryDetection(active, (int) Direction.RIGHT))
                    {
                        active.center++;
                    }
                    Draw();
                    break;
                case ConsoleKey.LeftArrow:
                    if (!_playfieldDetection.BoundaryDetection(active, (int) Direction.LEFT))
                    {
                        active.center--;
                    }
                    Draw();
                    break;
                case ConsoleKey.DownArrow:
                    active.center += 10;
                    Draw();
                    break;
                case ConsoleKey.I:
                    Draw();
                    break;
            }
            
            Thread.Sleep(10);
        }
    }
}