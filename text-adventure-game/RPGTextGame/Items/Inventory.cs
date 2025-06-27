using EntityPlayer;
using System.Text;
using Util;

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

        public string ListItens(Player player)
        {
            StringBuilder sb = new StringBuilder();
            List<Item> sortedItems = Itens.OrderBy(i => i.Name).ToList();
            const int boxWidth = 40;
            string topBorder = "╔" + new string('═', boxWidth) + "╗";
            string title = "Mochila";
            string middleBorder = "╠" + new string('═', boxWidth) + "╣";
            string bottomBorder = "╚" + new string('═', boxWidth) + "╝";

            sb.AppendLine(topBorder);
            sb.AppendLine("║" + TextUtils.CenterText(title, boxWidth) + "║");
            sb.AppendLine(middleBorder);

            if (Itens.Count == 0)
            {
                sb.AppendLine("║" + TextUtils.CenterText("(vazio)", boxWidth) + "║");
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
                    else if (item is ConsumableItem consumableItem)
                    {
                        displayText = $"{consumableItem.Name} ({consumableItem.Rolls}d{consumableItem.Face}+{consumableItem.BonusHeal} HP)";
                    }

                    sb.AppendLine("║" + TextUtils.CenterText(displayText, boxWidth) + "║");
                }
            }
            sb.AppendLine(bottomBorder);

            return sb.ToString();
        }

        public bool HasItem(string item)
        {
            foreach(Item i in Itens)
            {
                if(i.Name == item)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
