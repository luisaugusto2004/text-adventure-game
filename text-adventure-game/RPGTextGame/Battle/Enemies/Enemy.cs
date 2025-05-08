using EntityPlayer;
using System;
using Util;

namespace Enemies
{
    abstract class Enemy
    {
        public string Name { get; private set; }
        public int MaxHealth { get; private set; }
        public int Health { get; private set; }
        public int Strength { get; private set; }
        public int Rolls { get; private set; }
        public int Face { get; private set; }
        public int ExperienceGain { get; private set; }
        public int MaxGold { get; private set; }
        public bool IsAlive { get; private set; }

        public Enemy(string name, int health, int strenght, int rolls, int face, int experienceGain, int maxGold)
        {
            Name = name;
            MaxHealth = health;
            Health = health;
            Strength = strenght;
            Rolls = rolls;
            Face = face;
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

        public int RollDamage()
        {
            return RollDice.Roll(Rolls, Face);
        }
    }
}
