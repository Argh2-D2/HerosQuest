using System;
using static System.Net.Mime.MediaTypeNames;

namespace HerosQuest
{
    //inheritance
    public class BarbarianModel : CharacterModel, ICharacter
    {
        
        //inheritance
        private static readonly PlayerModel _player = new PlayerModel(
            health: rng.Next(14, 19),
            energy: rng.Next(8, 13),
            hPotion: new PotionModel("Health", rng.Next(1, 3)),
            ePotion: new PotionModel("Energy", rng.Next(2, 4)),
            weaponName: "Axe",
            weaponAction: "Swing"
        );

        private readonly int _attackHit;
        private readonly int _attackGraze;
        private readonly int _attackMiss;

        private readonly int _maxRage;
        private int _rage = 0;
        
        //inheritance
        public BarbarianModel(string pName) : base(pName, "Barbarian", _player)
        {
            _maxRage = rng.Next(4, 9);
            Health = MaxHealth;
            Energy = MaxEnergy;

            _attackHit = 17;
            _attackGraze = 9;
            _attackMiss = 4;
        }

        public int Rage
        {
            get { return _rage; }
            set { _rage = CheckMax(value, _maxRage); }
        }

        public int AttackMiss { get { return _attackMiss; } }

        public int AttackGraze { get { return _attackGraze; } }

        public int AttackHit { get { return _attackHit; } }


        public new void Status()
        {
            string output = Name + " the " + CharacterType + ":" + Health + "/" + MaxHealth + " Health. " + Energy + "/" + MaxEnergy + " Energy. ";
            output += Rage + "/" + _maxRage + " rage.";

            output += "\r\n" + Name + " has " + HealthPotions.Name + " health potions and " + EnergyPotions.Name + " energy potions.\r\n";

            if (Allied != null)
            {
                output += Name + " is currently allied with " + Allied.Name;
            }

            Console.WriteLine(output);
        }

        public new bool Attack(ICharacter aggressor, ICharacter target, int roll)
        {
            int damageMultiplier = 1;
            if (Rage >= _maxRage)
            {
                Console.WriteLine("RAGING STRIKE - THIS STRIKE WILL DEAL DOUBLE DAMAGE!");
                damageMultiplier = 2;
                Rage = 0;
            }

            if (roll < aggressor.AttackMiss)
            {
                Console.WriteLine("The " + WeaponName + " misses " + target.Name + " completely!");
                Rage += 4;
            }
            else if (roll < aggressor.AttackGraze)
            {
                Console.WriteLine("The " + WeaponName + " grazes " + target.Name + "'s limb dealing " + (damageMultiplier * 2) + " damage!");
                target.Health -= (damageMultiplier * 2);
                Rage += 3;
            }
            else if (roll < aggressor.AttackHit)
            {
                Console.WriteLine("The " + WeaponName + " hits " + target.Name + "'s torso dealing " + (damageMultiplier * 3) + " damage.");
                target.Health -= (damageMultiplier * 3);
                Rage += 2;
            }
            else
            {
                Console.WriteLine("The " + WeaponName + " hits " + target.Name + "'s head dealing " + (damageMultiplier * 4) + " damage.");
                target.Health -= (damageMultiplier * 4);
                Rage += 1;
            }

            return true;
        }

        public ConsoleColor TextColor()
        {
            return ConsoleColor.Red;
        }
    }
}
