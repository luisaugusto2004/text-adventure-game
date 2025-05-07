using System;

namespace Items
{
    class ConsumableItem : Item
    {
        private int healthGain;

        public ConsumableItem(string name, string description, int _healthGain) : base(name, description) 
        {
            healthGain = _healthGain;  
        }
    }
}
