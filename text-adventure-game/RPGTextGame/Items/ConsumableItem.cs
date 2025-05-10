using Util;

namespace Items
{
    class ConsumableItem : Item
    {
        public int Rolls { get; private set; }
        public int Face { get; private set; }
        public int BonusHeal { get; private set; }

        public ConsumableItem(string name, string description, int rolls, int face, int bonus, int price) : base(name, description, price) 
        {
            Rolls = rolls;
            Face = face;
            BonusHeal = bonus;
        }

        public int RollHeal()
        {
            return RollDice.Roll(Rolls, Face) + BonusHeal;
        }

        public int StrongerPotion()
        {
            return (Rolls * Face) + BonusHeal;
        }
    }
}
