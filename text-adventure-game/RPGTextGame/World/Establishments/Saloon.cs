using EntityPlayer;
using Items;

namespace World
{
    class Saloon : IShop
    {
        public Dictionary<string, int> comidas = new Dictionary<string, int>
            {
                { "Rato Frito", 12 },
                { "Carne Fresca", 15 },
                { "Banquete Completo", 25 }
            };

        public Saloon()
        {

        }

        public void PrintShop(Player player)
        {
            Console.WriteLine("Bem-vindo ao Saloon parceiro! Aqui estão as comidas no cardápio:");
            Console.WriteLine("════════════════════════════════════════════");
            Console.WriteLine("{0,-30} {1,12}", "Item", "Preço (PO)");
            Console.WriteLine("════════════════════════════════════════════");

            int index = 1;
            foreach (var item in comidas)
            {
                Console.WriteLine("{0,-30} {1,9} PO", $"[{index}] {item.Key}", item.Value);
                index++;
            }
            Console.WriteLine();
            Console.WriteLine("{0, -30} {1,12}", "", $"Gold: {player.Coins} PO");
            Console.WriteLine("════════════════════════════════════════════");
            Console.WriteLine("Digite \"comprar <numero da comida que deseja comprar>\"");
        }

        public void ProcessPurchase(Player player, string? arg)
        {
            if (!int.TryParse(arg, out int escolha))
            {
                Console.WriteLine("Escolha invalida");
                return;
            }

            int index = escolha - 1;

            if (index < 0 || index >= comidas.Count)
            {
                Console.WriteLine("Escreva um valor válido");
                return;
            }

            var itemSelecionado = comidas.ElementAt(index);

            if (!player.CanBuy(itemSelecionado.Value))
            {
                Console.WriteLine("Você não tem ouro suficiente.");
                return;
            }

            player.LoseGold(itemSelecionado.Value);

            switch (index)
            {
                case 0:
                    player.Heal(10);
                    break;
                case 1:
                    player.Heal(15);
                    break;
                case 2:
                    player.Heal(20);
                    break;
            }
        }
    }
}
