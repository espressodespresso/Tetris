namespace Game;

public class Playfield
{
    private const int x = 10;
    private const int y = 16;
    private int[] grid = new[] {9, 19, 29, 39,};
    private Block _block = new Block();
    private List<BlockType> field = new List<BlockType>();
    private BlockType active;

    public void Reset()
    {
        
    }
    
    public void Draw()
    {
        // TEMP
        field.Add(_block.GetType(new Random().Next(0, 7)));

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
}