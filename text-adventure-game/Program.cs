using EntityPlayer;
using Core;
using Battle;
using text_adventure_game.RPGTextGame.Battle.Enemies;
using Enemies;

namespace text_adventure_game
{
    internal class Program
    {
        static void Main(string[] args)
        {
            
            Game.Start();
            Enemy enemy = new Encapuzado();
            while (Game.currentPlayer.IsAlive && enemy.IsAlive)
            {
                BattleManager.PlayerTurn(Game.currentPlayer, enemy);
                Console.Clear();
                BattleManager.ShowBattleStatus(Game.currentPlayer, enemy);
            }
        }
    }
}
