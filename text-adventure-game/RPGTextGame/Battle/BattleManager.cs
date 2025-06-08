using EntityPlayer;
using Enemies;
using Util;
using Core;
using Scripts;
using System.Security.Cryptography;
using World;

namespace Battle
{
    class BattleManager
    {
        private static int turns = 1;
        private static bool inCombat = false;

        public static void StartFight(Game game, Player player, Enemy enemy, bool isScripted = false)
        {
            while (player.IsAlive && enemy.IsAlive && GetInCombat())
            {
                ShowBattleStatus(player, enemy);
                Console.WriteLine();
                PlayerTurn(player, enemy);
                if (!GetInCombat())
                    return;
                if (CheckSecretEnding(enemy)) return;
                Console.Clear();
                ShowBattleStatus(player, enemy);
                Console.WriteLine();
                if (!enemy.IsAlive)
                    return;
                EnemyTurn(player, enemy);
                Console.Clear();
                if (HandlePlayerDefeatedByEncapuzado(player, enemy)) return;
                if (HandlePlayerDeadToRandomEncounters(player, game)) EndFight();
            }
        }

        public static void SetFight()
        {
            SetInCombat(true);
            turns = 1;
        }

        public static void EndFight()
        {
            SetInCombat(false);
        }

        private static bool HandlePlayerDeadToRandomEncounters(Player player, Game game)
        {
            if (!player.IsAlive)
            {
                RespawnManager.HandlePlayerDeath(player, game);
                return true;
            }
            return false;
        }

        public static bool GetInCombat()
        {
            return inCombat;
        }

        public static void SetInCombat(bool _inCombat)
        {
            inCombat = _inCombat;
        }

        public static void PlayerTurn(Player player, Enemy monster)
        {
            player.SetDefense();

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
                player.Attack(monster);
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
                player.ItemHeal(player.BuscarPocaoMaisForteNoInventario());
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
                if (RandomNumberGenerator.GetInt32(2) == 0)
                {
                    Console.WriteLine("Falha. Você fica aberto para um ataque.");
                    Console.ReadLine();
                }
                else
                {
                    SetInCombat(false);
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

            if (EncapuzadoFirstTurns(player, monster)) return;
            turns++;
            monster.Attack(player);
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

        public static bool EncapuzadoFirstTurns(Player player, Enemy monster)
        {
            if (monster.Name.ToLower() == "encapuzado" && !player.ProfeciaAtivada && turns <= 4)
            {
                switch (turns)
                {
                    case 1:
                        TextPrinter.Print("O encapuzado observa você em silêncio. Seus olhos brilham sob a sombra do capuz.", 30);
                        Console.ReadLine();
                        turns++;
                        return true;
                    case 2:
                        TextPrinter.Print("“Por um segundo, você vê algo atrás dele. Sua própria silhueta... caída no chão.”", 30);
                        Console.ReadLine();
                        turns++;
                        return true;
                    case 3:
                        TextPrinter.Print("“Sua mente treme. Uma voz surge diretamente em seus pensamentos: ‘Você acha que pode mudar o destino?’”", 30);
                        Console.ReadLine();
                        turns++;
                        return true;
                }
            }
            return false;
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
                SetInCombat(false);
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
