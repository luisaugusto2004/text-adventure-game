using Core;

namespace text_adventure_game
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Game game = new Game();
            game.Start();     
        }
    }
}
