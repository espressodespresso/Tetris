﻿namespace Game;

public class Playfield
{
    private const int x = 10;
    private const int y = 16;
    private const int refreshRate = 1;
    private Block _block = new Block();
    private List<BlockType> field = new List<BlockType>();
    private BlockType active = new BlockType();
    private PlayfieldDetection _playfieldDetection = new PlayfieldDetection();

    // Past Harry : Please never do active = blah again, tends to break stuff for some unknown reason
    // Do proprietary func .Duplicate instead
    
    public Playfield()
    {
        active.Duplicate(_block.GetType(new Random().Next(0, 7)));
    }
    
    public void NewActive()
    {
        BlockType insert = new BlockType();
        insert.Duplicate(active);
        field.Add(insert);
        BlockType newActive = _block.GetType(new Random().Next(0, 7));
        active.Duplicate(newActive);
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
        foreach (var blockType in field)
        {
            renderField.Add(blockType.center, blockType.character);
            foreach (var posField in blockType.pos[blockType.currentPos])
            {
                renderField.Add(blockType.center + posField, blockType.character);
            }
        }
        
        renderField.Add(active.center, active.character);
        foreach (var posField in active.pos[active.currentPos])
        {
            renderField.Add(active.center + posField, active.character);
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
                    if (!_playfieldDetection.BoundaryDetection(active, (int) Direction.DOWN))
                    {
                        active.center += 10;
                    }
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