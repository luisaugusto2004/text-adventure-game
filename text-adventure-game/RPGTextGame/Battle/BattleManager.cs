using EntityPlayer;
using Enemies;
using System;
using Util;
using Core;
using Scripts;
using System.Threading;

namespace Battle
{
    class BattleManager
    {
        private static int turns = 1;

        public static void StartFight(Player player, Enemy enemy, bool isScripted = false)
        {
            turns = 1;
            while (player.IsAlive && enemy.IsAlive)
            {
                BattleManager.ShowBattleStatus(player, enemy);
                Console.WriteLine();
                BattleManager.PlayerTurn(player, enemy);
                if (!Encounter.GetInCombat())
                    break;
                if (CheckSecretEnding(enemy)) return;
                Console.Clear();
                ShowBattleStatus(player, enemy);
                Console.WriteLine();
                if (!enemy.IsAlive)
                    return;
                BattleManager.EnemyTurn(player, enemy);
                if (HandlePlayerDefeatedByEncapuzado(player, enemy)) return;
                // TODO: Implementar lógica quando o jogador morrer em encontros aleatórios.
                Console.Clear();
            }
        }

        public static void PlayerTurn(Player player, Enemy monster)
        {
            player.SetBaseDefense();

            Console.WriteLine("=============================");
            Console.WriteLine("| [a] Atacar | [d] Defender |");
            Console.WriteLine("| [h] Curar  | [r] Correr   |");
            Console.WriteLine("=============================");
            Console.Write("Digite sua ação: ");
            string input = Console.ReadLine()?.Trim().ToLower();

            if (string.IsNullOrWhiteSpace(input))
            {
                Console.WriteLine("Você hesita... e o tempo parece parar por um instante.");
                Console.ReadLine();
                return;
            }

            if (input == "viver" && monster.Name.ToLower() == "encapuzado" && !player.ProfeciaAtivada)
            {
                player.ProphecyActivated();
                ScriptManager.HandleSecretEnding();

            }
            else if (input == "a")
            {
                player.Attack(monster, Game.GlobalRandom);
                if (monster.Name.ToLower() == "encapuzado" && !monster.IsAlive)
                {
                    Console.Clear();
                    return;
                }
                Console.ReadLine();
            }
            else if (input == "d")
            {
                player.Defend();
                Console.WriteLine("Você levanta os braços entra em guarda.");
                Console.ReadLine();
            }
            else if (input == "h")
            {
                player.Heal(Game.GlobalRandom.Next(1, 20));
                Console.ReadLine();
            }
            else if (input == "r")
            {
                if (monster.Name.ToLower() == "encapuzado")
                {
                    Console.WriteLine("Não há pra onde correr");
                    Console.WriteLine("Seu destino já está selado.");
                    Console.ReadLine();
                    return;
                }
                Console.Write("Você tenta escapar e...");
                Console.ReadLine();
                // Tenta fugir da batalha com 50% de chance de sucesso.
                // Futuramente, considerar usar um atributo de agilidade para influenciar essa chance.
                if (Game.GlobalRandom.Next(2) == 0)
                {
                    Console.WriteLine("Falha. Você fica aberto para um ataque.");
                    Console.ReadLine();
                }
                else
                {
                    Encounter.SetInCombat(false);
                    Console.WriteLine("Consegue! Você escapa e está fora de combate.");
                    Console.ReadLine();
                }
            }
            else
            {
                Console.WriteLine("Você pensa no que quer fazer mas hesita, nem mesmo você sabe o que quer fazer.");
                Console.ReadLine();
            }
        }

        public static void EnemyTurn(Player player, Enemy monster)
        {
            if (monster.Name.ToLower() == "encapuzado" && !player.ProfeciaAtivada && turns <= 3)
            {
                switch (turns)
                {
                    case 1:
                        TextPrinter.Print("O encapuzado observa você em silêncio. Seus olhos brilham sob a sombra do capuz.", 30);
                        turns++;
                        Console.ReadLine();
                        return;
                    case 2:
                        TextPrinter.Print("“Por um segundo, você vê algo atrás dele. Sua própria silhueta... caída no chão.”", 30);
                        turns++;
                        Console.ReadLine();
                        return;
                    case 3:
                        TextPrinter.Print("“Sua mente treme. Uma voz surge diretamente em seus pensamentos: ‘Você acha que pode mudar o destino?’”", 30);
                        turns++;
                        Console.ReadLine();
                        return;
                    default:
                        TextPrinter.Print("O encapuzado permanece imóvel, mas você sente que ele está apenas aguardando seu fim.", 30);
                        Console.ReadLine();
                        return;
                }
            }

            turns++;
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
                Console.WriteLine($"Turno: {turns}");
                return;
            }
            Console.WriteLine($"{monster.Name}: {monster.Health}/{monster.MaxHealth} HP");
            Console.WriteLine($"Turno: {turns}");
        }

        public static bool CheckSecretEnding(Enemy enemy)
        {
            if (enemy.Name.ToLower() == "encapuzado" && !enemy.IsAlive)
            {
                Console.Clear();
                ScriptManager.HandleSecretEndingWhenDefeated();
                Console.ReadLine();
                Environment.Exit(0);
                return true;
            }
            return false;
        }

        public static bool HandlePlayerDefeatedByEncapuzado(Player player, Enemy enemy)
        {
            if (enemy.Name.ToLower() == "encapuzado" && !player.IsAlive)
            {
                player.Revive();
                Console.Clear();
                TextPrinter.Print("Você não tem nome aqui, Pereça.", 70);
                Console.ReadLine();
                Console.Clear();
                ScriptManager.ScriptedScenePostBattle();
                return true;
            }
            return false;
        }
    }
}
