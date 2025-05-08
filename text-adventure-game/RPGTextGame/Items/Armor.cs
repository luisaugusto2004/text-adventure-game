namespace Items
{
    class Armor : Item, IEquippable
    {
        public int DefenseAmount { get; set; }

        public Armor(string name, string description, int defenseAmount) : base(name, description)
        {
            DefenseAmount = defenseAmount;
        }
    }
}
