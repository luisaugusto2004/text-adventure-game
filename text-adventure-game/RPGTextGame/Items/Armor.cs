namespace Items
{
    class Armor : Item, IEquippable
    {
        public int DefenseAmount { get; set; }

        public Armor(string name, string description, int defenseAmount, int price) : base(name, description, price)
        {
            DefenseAmount = defenseAmount;
        }
    }
}
