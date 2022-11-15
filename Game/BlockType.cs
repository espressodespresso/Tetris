namespace Game;

public class BlockType
{
    public int center { get; set; }
    public List<int[]> pos { get; set; }
    public char character { get; set; }
    public int currentPos { get; set; }

    public BlockType(int center, List<int[]> pos, char character)
    {
        this.center = center;
        this.pos = pos;
        this.character = character;
        currentPos = 0;
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
    }
}