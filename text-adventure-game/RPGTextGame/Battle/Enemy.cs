using EntityPlayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Battle {
    abstract class Enemy {
        public string Name { get; set; }
        public int MaxHealth { get; set; }
        public int Health { get; set; }
        public int Strength { get; set; }
        public int ExperienceGain { get; set; }

        public Enemy(string name, int health, int strenght, int experienceGain) {
            Name = name;
            MaxHealth = health;
            Health = health;
            Strength = strenght;
            ExperienceGain = experienceGain;
        }

        public void TakeDamage(int amount) {
            Health -= amount;
        }

        public abstract void Attack(Player player, Random random);
    }
}
