using EntityPlayer;
using Core;
using Battle;
using text_adventure_game.RPGTextGame.Battle.Enemies;

namespace text_adventure_game
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Game.Start();
            BattleManager.PlayerTurn(Game.currentPlayer, new Encapuzado());
        }
    }
}
