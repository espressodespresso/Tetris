using System.Diagnostics;
using System.Timers;
using Timer = System.Timers.Timer;

namespace Game;

public class Playfield
{
    private const int x = 10;
    private const int y = 16;
    private int score = 0;
    private int lines = 0;
    private Block _block = new Block();
    private List<BlockType> field = new List<BlockType>();
    private BlockType active = new BlockType();
    private PlayfieldDetection _playfieldDetection = new PlayfieldDetection();
    private Timer timer;
    

    // Dev to Dev : Please never do active = ...
    // Do proprietary func .Duplicate instead
    // Breaks pretty much all functions
    
    public Playfield()
    {
        active.Duplicate(_block.GetType(new Random().Next(0, 7)));
        SetTimer();
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
            char displayChar = _block.ReturnCharacter(renderField, i);
            Console.ForegroundColor = _block.ReturnCColor(displayChar);
            if (i != 0 && i % 10 == 9)
            {
                if (i == 59 || i == 69 || i == 99 || i == 109)
                {
                    Console.Write(displayChar);
                    switch (i)
                    {
                        case 59:
                            SpecialPrint(ConsoleColor.DarkRed, "\tS C O R E");
                            break;
                        case 69:
                            SpecialPrint(ConsoleColor.White, "\t" + score);
                            break;
                        case 99:
                            SpecialPrint(ConsoleColor.DarkRed, "\tL I N E S");
                            break;
                        case 109:
                            SpecialPrint(ConsoleColor.White, "\t" + lines);
                            break;
                    }
                }
                else
                {
                    Console.WriteLine(displayChar);
                }
            }
            else
            {
                Console.Write(displayChar);
            }
        }
    }

    private void SpecialPrint(ConsoleColor textcolor, string text)
    {
        Console.ForegroundColor = textcolor;
        Console.WriteLine(text);
        Console.ForegroundColor = ConsoleColor.Black;
    }

    public Dictionary<int, char> Render()
    {
        Dictionary<int, char> renderField = new Dictionary<int, char>();
        try
        {
            foreach (var blockType in field)
            {
                renderField.Add(blockType.Center, blockType.Character);
                if (blockType.Pos.Any())
                {
                    foreach (var posField in blockType.Pos[blockType.CurrentPos])
                    {
                        renderField.Add(blockType.Center + posField, blockType.Character);
                    }
                }
            }

            renderField.Add(active.Center, active.Character);

            foreach (var posField in active.Pos[active.CurrentPos])
            {
                renderField.Add(active.Center + posField, active.Character);
            }
        }
        catch (ArgumentException)
        {
            CloseApp();
        }
        return renderField;
    }

    public void CloseApp()
    {
        timer.Close();
        timer.Dispose();
        Console.Clear();
        string line = "*******************************";
        Console.WriteLine(line);
        Console.WriteLine("Thanks for playing!");
        try
        {
            if (score > GetHighScore())
            {
                Console.WriteLine("-*-NEW HIGH SCORE -*-");
            }
        }
        catch (FileNotFoundException)
        {
            Console.WriteLine("-*-NEW HIGH SCORE -*-");
        }
        SaveScore();
        Console.WriteLine("Score - " + score);
        Console.WriteLine("Lines - " + lines);
        try
        {
            Console.WriteLine("High Score - " + GetHighScore());
        }
        catch (FileNotFoundException ex)
        {
            Console.WriteLine("High Score - 0");
        }
        Console.WriteLine(line);
        Thread.Sleep(5000);
        Environment.Exit(0);
    }

    public void PlayerInput()
    {
        while (true)
        {
            switch (Console.ReadKey(true).Key)
            {
                case ConsoleKey.UpArrow:
                    if (!_playfieldDetection.BoundaryDetection(field, active, (int) Direction.ROTATE))
                    {
                        if (!_playfieldDetection.InterTDetection(field, active, (int) Direction.ROTATE))
                        {
                            active.VaryCurrentPos();
                            _playfieldDetection.LineDetection(field);
                        }
                    }
                    Draw();
                    break;
                case ConsoleKey.RightArrow:
                    if (!_playfieldDetection.BoundaryDetection(field, active, (int) Direction.RIGHT))
                    {
                        if (!_playfieldDetection.InterTDetection(field, active, (int) Direction.RIGHT))
                        {
                            active.Center++;
                            _playfieldDetection.LineDetection(field);
                        }
                    }
                    Draw();
                    break;
                case ConsoleKey.LeftArrow:
                    if (!_playfieldDetection.BoundaryDetection(field, active, (int) Direction.LEFT))
                    {
                        if (!_playfieldDetection.InterTDetection(field, active, (int) Direction.LEFT))
                        {
                            active.Center--;
                            _playfieldDetection.LineDetection(field);
                        }
                    }
                    Draw();
                    break;
                case ConsoleKey.DownArrow:
                    if (!_playfieldDetection.BoundaryDetection(field, active, (int) Direction.DOWN))
                    {
                        if (!_playfieldDetection.InterTDetection(field, active, (int) Direction.DOWN))
                        {
                            active.Center += 10;
                            _playfieldDetection.LineDetection(field);
                        }
                    }
                    Draw();
                    InputDown();
                    break;
            }
            
            Thread.Sleep(10);
        }
    }

    // https://learn.microsoft.com/en-us/dotnet/api/system.timers.timer?view=net-5.0
    private void SetTimer()
    {
        timer = new Timer(2000);
        timer.Elapsed += OnTimedEvent;
        timer.AutoReset = true;
        timer.Enabled = true;
    }

    private void InputDown()
    {
        timer.Stop();
        timer.Dispose();
        SetTimer();
    }
    
    public void OnTimedEvent(Object source, ElapsedEventArgs e)
    {
        if (!_playfieldDetection.BoundaryDetection(field, active, (int) Direction.DOWN))
        {
            if (!_playfieldDetection.InterTDetection(field, active, (int) Direction.DOWN))
            {
                active.Center += 10;
                _playfieldDetection.LineDetection(field);
            }
        }
        Draw();
    }

    public int GetFieldSize()
    {
        return x * y;
    }

    public void AddUndefinedBlock(BlockType undefined)
    {
        field.Add(undefined);
    }

    public void AddLines(int amount)
    {
        lines += amount;
    }

    public void AddScore(int amount)
    {
        score += amount;
    }

    public int GetHighScore()
    {
        FileStream file = File.Open("highscore.dat", FileMode.Open);
        BinaryReader br = new BinaryReader(file);
        int localScore = br.ReadInt32();
        br.Close();
        file.Close();
        return localScore;
    }
    
    public void SaveScore()
    {
        int highscore;
        try
        {
            highscore = GetHighScore();
        }
        catch (FileNotFoundException)
        {
            highscore = 0;
        }

        if (score > highscore)
        {
            FileStream file = File.Open("highscore.dat", FileMode.Create);
            BinaryWriter bw = new BinaryWriter(file);
            bw.Write(score);
            bw.Close();
            file.Close();
        }
    }
}