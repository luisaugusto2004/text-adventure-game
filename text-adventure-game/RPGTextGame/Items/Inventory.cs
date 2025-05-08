using EntityPlayer;

namespace Items
{
    class Inventory
    {
        public List<Item> Itens { get; private set; }

        public Inventory()
        {
            Itens = new List<Item>();
        }

        public void AddItem(Item item)
        {
            Itens.Add(item);
        }

        public void ListItens(Player player)
        {
            List<Item> sortedItems = Itens.OrderBy(i => i.Name).ToList();
            const int boxWidth = 40;
            string topBorder = "╔" + new string('═', boxWidth) + "╗";
            string title = "Mochila";
            string middleBorder = "╠" + new string('═', boxWidth) + "╣";
            string bottomBorder = "╚" + new string('═', boxWidth) + "╝";

            Console.WriteLine(topBorder);
            Console.WriteLine("║" + CenterText(title, boxWidth) + "║");
            Console.WriteLine(middleBorder);

            if (Itens.Count == 0)
            {
                Console.WriteLine("║" + CenterText("(vazio)", boxWidth) + "║");
            }
            else
            {
                foreach (Item item in sortedItems)
                {
                    string itemText = item is Weapon weapon
                ? $"{weapon.Name} (+{weapon.Rolls}d{weapon.Face})"
                : item.Name;
                    if(player.equippedWeapon.Name == item.Name)
                    {
                        Console.WriteLine("║" + CenterText("(E) "+itemText, boxWidth) + "║");
                        continue;
                    }

                    Console.WriteLine("║" + CenterText(itemText, boxWidth) + "║");
                }
            }
            Console.WriteLine(bottomBorder);
            Console.ReadLine();
        }

        private string CenterText(string text, int width)
        {
            if (text.Length >= width)
                return text.Substring(0, width);

            int spaces = width - text.Length;
            int left = spaces / 2;
            int right = spaces - left;
            return new string(' ', left) + text + new string(' ', right);
        }
    }
}
