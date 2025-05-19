//#define DEBUG_MODE

using Battle;
using EntityPlayer;
using System.Runtime.CompilerServices;
using System.Text;
using Util;
using World;

[assembly: InternalsVisibleTo("text-adventure-game.Test")]
namespace Core
{
    class CommandHandler
    {
        private readonly Player player;
        private readonly ItemShop shop;
        private readonly Game game;

        public CommandHandler(Player player)
        {
            this.player = player;
        }

        public CommandHandler(Player player, ItemShop shop)
        {
            this.player = player;
            this.shop = shop;
        }

        public CommandHandler(Game game, Player player, ItemShop shop)
        {
            this.game = game;
            this.player = player;
            this.shop = shop;
        }

        public void Handle(string[] input, Game game)
        {
            if (input.Length == 0)
            {
                Console.WriteLine("Digite algum comando");
                Console.ReadLine();
                return;
            }
            string command = input[0].ToLower();
            //Separar parâmetro com espaços para verificação mais intuitiva excluindo o espaço na última parte do parâmetro               
            StringBuilder sb = new StringBuilder();
            for (int i = 1; i < input.Length; i++)
            {
                sb.Append(input[i]);
                if (input.Length - i != 1)
                    sb.Append(' ');
                else
                    break;
            }
            string? arg = input.Length > 1 ? sb.ToString().ToLower() : null;

            switch (command)
            {
                case "deslocar":
                #if DEBUG_MODE
                    Deslocar(arg, true);
                #else
                    Deslocar(arg);
                #endif
                    break;
                case "lutar":
                    Lutar();
                    break;
                case "examinar":
                    Examinar(arg);
                    break;
                case "equipar":
                    Equipar(arg);
                    break;
                case "status":
                    Console.WriteLine(game.State.PlayerData);
                    Console.ReadLine();
                    break;
                case "inventario":
                    player.inventory.ListItens(player);
                    break;
                case "usar":
                    Usar(arg);
                    break;
                case "comprar":
                    Comprar(arg);
                    break;
                case "sair":
                    Quit(game);
                    break;
                default:
                    Console.WriteLine("Digite um comando válido");
                    Console.ReadLine();
                    break;
            }
        }

        private void Quit(Game game)
        {
            game.SaveGame();
            Environment.Exit(0);
        }

        private void Comprar(string? arg)
        {
            if (string.IsNullOrEmpty(arg))
            {
                Console.WriteLine("Digite o número do que você quer comprar");
                Console.ReadLine();
                return;
            }

            shop.ProcessPurchase(player, arg);
            Console.ReadLine();
        }

        private void Usar(string? arg)
        {

            if (string.IsNullOrWhiteSpace(arg))
            {
                Console.WriteLine("Digite algo para usar");
                Console.ReadLine();
                return;
            }

            var nameItem = TextUtils.RemoverAcentos(arg);
            var consumableItem = player.BuscarPocaoNoInventario(nameItem);

            if (consumableItem != null)
            {
                player.Heal(consumableItem);
            }
            else
            {
                Console.WriteLine("Você não tem esse item no inventario");
            }
            Console.ReadLine();
        }

        private void Equipar(string? arg)
        {
            if (string.IsNullOrWhiteSpace(arg))
            {
                Console.WriteLine("Digite algo para equipar");
                Console.ReadLine();
                return;
            }

            var nameItem = TextUtils.RemoverAcentos(arg);
            var itemToEquip = player.BuscarItemEquipavelNoInventario(nameItem);
            if (itemToEquip != null)
            {
                player.EquiparItem(itemToEquip);
            }
            else
            {
                Console.WriteLine("Você não tem esse item no inventario");
            }
            Console.ReadLine();
        }

        private void Examinar(string? arg)
        {
            // TODO: Fazer um sistema de examinar decente, que dê pra examinar tudo que está no inventário
            if (string.IsNullOrWhiteSpace(arg))
            {
                Console.WriteLine("Digite o que quer examinar");
                Console.ReadLine();
                return;
            }

            string argSemAcento = TextUtils.RemoverAcentos(arg.ToLower());
            string nomeArmaSemAcento = TextUtils.RemoverAcentos(player.EquippedWeapon.Name.ToLower());

            if (arg == "sala")
            {
                Console.WriteLine(player.CurrentRoom.Description);
            }
            else if (argSemAcento == nomeArmaSemAcento)
            {
                Console.WriteLine(player.EquippedWeapon.Description);
            }
            else
            {
                Console.WriteLine("Escolha algo válido para examinar");
            }
            Console.ReadLine();
        }

        private void Lutar()
        {
            if (player.CurrentRoom.IsHostile)
            {
                BattleManager.SetFight();
                while (BattleManager.GetInCombat())
                    Encounter.RandomEncounter(player, game);
            }
            else
            {
                Console.WriteLine("Não há com o que lutar aqui.");
                Console.ReadLine();
            }
        }

        private void Deslocar(string? arg, bool isTest = false)
        {
            if (string.IsNullOrWhiteSpace(arg))
            {
                Console.WriteLine("Digite para onde quer ir");
                if (!isTest)
                    Console.ReadLine();
                return;
            }

            if (player.CurrentRoom.Exits.TryGetValue(arg, out Room nextRoom))
            {
                player.SetRoom(nextRoom);
                Console.WriteLine($"Você se moveu para {player.CurrentRoom.Name}.");

            }
            else
            {
                Console.WriteLine("Modo de usar: deslocar <saida>");
            }
            if (!isTest)
                Console.ReadLine();
        }
    }
}