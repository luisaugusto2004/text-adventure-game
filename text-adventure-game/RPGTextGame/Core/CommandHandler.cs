using Battle;
using EntityPlayer;
using Items;
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
        private readonly Saloon saloon;

        public CommandHandler(Game game, Player player, ItemShop shop, Saloon saloon)
        {
            this.game = game;
            this.player = player;
            this.shop = shop;
            this.saloon = saloon;
        }

        /// <summary>
        /// Divide o comando bruto que o usuário digitou e realiza a ação correspondente
        /// </summary>
        /// <param name="input">Comando bruto digitado</param>
        /// <param name="game">Instância atual do jogo usada para executar ações</param>
        public string Handle(string[] input)
        {
            if (input.Length == 0)
            {
                return "Digite algum comando";
            }
            string command = input[0].ToLower();    
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
                    return Deslocar(arg);
                case "lutar":
                    return Lutar();
                case "examinar":
                    return Examinar(arg);
                case "equipar":
                    return Equipar(arg);
                case "status":
                    return game.State.PlayerData.ToString();
                case "inventario":
                    return player.inventory.ListItens(player);
                case "usar":
                    return Usar(arg);
                case "comprar":
                    return Comprar(arg);
                case "sair":
                    return Quit(game);
                default:
                    return "Digite um comando válido";
            }
        }

        private string Quit(Game game)
        {
            game.SaveGame();
            Environment.Exit(0);
            return "Saindo do jogo...";
        }

        private string Comprar(string? arg)
        {
            if (string.IsNullOrWhiteSpace(arg))
            {
                return "Digite o número do que você quer comprar";
            }
            if(player.CurrentRoom == game.Rooms.FirstOrDefault(r => r.Name == "Loja"))
            {
                return shop.ProcessPurchase(player, arg);
            }
            else if (player.CurrentRoom == game.Rooms.FirstOrDefault(r => r.Name == "Saloon"))
            {
                return saloon.ProcessPurchase(player, arg);
            }
            return "Não é possível comprar aqui.";
        }

        private string Usar(string? arg)
        {

            if (string.IsNullOrWhiteSpace(arg))
            {
                return "Digite algo para usar";
            }

            var nameItem = TextUtils.RemoverAcentos(arg);
            var consumableItem = player.BuscarPocaoNoInventario(nameItem);

            if (consumableItem != null)
            {
                return player.ItemHeal(consumableItem);
            }
            else
            {
                return "Você não tem esse item no inventario";
            }        
        }

        private string Equipar(string? arg)
        {
            if (string.IsNullOrWhiteSpace(arg))
            {
                return "Digite algo para equipar";
            }

            var nameItem = TextUtils.RemoverAcentos(arg);
            var itemToEquip = player.BuscarItemEquipavelNoInventario(nameItem);
            if (itemToEquip != null)
            {
                player.EquiparItem(itemToEquip);
                return $"Você equipou {itemToEquip.Name}.";
            }
            else
            {
                return "Você não tem esse item no inventario";
            }
        }

        private string Examinar(string? arg)
        {
            if (string.IsNullOrWhiteSpace(arg))
            {
                return "Digite o que quer examinar";
            }

            string argSemAcento = TextUtils.RemoverAcentos(arg.ToLower());

            if (arg == "sala")
            {              
                return player.CurrentRoom.Description;
            }
            string? itemDescription = GetItemDescription(player.inventory.Itens, argSemAcento);
            if (itemDescription != null) return itemDescription;

            if (player.CurrentRoom.Name == "Loja")
            {
                itemDescription = GetItemDescription(shop.Itens, argSemAcento);
                if(itemDescription != null) return itemDescription;
            }
            return "Escolha algo válido para examinar";
        }

        string? GetItemDescription(IEnumerable<Item> itens, string arg)
        {
            foreach (var item in itens)
            {
                string nome = TextUtils.RemoverAcentos(item.Name.ToLower());
                if (arg == nome)
                {
                    return item.Description;

                }
            }
            return null;
        }

        private string Lutar()
        {
            if (player.CurrentRoom.IsHostile)
            {
                BattleManager.SetFight();
                while (BattleManager.GetInCombat())
                    Encounter.RandomEncounter(player, game);
                return "A batalha acabou";
            }
            else
            {
                return  "Não há com o que lutar aqui.";
            }
        }

        private string Deslocar(string? arg)
        {
            if (string.IsNullOrWhiteSpace(arg))
            {
                return "Digite para onde quer ir";
            }

            if (player.CurrentRoom.Exits.TryGetValue(arg, out Room nextRoom))
            {
                player.SetRoom(nextRoom);
                return $"Você se moveu para {player.CurrentRoomName}.";
            }
            else
            {
                return "Modo de usar: deslocar <saida>";
            }
        }
    }
}