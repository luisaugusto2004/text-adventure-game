﻿using Core;
using System.Security.Cryptography;

namespace Util
{
    class RollDice
    {
        public static int Roll(int rolls, int faces)
        {
            List<int> values = new List<int>(); 
            for (int i = 0; i < rolls; i++)
            {
                int dice_roll = RandomNumberGenerator.GetInt32(1, faces + 1);
                values.Add(dice_roll);
            }

            return values.Sum();  
        }
    }
}
