using System;

namespace SandBox
{
    public static class Program
    {
        [STAThread]
        static void Main()
        {
            using (var game = new Engine(150,100,8))
                game.Run();
        }
    }
}
