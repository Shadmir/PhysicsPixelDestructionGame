using System;

namespace PhysicsPixelDestructionGame
{
    public static class Program
    {
        [STAThread]
        static void Main() //main is called as the first method when the program is compiled
        {
            //run the game
            using var game = new Game1();
            game.Run();
        }
    }
}
