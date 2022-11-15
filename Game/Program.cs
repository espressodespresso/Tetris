namespace Game;

static class Program
{
    static void Main()
    {
        Start();
    }

    public static void Start()
    {
        Playfield playfield = new Playfield();
        playfield.NewActive();
        playfield.Draw();
        Task user = Task.Factory.StartNew(() => playfield.PlayerInput());
        Task.WaitAll(user);
    }
}