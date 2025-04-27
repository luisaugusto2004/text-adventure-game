using Core;
using Enemies;
using System;
using text_adventure_game.RPGTextGame.Battle.Enemies;

namespace Battle
{
    class Encounter
    {
        public static void FirstEncounter()
        {
            Console.WriteLine("Você leva a mão ao que sobrou da sua arma, mesmo sabendo que não faz diferença.");
            Console.WriteLine("O ser à sua frente não se move. Não precisa.");
            Console.WriteLine("Seu corpo treme — de medo, de frio, ou de destino?");
            Console.WriteLine("A batalha começa...");
            Console.ReadLine();
            Console.Clear();
            Enemy enemy = new Encapuzado();
            BattleManager.StartFight(Game.currentPlayer, enemy, true);
            
        }
    }
}
