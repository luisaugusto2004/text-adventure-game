using Enemies;
using EntityPlayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace text_adventure_game.RPGTextGame.Battle.Enemies {
    sealed class Encapuzado : Enemy {

        public Encapuzado() : base("Encapuzado", 1500, 100, 1000){}

        public override void Attack(Player player, Random random) {
            int attack = Strength + random.Next(20, 50);
            Console.WriteLine($"O {Name} estende a mão e sombras se lançam contra você. Dano: {attack}");
            player.TakeDamage(attack);
        }
    }
}
