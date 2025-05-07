using EntityPlayer;
using Core;
using Battle;
using Enemies;

namespace text_adventure_game
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Game game = new Game();
            game.Start();     
        }
    }
}
