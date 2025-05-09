using EntityPlayer;
using Items;

namespace World
{
    class ItemShop
    {
        private readonly Player player;

        List<Item> itens = new List<Item>()
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

        public ItemShop(Player player)
        {
            this.player = player;
        }

        public void PrintShop()
        {
            Console.WriteLine("Bem-vindo à Loja! Aqui estão os itens disponíveis:");
            Console.WriteLine("════════════════════════════════════════════");
            Console.WriteLine("{0,-30} {1,12}", "Item", "Preço (PO)");
            Console.WriteLine("════════════════════════════════════════════");

            for (int i = 0; i < itens.Count; i++)
            {
                string itemName = itens[i].Name;
                int price = itens[i].Price;

                Console.WriteLine("{0,-30} {1,9} PO", $"[{i + 1}] {itemName}", price);
            }
            Console.WriteLine();
            Console.WriteLine("{0, -30} {1,12}", "", $"Gold: {player.Coins} PO");
            Console.WriteLine("════════════════════════════════════════════");
            Console.WriteLine("Digite \"comprar <numero do item que deseja comprar>\"");
        }

        public void ProcessPurchase(string? arg)
        {
            if (!int.TryParse(arg, out int escolha))
            {
                Console.WriteLine("Escolha invalida");
                return;
            }
            else
            {
                int index = escolha - 1;

                if (index >= 0 && index < itens.Count)
                {
                    Item itemSelecionado = itens[index];

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
                        itens.Remove(itemSelecionado);
                    }

                    Console.WriteLine($"Você comprou {itemSelecionado.Name}");
                    return;
                }
                else
                {
                    Console.WriteLine("Escreva um valor válido");
                }
            }
        }
    }
}
