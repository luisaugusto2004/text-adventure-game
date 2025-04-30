using System.Globalization;
using System.Text;
using Enemies;
using Scripts;

namespace EntityPlayer
{
    class Player
    {
        public string Name { get; private set; } = string.Empty;
        public int MaxHealth { get; private set; }
        public int Health { get; private set; }
        public int Strength { get; private set; }
        public int Defense { get; private set; }
        public int BaseDefense { get; private set; }
        public int Potions { get; private set; }
        public int Coins { get; private set; }
        public int RequiredExperience { get; private set; }
        public int Experience { get; private set; }
        public int Level { get; private set; }
        public bool IsAlive { get; private set; }

        public Player() { }

        public Player(string name, int health, int strenght)
        {
            Name = name;
            if (Name == string.Empty)
                Name = "Estranho";
            MaxHealth = health;
            Health = health;
            Strength = strenght;
            BaseDefense = 0;
            Potions = 0;
            Defense = 0;
            Coins = 0;
            RequiredExperience = 50;
            Experience = 0;
            Level = 1;
            IsAlive = true;
        }

        public void ProphecyActivated()
        {
            MaxHealth += 999;
            Health = MaxHealth;
            Strength += 999;
        }

        public void Revive()
        {
            if (IsAlive) return;

            IsAlive = true;
            Health = MaxHealth;
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
                Console.WriteLine($"Você tomou uma poção e curou {healedAmount}");

            }
            else
            {
                Console.WriteLine("Você procura na sua bolsa... mas não encontra nada");
            }
        }

        public void TakeDamage(int amount)
        {
            if (!IsAlive) return;

            Health -= amount;
            if (Health <= 0)
            {
                Health = 0;
                IsAlive = false;
            }
        }

        public void GainExperience(int amount)
        {
            Experience += amount;
        }

        public void GainGold(int amount)
        {
            Coins += amount;
        }

        public void LevelUp()
        {
            Level++;
            MaxHealth += 10;
            Health = MaxHealth;
            Strength += 5;
        }

        public void Attack(Enemy monster, Random random)
        {
            int attack = Strength + (random.Next(1, 7));

            Console.WriteLine($"{Name} atacou {monster.Name} e causou {attack} de dano.");

            monster.TakeDamage(attack);

            if (!monster.IsAlive)
            {
                int goldAmount = random.Next(4, monster.MaxGold);
                Console.WriteLine($"{monster.Name} derrotado!");
                Console.WriteLine($"{monster.Name} dropou {goldAmount} moedas de ouro!");
                Console.WriteLine($"Você recebeu {monster.ExperienceGain} XP!");

                GainGold(goldAmount);
                GainExperience(monster.ExperienceGain);
                VerifyLevelUp();
            }
        }

        public void Defend()
        {
            Defense = BaseDefense + 5;
        }

        public void SetBaseDefense()
        {
            Defense = BaseDefense;
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
            sb.AppendLine($"Gold: {Coins}");
            sb.AppendLine($"Experience: {Experience}/{RequiredExperience}");
            return sb.ToString();
        }
    }
}
