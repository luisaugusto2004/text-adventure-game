using EntityPlayer;
using Items;
using System.Net.Http.Headers;
using System.Text.Json.Serialization;

namespace World
{
    class ItemShop
    {
        public int Id { get; set; }
        public List<Item> Itens { get; private set; } = new List<Item>();           

        public ItemShop()
        {
            
        }

        public void SetList()
        {
            Itens = new List<Item>()
            {
                new Weapon("Espada curta", 1, 6, 30, "Uma espada enferrujada, barata e nada potente, mas dá pro gasto."),

                new Armor("Armadura incompleta", "Um bracelete de couro para o braço esquerdo, um colete alcochoado e duas ombreiras, é tudo o que você precisa", 4, 30),

                new Weapon("Forcado", 2, 8, 100, "\"Ideal para churrascos... se você for o prato principal.\""),

                new Armor("Colete de Couro Reforçado", "Feito para suportar o que vier, com couro endurecido e um toque de perigo.\n" +
                                                       " Não é perfeito, mas com certeza vai manter você inteiro.", 10, 100),

                new Weapon("Motosserra", 10, 8, 300, "Essa relíquia banhada em sangue já enfrentou horrores que fariam o próprio inferno tremer. \n" +
                                                      "Antiga companheira de um homem que trocou a mão por uma promessa de carnificina. Barulhenta, brutal e absurdamente eficaz. \n" +
                                                      "Se alguém te perguntar por que ela está sempre suja, apenas responda: 'Foi um fim de semana difícil.'"),

                new Armor("Sobretudo do Caleb", "Um sobretudo negro como a noite, desgastado pelo tempo e encharcado de histórias. \n" +
                          "Exala um cheiro de pólvora, cinzas e vingança. Ao vesti-lo, você sente a presença de algo antigo... e perigoso. Estilo impecável. Proteção inigualável. \n" +
                          "\"A bala pode até acertar, mas não atravessa atitude.\"", 50, 300),

                new ConsumableItem("Poção de cura", "\"Uma mistura simples, mas eficaz. Quando a vida te escapa, ela te traz de volta. Beba e sinta a dor desaparecer, por um tempo...\"", 4, 4, 4, 10),

                new ConsumableItem("Poção de cura +1", "\"Uma poção mais potente, feita para curar feridas profundas e restaurar vitalidade rapidamente. \n" +
                                   "Não subestime o poder dessa mistura, ela pode ser sua ultima esperança.\"", 5, 4, 8, 20)
            };
        }

        public void SetId(int id)
        {
            Id = id;
        }

        public void PrintShop(Player player)
        {
            Console.WriteLine("Bem-vindo à Loja! Aqui estão os Itens disponíveis:");
            Console.WriteLine("════════════════════════════════════════════");
            Console.WriteLine("{0,-30} {1,12}", "Item", "Preço (PO)");
            Console.WriteLine("════════════════════════════════════════════");

            for (int i = 0; i < Itens.Count; i++)
            {
                string itemName = Itens[i].Name;
                int price = Itens[i].Price;

                Console.WriteLine("{0,-30} {1,9} PO", $"[{i + 1}] {itemName}", price);
            }
            Console.WriteLine();
            Console.WriteLine("{0, -30} {1,12}", "", $"Gold: {player.Coins} PO");
            Console.WriteLine("════════════════════════════════════════════");
            Console.WriteLine("Digite \"comprar <numero do item que deseja comprar>\"");
        }

        public void ProcessPurchase(Player player, string? arg)
        {
            if (!int.TryParse(arg, out int escolha))
            {
                Console.WriteLine("Escolha invalida");
                return;
            }

            int index = escolha - 1;

            if (index < 0 || index >= Itens.Count)
            {
                Console.WriteLine("Escreva um valor válido");
                return;
            }

            Item itemSelecionado = Itens[index];

            if (!player.CanBuy(itemSelecionado))
            {
                Console.WriteLine("Você não tem ouro suficiente.");
                return;
            }

            player.LoseGold(itemSelecionado.Price);

            if (itemSelecionado is ConsumableItem)
            {
                player.AddItem(itemSelecionado);
            }
            else
            {
                player.AddItem(itemSelecionado);
                Itens.Remove(itemSelecionado);
            }

            Console.WriteLine($"Você comprou {itemSelecionado.Name}");
        }
    }
}
