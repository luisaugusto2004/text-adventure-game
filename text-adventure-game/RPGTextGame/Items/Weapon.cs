using Util;

namespace Items
{
    sealed class Weapon : Item
    {
        public int Rolls { get; set; }
        public int Face { get; set; }

        public Weapon(string name, int rolls, int face, string description = "Seus velhos e confiáveis punhos") : base(name, description)
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
