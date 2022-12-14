namespace Game;

public class BlockType
{
    private int center;
    private List<int[]> pos;
    private char character;
    private int currentPos;
    private ConsoleColor color;

    public int Center
    {
        get => center;
        set => center = value;
    }

    public List<int[]> Pos
    {
        get => pos;
        set => pos = value;
    }

    public char Character
    {
        get => character;
        set => character = value;
    }

    public int CurrentPos
    {
        get => currentPos;
        set => currentPos = value;
    }

    public ConsoleColor Color
    {
        get => color;
        set => color = value;
    }
    
    public BlockType(int center, List<int[]> pos, char character, ConsoleColor color)
    {
        this.center = center;
        this.pos = pos;
        this.character = character;
        currentPos = 0;
        this.color = color;
    }
    
    public BlockType() {}

    public void VaryCurrentPos()
    {
        if (currentPos == pos.Count - 1)
        {
            currentPos = 0;
        }
        else
        {
            currentPos++;
        }
    }

    public void Duplicate(BlockType blockType)
    {
        center = blockType.center;
        pos = blockType.pos;
        character = blockType.character;
        currentPos = blockType.currentPos;
        color = blockType.color;
    }
}