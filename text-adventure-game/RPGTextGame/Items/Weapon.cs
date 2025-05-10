using Util;

namespace Items
{
    sealed class Weapon : Item, IEquippable
    {
        public int Rolls { get; set; }
        public int Face { get; set; }

        public Weapon(string name, int rolls, int face, int price, string description = "Seus velhos e confiáveis punhos") : base(name, description, price)
        {
            Rolls = rolls;
            Face = face;
        }

        public int RollDamage()
        {
            return RollDice.Roll(Rolls, Face);
        }
    }
}
