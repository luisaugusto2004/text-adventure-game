using System.Globalization;
using System.Text;
using Enemies;
using Scripts;

namespace EntityPlayer
{
    class Player
    {
        public string Name { get; set; }
        public int MaxHealth { get; set; }
        public int Health { get; set; }
        public int Strength { get; set; }
        public int Defense { get; set; }
        public int BaseDefense { get; set; }
        public int Potions { get; set; }
        public int RequiredExperience { get; set; }
        public int Experience { get; set; }
        public int Level { get; set; }
        public bool IsAlive { get; set; }

        public Player() { }

        public Player(string name, int health, int strenght)
        {
            Name = name;
            MaxHealth = health;
            Health = MaxHealth;
            Strength = strenght;
            BaseDefense = 0;
            Defense = 0;
            RequiredExperience = 50;
            Experience = 0;
            Level = 1;
            IsAlive = true;
        }

        public void Heal(int amount)
        {
            if (Potions >= 1)
            {
                int healthBefore = Health;
                Health += amount;

                if (Health > MaxHealth)
                {
                    Health = MaxHealth;
                }

                int healedAmount = Health - healthBefore;
                Console.WriteLine($"Você toma uma poção e curou {healedAmount}");

            }
            else
            {
                Console.WriteLine("Você procura na sua bolsa... mas não encontra nada");
            }
        }

        public void TakeDamage(int amount)
        {
            Health -= amount;
            if (Health <= 0)
            {
                IsAlive = false;
            }
        }

        public void GainExperience(int amount)
        {
            Experience += amount;
        }

        public void LevelUp()
        {
            Level++;
            MaxHealth += 10;
            Health = MaxHealth;
            Strength += 5;
        }

        public void Attack(Enemy monster)
        {
            Random rand = new Random();

            int attack = Strength + (rand.Next(1, 6));

            Console.WriteLine($"{Name} atacou {monster.Name} e causou {attack} de dano.");

            monster.TakeDamage(attack);

            if (!monster.IsAlive)
            {
                Console.WriteLine($"{monster.Name} derrotado!");
                Console.WriteLine($"Você recebeu {monster.ExperienceGain} XP!");

                GainExperience(monster.ExperienceGain);
                VerifyLevelUp();
            }
        }

        public void Defend()
        {
            Defense = BaseDefense + 5;
        }

        public void VerifyLevelUp()
        {
            while (Experience >= RequiredExperience)
            {
                LevelUp();
                Experience -= RequiredExperience;
                RequiredExperience += 30;
            }
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"Name: {Name}");
            sb.AppendLine($"Level: {Level}");
            sb.AppendLine($"Health: {Health}/{MaxHealth}");
            sb.AppendLine($"Strenght: {Strength}");
            sb.AppendLine($"Experience: {Experience}/{RequiredExperience}");
            sb.AppendLine($"Is Alive: {IsAlive}");
            return sb.ToString();
        }
    }
}
