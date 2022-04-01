using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HerosQuest
{
    public sealed class PlayerModel : IPlayer
    {
        public int Health { get; set; }

        public int Energy { get; set; }

        public PotionModel HealthPotions { get; set; }

        public PotionModel EnergyPotions { get; set; }

        public string WeaponName { get; set; }

        public string WeaponAction { get; set; }

        public PlayerModel(int health, int energy, PotionModel hPotion, PotionModel ePotion, string weaponName, string weaponAction)
        {
            Health = health;
            Energy = energy;
            HealthPotions = hPotion;
            EnergyPotions = ePotion;
            WeaponName = weaponName;
            WeaponAction = weaponAction;
        }
    }
}
