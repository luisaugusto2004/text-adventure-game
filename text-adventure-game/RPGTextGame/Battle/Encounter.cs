using Core;
using Enemies;
using EntityPlayer;
using System;
using System.Diagnostics.Metrics;

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
            BattleManager.SetInCombat(true);
            BattleManager.StartFight(Game.currentPlayer, enemy, true);
            BattleManager.SetInCombat(false);
        }

        public static void RandomEncounter(Player player)
        {
            if (player.CurrentRoom.IsHostile)
            {
                Console.Clear();
                var enemy = GenerateEnemy(Game.GlobalRandom);
                
                BattleManager.StartFight(player, enemy);
            }
            else
            {
                Console.WriteLine("Não há com o que lutar aqui.");
                Console.ReadLine();
            }
        }

        public static Enemy GenerateEnemy(Random random)
        {
            int roll = random.Next(1, 4);
            switch (roll)
            {
                case 1:
                    ShowEncounter("A wild Zombie appears!");
                    return new BustoDeZumbi();
                case 2:
                    ShowEncounter("A wild Zombie appears!");
                    return new BustoDeZumbi();
                case 3:
                    ShowEncounter("A wild Skeleton appears!");
                    return new EsqueletoBruto();
                default:
                    return new BustoDeZumbi();
            }
        }
        private static void ShowEncounter(string message)
        {
            Console.Clear();
            Console.WriteLine(message);
            Console.ReadLine();
            Console.Clear();
        }
    }
}
