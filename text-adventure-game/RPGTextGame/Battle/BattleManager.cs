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



            if (input == "viver" && monster.Name.ToLower() == "encapuzado" && !Game.ProfeciaAtivada)
            {
                Game.ProfeciaAtivada = true;
                player.Strength += 999;
                player.MaxHealth += 999;
                player.Health = player.MaxHealth;
                ScriptManager.HandleSecretEnding();

            }
            else if (input == "a")
            {
                player.Attack(monster);
                if (monster.Name.ToLower() == "encapuzado" && !monster.IsAlive)
                {
                    ScriptManager.HandleSecretEndingWhenDefeated();
                    Console.ReadLine();
                    return;
                }
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

        public static void EnemyTurn(Player player, Enemy monster)
        {
            if (monster.Name.ToLower() == "encapuzado" && !Game.ProfeciaAtivada && Game.Turns <= 3)
            {
                switch (Game.Turns)
                {
                    case 1:
                        TextPrinter.Print("O encapuzado observa você em silêncio. Seus olhos brilham sob a sombra do capuz.", 30);
                        Game.Turns++;
                        Console.ReadLine();
                        return;
                    case 2:
                        TextPrinter.Print("“Por um segundo, você vê algo atrás dele. Sua própria silhueta... caída no chão.”", 30);
                        Game.Turns++;
                        Console.ReadLine();
                        return;
                    case 3:
                        TextPrinter.Print("“Sua mente treme. Uma voz surge diretamente em seus pensamentos: ‘Você acha que pode mudar o destino?’”", 30);
                        Game.Turns++;
                        Console.ReadLine();
                        return;
                    default:
                        TextPrinter.Print("O encapuzado permanece imóvel, mas você sente que ele está apenas aguardando seu fim.", 30);
                        Console.ReadLine();
                        return;
                }
            }

            Game.Turns++;
            monster.Attack(player, Game.GlobalRandom);
            Console.ReadLine();
        }

        public static void ShowBattleStatus(Player player, Enemy monster)
        {
            Console.WriteLine("=== STATUS DA BATALHA ===");
            Console.WriteLine($"Você: {player.Health}/{player.MaxHealth} HP");
            if (monster.Name.ToLower() == "encapuzado")
            {
                Console.WriteLine("Encapuzado: ??/??");
                Console.WriteLine($"Turno: {Game.Turns}");
                return;
            }
            Console.WriteLine($"{monster.Name}: {monster.Health}/{monster.MaxHealth} HP");
            Console.WriteLine($"Turno: {Game.Turns}");
        }
    }
}
