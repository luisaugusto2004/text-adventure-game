using Enemies;
using EntityPlayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Enemies
{
    class EsqueletoBruto : Enemy
    {
        public EsqueletoBruto() : base("Esqueleto", 26, 5, 8) { }

        public override void Attack(Player player, Random random)
        {
            int damage = Strength + random.Next(1, 7);
            Console.Write($"O Esqueleto avança com seu machado e causa {damage} de dano");
            player.TakeDamage(damage);
        }
    }
}
