using EntityPlayer;
using System;

namespace Enemies
{
    abstract class Enemy
    {
        public string Name { get; private set; }
        public int MaxHealth { get; private set; }
        public int Health { get; private set; }
        public int Strength { get; private set; }
        public int ExperienceGain { get; private set; }
        public int MaxGold { get; private set; }
        public bool IsAlive { get; private set; }

        public Enemy(string name, int health, int strenght, int experienceGain, int maxGold)
        {
            Name = name;
            MaxHealth = health;
            Health = health;
            Strength = strenght;
            ExperienceGain = experienceGain;
            MaxGold = maxGold;
            IsAlive = true;
        }

        public void TakeDamage(int amount)
        {
            Health -= amount;
            if (Health <= 0)
            {
                IsAlive = false;
                Health = 0;
            }
        }

        public abstract void Attack(Player player, Random random);
    }
}
