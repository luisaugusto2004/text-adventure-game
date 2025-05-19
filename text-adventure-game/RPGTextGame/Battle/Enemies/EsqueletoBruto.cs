using EntityPlayer;

namespace Enemies
{
    class EsqueletoBruto : Enemy
    {
        public EsqueletoBruto() : base("Esqueleto", 26, 3, 1, 6, 8, 21) { }

        public override void Attack(Player player)
        {
            int damage = Strength + RollDamage() - player.Defense;
            damage = Math.Max(0, damage);
            if (damage > 0)
            {
                Console.WriteLine($"O Esqueleto avança com seu machado e causa {damage} de dano!");
            }
            else
            {
                Console.WriteLine("O Esqueleto ataca, mas o machado nem atravessa sua armadura!");
            }
            player.TakeDamage(damage);
        }
    }
}
