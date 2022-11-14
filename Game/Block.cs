namespace Game;

public enum BlockTypes
{
   I,
   J,
   L,
   O,
   S,
   T,
   Z
}

public class Block
{
    // Defining all tetris blocks and/or Tetrominoes
    // Defining tile placement in reference to tetromino centre position
    
    private BlockType I = new BlockType(23, new List<int[]>()
    {
        new int[]
        {
            10, 20, 30
        },
        new int[]
        {
            -1, -2, -3
        }
    }, 'I');
    
    private BlockType J = new BlockType(23, new List<int[]>()
    {
        new int[]
        {
            -10, +1, +2
        },
        new int[]
        {
            1, 10, 20
        },
        new int[]
        {
            -1, -2, 10
        },
        new int[]
        {
            -1, -10, -20
        }
    }, 'J');

    private BlockType L = new BlockType(23, new List<int[]>()
    {
        new int[]
        {
            -10, -1, -2
        },
        new int[]
        {
            +1, -10, -20
        },
        new int[]
        {
            10, 1, 2
        },
        new int[]
        {
            -1, 10, 20
        }
    }, 'L');
    
    private BlockType O = new BlockType(23, new List<int[]>()
    {
        new int[]
        {
            1, 10, 11
        }
    }, 'O');
    
    private BlockType S = new BlockType(23, new List<int[]>()
    {
        new int[]
        {
            1, 9, 10
        },
        new int[]
        {
            -10, 1, 11
        }
    }, 'S');
    
    private BlockType T = new BlockType(23, new List<int[]>()
    {
        new int[]
        {
            -1, -10, 1
        },
        new int[]
        {
            -10, 1, 10
        },
        new int[]
        {
            -1, 1, 10
        },
        new int[]
        {
            -10, -1, 10
        }
    }, 'T');
    
    private BlockType Z = new BlockType(23, new List<int[]>()
    {
        new int[]
        {
            -1, 10, 11
        },
        new int[]
        {
            -10, -1, -9
        }
    }, 'Z');
    
    public BlockType GetType(int type)
    {
        switch (type)
        {
            case (int) BlockTypes.I:
                return I;
            case (int) BlockTypes.J:
                return J;
            case (int) BlockTypes.L:
                return L;
            case (int) BlockTypes.O:
                return O;
            case (int) BlockTypes.S:
                return S;
            case (int) BlockTypes.T:
                return T;
            case (int) BlockTypes.Z:
                return Z;
        }

        // Uh-oh
        return null;
    }
}