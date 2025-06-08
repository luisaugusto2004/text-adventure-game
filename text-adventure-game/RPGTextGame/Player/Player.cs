using System.Text;
using Enemies;
using Items;
using World;
using Util;
using System.Text.Json.Serialization;
using System.Security.Cryptography;

namespace EntityPlayer
{
    class Player
    {      
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int MaxHealth { get; set; }
        public int Health { get; set; }
        public int Strength { get; set; }
        public int Defense { get; set; }
        public int BaseDefense { get; set; }
        public int Coins { get; set; }
        public int RequiredExperience { get; set; }
        public int Experience { get; set; }
        public int Level { get; set; }
        public bool IsAlive { get; set; }
        [JsonIgnore]
        public Room CurrentRoom { get; private set; }

        public string CurrentRoomName { get; set; }
        public bool ProfeciaAtivada { get; private set; } = false;
        public Inventory inventory { get; set; }
        public Weapon EquippedWeapon { get; set; }
        public Armor EquippedArmor { get; set; }
        public static readonly Weapon DefaultWeapon = new Weapon("Mão", 1, 3, 0);
        public static readonly Armor DefaultArmor = new Armor("Sobretudo batido", "Um sobretudo que você usa desde que se conhece por gente, nunca perde sua beleza", 0, 0);

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
            Defense = 0;
            Coins = 10;
            RequiredExperience = 50;
            Experience = 0;
            Level = 1;
            IsAlive = true;
            inventory = new Inventory();
            inventory.SetPlayer(this);
            EquippedWeapon = DefaultWeapon;
            EquippedArmor = DefaultArmor;
        }

        public void SetId(int id)
        {
            Id = id;
        }

        public void ProphecyActivated()
        {
            ProfeciaAtivada = true;
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

        public void Heal(ConsumableItem consumableToUse)
        {
            if (consumableToUse != null && inventory.Itens.Contains(consumableToUse))
            {
                inventory.RemoveItem(consumableToUse);

                int healthBefore = Health;
                Health += consumableToUse.RollHeal();

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

        public void LoseGold(int amount)
        {
            Coins -= amount;
        }

        public bool CanBuy(Item item)
        {
            if (Coins >= item.Price)
                return true;

            return false;
        }

        public void AddItem(Item item)
        {
            inventory.AddItem(item);
        }

        public void SetRoom(Room room)
        {
            CurrentRoomName = room.Name;
            CurrentRoom = room;
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
            int attack = Strength + EquippedWeapon.RollDamage();

            Console.WriteLine($"{Name} atacou {monster.Name} e causou {attack} de dano.");

            monster.TakeDamage(attack);

            if (!monster.IsAlive)
            {
                int goldAmount = RandomNumberGenerator.GetInt32(4, monster.MaxGold);
                Console.WriteLine($"{monster.Name} derrotado!");
                Console.WriteLine($"{monster.Name} dropou {goldAmount} PO!");
                Console.WriteLine($"Você recebeu {monster.ExperienceGain} XP!");

                GainGold(goldAmount);
                GainExperience(monster.ExperienceGain);
                VerifyLevelUp();
            }
        }

        public void Defend()
        {
            Defense = BaseDefense + EquippedArmor.DefenseAmount + 5;
        }

        public void SetDefense()
        {
            Defense = BaseDefense + EquippedArmor.DefenseAmount;
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

        public IEquippable BuscarItemEquipavelNoInventario(string nome)
        {
            foreach (var item in inventory.Itens)
            {
                if (item is IEquippable equipavel)
                {
                    string nomeItem = TextUtils.RemoverAcentos(item.Name).ToLower();
                    if (nome == nomeItem)
                    {
                        return equipavel;
                    }
                }
            }
            return null;
        }

        public ConsumableItem BuscarPocaoNoInventario(string nome)
        {
            foreach (var item in inventory.Itens)
            {
                if (item is ConsumableItem potion)
                {
                    if (TextUtils.RemoverAcentos(potion.Name).Equals(nome, StringComparison.OrdinalIgnoreCase))
                    {
                        return potion;
                    }
                }
            }
            return null;
        }

        public ConsumableItem BuscarPocaoMaisForteNoInventario()
        {
            ConsumableItem lastPotion = null;
            foreach (var item in inventory.Itens)
            {
                if (item is ConsumableItem potion)
                {
                    if (lastPotion == null || potion.StrongerPotion() > lastPotion.StrongerPotion())
                    {
                        lastPotion = potion;
                    }
                }
            }
            return lastPotion;
        }

        public void SetWeapon(Weapon weapon)
        {
            EquippedWeapon = weapon;
            Console.WriteLine($"Você equipou a arma: {weapon.Name}");
        }

        public void SetArmor(Armor armor)
        {
            EquippedArmor = armor;
            SetDefense();
            Console.WriteLine($"Você equipou a armadura: {armor.Name}");
        }

        public void EquiparItem(IEquippable item)
        {
            if (item is Weapon weapon)
            {
                SetWeapon(weapon);
            }
            else if (item is Armor armor)
            {
                SetArmor(armor);
            }
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"Nome: {Name}");
            sb.AppendLine($"Nível: {Level}");
            sb.AppendLine($"Vida: {Health}/{MaxHealth}");
            sb.AppendLine($"Defesa: {Defense}");
            sb.AppendLine($"Força: {Strength}");
            sb.AppendLine($"Gold: {Coins}");
            sb.AppendLine($"Experiência: {Experience}/{RequiredExperience}");
            string equippedW = $"{EquippedWeapon.Name} (+{EquippedWeapon.Rolls}d{EquippedWeapon.Face})";
            sb.AppendLine($"Arma equipada: {equippedW}");
            sb.AppendLine($"Armadura equipada: {EquippedArmor.Name} (+{EquippedArmor.DefenseAmount})");
            return sb.ToString();
        }
    }
}
