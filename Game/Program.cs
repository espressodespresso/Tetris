namespace Game;

static class Program
{
    public static Playfield playfield = new Playfield();
    
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