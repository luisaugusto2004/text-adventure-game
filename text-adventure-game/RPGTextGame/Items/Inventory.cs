using EntityPlayer;

namespace Items
{
    class Inventory
    {
        public List<Item> Itens { get; set; } = new List<Item>();
        public Player player;

        public Inventory()
        {
            
        }

        public void SetPlayer(Player player)
        {
            this.player = player;
        }

        public void AddItem(Item item)
        {
            Itens.Add(item);
        }

        public void RemoveItem(Item item)
        {
            for (int i = 0; i < Itens.Count; i++)
            {
                if (Itens[i] == item)
                {
                    Itens.RemoveAt(i);
                }
            }
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
                    string displayText = item.Name;

                    if (item is Weapon weapon)
                    {
                        displayText = $"{weapon.Name} (+{weapon.Rolls}d{weapon.Face} dano)";
                        if (player.EquippedWeapon?.Name == weapon.Name)
                            displayText = "(E) " + displayText;
                    }
                    else if (item is Armor armor)
                    {
                        displayText = $"{armor.Name} (+{armor.DefenseAmount} def)";
                        if (player.EquippedArmor?.Name == armor.Name)
                            displayText = "(E) " + displayText;
                    } 
                    else if(item is ConsumableItem consumableItem)
                    {
                        displayText = $"{consumableItem.Name} ({consumableItem.Rolls}d{consumableItem.Face}+{consumableItem.BonusHeal} HP)";
                    }
                        
                        Console.WriteLine("║" + CenterText(displayText, boxWidth) + "║");
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
