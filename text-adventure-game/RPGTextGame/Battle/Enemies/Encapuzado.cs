using EntityPlayer;
using Util;

namespace Enemies
{
    sealed class Encapuzado : Enemy
    {

        public Encapuzado() : base("Encapuzado", 1500, 50, 1, 50, 1000, 5001) { }

        public override void Attack(Player player, Random random)
        {
            int damage = Strength + RollDamage() - player.Defense;
            damage = Math.Max(0, damage);
            if (damage > 0)
            {
                Console.WriteLine($"O {Name} estende a mão e sombras se lançam contra você. Dano: {damage}");
            }
            else
            {
                Console.WriteLine($"O {Name} lança sobras contra você, mas elas se dissipam ao tocar-lhe.");
                Console.WriteLine("Você exclama: ");
                TextPrinter.Print($"\"O encapuzado tenta. Falha. E o show continua!\"", 30);
            }
            player.TakeDamage(damage);
        }
    }
}
