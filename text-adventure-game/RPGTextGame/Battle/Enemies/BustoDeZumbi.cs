using Enemies;
using EntityPlayer;
using System;

namespace Enemies
{
    class BustoDeZumbi : Enemy
    {
        public BustoDeZumbi() : base("Busto de Zumbi", 16, 3, 6){ }

        public override void Attack(Player player, Random random)
        {
            int damage = Strength + random.Next(1, 5);
            Console.WriteLine($"O Zumbi pula na sua direção, lhe dá um arranhão e causa {damage}");
            player.TakeDamage(damage);
        }
    }
}
