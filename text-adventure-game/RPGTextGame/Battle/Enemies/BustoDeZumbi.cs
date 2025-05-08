using EntityPlayer;

namespace Enemies
{
    class BustoDeZumbi : Enemy
    {
        public BustoDeZumbi() : base("Busto de Zumbi", 13, 3, 1, 4, 6, 13) { }

        public override void Attack(Player player, Random random)
        {
            int damage = Strength + RollDamage() - player.Defense;
            damage = Math.Max(0, damage);
            if (damage > 0)
            {
                Console.WriteLine($"O Zumbi pula na sua direção, lhe dá um arranhão e causa {damage}");
            }
            else
            {
                Console.WriteLine("O Zumbi pula, você apenas dá um passo pro lado e ele passa direto!");
            }
            player.TakeDamage(damage);
        }
    }
}
