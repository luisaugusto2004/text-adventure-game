using Core;
using Enemies;
using EntityPlayer;
using System.Security.Cryptography;

namespace Battle
{
    class Encounter
    {
        public static void FirstEncounter(Player player, Game game)
        {
            Console.WriteLine("Você leva a mão ao que sobrou da sua arma, mesmo sabendo que não faz diferença.");
            Console.WriteLine("O ser à sua frente não se move. Não precisa.");
            Console.WriteLine("Seu corpo treme — de medo, de frio, ou de destino?");
            Console.WriteLine("A batalha começa...");
            Console.ReadLine();
            Console.Clear();
            Enemy enemy = new Encapuzado();
            BattleManager.SetFight();
            BattleManager.StartFight(game, player, enemy, true);
        }

        public static void RandomEncounter(Player player, Game game)
        {
            if (player.CurrentRoom.IsHostile)
            {
                Console.Clear();
                var enemy = GenerateEnemy();

                BattleManager.StartFight(game, player, enemy);
            }
            else
            {
                Console.WriteLine("Não há com o que lutar aqui.");
                Console.ReadLine();
            }
        }

        public static Enemy GenerateEnemy()
        {
            int roll = RandomNumberGenerator.GetInt32(1, 4);
            switch (roll)
            {
                case 1:
                case 2:
                    ShowEncounter("A wild Zombie appears!");
                    return new BustoDeZumbi();
                case 3:
                    ShowEncounter("A wild Skeleton appears!");
                    return new EsqueletoBruto();
                default:
                    ShowEncounter("A mysterious enemy appears!");
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
