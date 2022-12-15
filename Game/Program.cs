namespace Game;

static class Program
{
    public static Playfield playfield = new Playfield();
    private static System.Timers.Timer timer;
    
    static void Main()
    {
        Start();
    }

    public static void Start()
    {
        playfield.Draw();
        Task user = Task.Factory.StartNew(() => playfield.PlayerInput());
        Task.WaitAll(user);
    }
    
}