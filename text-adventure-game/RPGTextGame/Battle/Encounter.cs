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

        public static void RandomEncounter(Random random)
        {
            if(random.Next(1,3) == 1 || random.Next(1,3) == 2)
            {
                Console.Clear();
                Console.WriteLine("A wild Zombie appears!");
                Console.ReadLine();
                Console.Clear();
                Enemy monster = new BustoDeZumbi();
                BattleManager.StartFight(Game.currentPlayer, monster);
            } else if(random.Next(3,4) == 3)
            {
                Console.Clear();
                Console.WriteLine("A wild Skeleton appears!");
                Console.ReadLine();
                Console.Clear();
                Enemy monster = new EsqueletoBruto();
                BattleManager.StartFight(Game.currentPlayer, monster);
            }
        }
    }
}
