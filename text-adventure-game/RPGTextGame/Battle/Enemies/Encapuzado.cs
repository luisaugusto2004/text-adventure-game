using Enemies;
using EntityPlayer;
using System;

namespace text_adventure_game.RPGTextGame.Battle.Enemies
{
    sealed class Encapuzado : Enemy
    {

        public Encapuzado() : base("Encapuzado", 100, 100, 1000) { }

        public override void Attack(Player player, Random random)
        {
            int damage = Strength + random.Next(20, 50) - player.Defense;
            if (damage < 0)
                damage = 0;
            Console.WriteLine($"O {Name} estende a mão e sombras se lançam contra você. Dano: {damage}");
            player.TakeDamage(damage);
        }
    }
}
