namespace Items
{
    public class Inventory
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

        public void ListItens()
        {
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
                Console.WriteLine("║"+ CenterText("(vazio)", boxWidth)+"║");
            }
            else
            {
                foreach (Item item in Itens)
                {
                    string itemText = item is Weapon weapon
                ? $"{item.Name} (+{weapon.Damage})"
                : item.Name;

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
