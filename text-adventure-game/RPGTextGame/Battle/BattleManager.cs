using EntityPlayer;
using Enemies;
using System;
using Util;
using Core;
using Scripts;

namespace Battle
{
    class BattleManager
    {

        static Random random = new Random();

        public static void StartFight(Player player, Enemy enemy, bool isScripted = false)
        {
            //Fight loop
        }

        public static void PlayerTurn(Player player, Enemy monster)
        {
            player.Defense = player.BaseDefense;

            Console.WriteLine("=============================");
            Console.WriteLine("| [a] Atacar | [b] Defender |");
            Console.WriteLine("| [c] Curar  | [d] Correr   |");
            Console.WriteLine("=============================");
            Console.Write("Digite sua ação: ");
            string input = Console.ReadLine()?.Trim().ToLower();

            if (string.IsNullOrWhiteSpace(input))
            {
                Console.WriteLine("Você hesita... e o tempo parece parar por um instante.");
                return;
            }

            

            if (input == "viver" && monster.Name.ToLower() == "encapuzado" && !Game.profeciaAtivada)
            {
                Game.profeciaAtivada = true;                
                player.Strength += 999;
                player.MaxHealth += 999;
                player.Health = player.MaxHealth;
                ScriptManager.HandleSecretEnding();
                
            }
            else if (input == "a")
            {
                player.Attack(monster);
                Console.ReadLine();
            }
            else if (input == "b")
            {
                player.Defend();
                Console.WriteLine("Você levanta os braços entra em guarda.");
                Console.ReadLine();
            }
            else if (input == "c")
            {
                player.Heal(Game.GlobalRandom.Next(1, 20));
                Console.ReadLine();
            }
            else if (input == "d")
            {
                if (monster.Name.ToLower() == "encapuzado")
                {
                    Console.WriteLine("Não há pra onde correr");
                    Console.WriteLine("Seu destino já está selado.");
                    Console.ReadLine();
                    return;
                }
                //Run logic
            }
            else
            {
                Console.WriteLine("Digite uma ação válida.");
                Console.ReadLine();
            }
        }

        public static void EnemyTurn()
        {

        }

        public static void ShowBattleStatus(Player player, Enemy monster)
        {
            Console.WriteLine("=== STATUS DA BATALHA ===");
            Console.WriteLine($"Você: {player.Health}/{player.MaxHealth} HP");
            if(monster.Name.ToLower() == "encapuzado")
            {
                Console.WriteLine("Encapuzado: ??/??");
                return;
            }
            Console.WriteLine($"{monster.Name}: {monster.Health}/{monster.MaxHealth} HP");
        }
    }
}
