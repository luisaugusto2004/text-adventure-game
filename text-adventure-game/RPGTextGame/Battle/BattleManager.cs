using EntityPlayer;
using Enemies;
using System;
using Util;
using Core;

namespace Battle
{
    class BattleManager
    {

        static Random random = new Random();

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

            bool profeciaAtivada = false;

            if (input == "viver" && monster.Name.ToLower() == "encapuzado" && !profeciaAtivada)
            {
                profeciaAtivada = true;
                Console.Clear();
                TextPrinter.Print("O mundo para.", 50);
                Console.ReadLine();
                TextPrinter.Print("O tempo se dobra como um papel queimando pelas bordas.", 50);
                Console.ReadLine();
                TextPrinter.Print("A névoa ao redor do encapuzado hesita. Pela primeira vez, ele recua um passo.", 50);
                Console.ReadLine();
                TextPrinter.Print("Sua pele arrepia. O coração, antes hesitante, bate com a força de mil trovões.", 50);
                Console.ReadLine();
                TextPrinter.Print("Você não sabe por quê, mas sabe que essa palavra... era a chave.", 50);
                Console.ReadLine();
                Console.Clear();
                TextPrinter.Print("A profecia desperta. Você foi escolhido. O portador da centelha que recusa o fim.", 50);
                TextPrinter.Print("Seu corpo se ilumina com uma chama antiga, esquecida pelas eras.", 50);
                Console.ReadLine();
                player.Strength += 999;
                player.MaxHealth += 999;
                player.Health = player.MaxHealth;
                TextPrinter.Print("O ser encapuzado fala pela primeira vez:\r\n“...impossível.”", 50);
                TextPrinter.Print("A verdadeira batalha começa!", 50);
            }

            if (input == "a")
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

        public static void ShowBattleStatus(Player player, Enemy monster)
        {
            Console.WriteLine("=== STATUS DA BATALHA ===");
            Console.WriteLine($"Você: {player.Health}/{player.MaxHealth} HP");
            Console.WriteLine($"Inimigo: {monster.Health}/{monster.MaxHealth} HP");
        }
    }
}
