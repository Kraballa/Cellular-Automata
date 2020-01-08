using System;

namespace SandBox
{
    public static class Program
    {
        [STAThread]
        static void Main()
        {
            using (var game = new Engine(80,60,16))
                game.Run();
        }
    }
}
