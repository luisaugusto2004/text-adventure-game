namespace Items
{
    public sealed class Weapon : Item
    {
        public int Damage { get; private set; }

        public Weapon(string name, int damage, string description = "Seus velhos e confiáveis punhos") : base(name, description)
        { 
            Damage = damage;
        }
    }
}
