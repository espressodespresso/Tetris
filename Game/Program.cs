namespace Game;

static class Program
{
    static void Main()
    {
        Playfield playfield = new Playfield();
        playfield.NewTetromino();
        playfield.Draw();
        Task user = Task.Factory.StartNew(() => playfield.PlayerInput());
        Task.WaitAll(user);
    }
}