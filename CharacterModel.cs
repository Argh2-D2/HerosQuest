using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace HerosQuest
{
    //Inheritance
    public class CharacterModel : ICharacterBase
    {
        public static Random rng = new Random();

        //Fields
        //'private'- Can't be accessed by any memebers that aren't of the same class
        //'readonly'- Allows member variable to be calculated at runtime
        private readonly string _name;
        private readonly int _maxEnergy;
        private readonly string _characterType;
        private readonly int _maxHealth;
        private readonly string _weaponName;
        private readonly string _weaponAction;
        private int _health;
        private int _energy;
        private PotionModel _energyPotions;
        private PotionModel _healthPotions;
        private CharacterModel _allied = null;
        

        //Constructor
        //'protected'- Accessible within its class and by derived class instances 
        protected CharacterModel(string name, string type, PlayerModel player)
        {
            _name = name;
            _characterType = type;
            _maxEnergy = player.Energy;
            _maxHealth = player.Health;
            _energyPotions = player.EnergyPotions;
            _healthPotions = player.HealthPotions;
            _weaponAction = player.WeaponAction;
            _weaponName = player.WeaponName;
        }
        

        //Properties
        //'get' - Returns value of the variable name
        //'set' - assigns a value to the name variable 
        public int Health
        {
            get { return _health; }
            set { _health = CheckMax(value, MaxHealth); }
        }

        public int Energy
        {
            get { return _energy; }
            set { _energy = CheckMax(value, MaxEnergy); }
        }

        public string Name
        {
            get { return _name; }
        }

        public int MaxHealth
        {
            get { return _maxHealth; }
        }

        public int MaxEnergy
        {
            get { return _maxEnergy; }
        }

        public PotionModel EnergyPotions
        {
            get { return _energyPotions; }
            set { _energyPotions = value; }
        }

        public PotionModel HealthPotions
        {
            get { return _healthPotions; }
            set { _healthPotions = value; }
        }

        public string CharacterType
        {
            get { return _characterType; }
        }

        public CharacterModel Allied
        {
            get { return _allied; }
            set { _allied = value; }
        }

        public string WeaponName
        {
            get { return _weaponName; }
        }

        public string WeaponAction
        {
            get { return _weaponAction; }
        }
        

        public List<string> CharacterPrompt(IEnumerable<ICharacter> victims)
        {
            List<string> options = new List<string>
            {
                "Rest.",
                "Take potion of healing.",
                "Take potion of vitality."
            };

            // attacks
            foreach (var victim in victims)
            {
                options.Add($"Use your { WeaponName } to attack the { victim.CharacterType }, { victim.Name }");
            }

            return options;
        }

        public static int PlayerMenu(ICharacter aggressor, IEnumerable<ICharacter> characters)
        {
            do
            {
                int max = 0;
                for (int i = 1; i <= aggressor.CharacterPrompt(characters).Count; i++)
                {
                    Console.WriteLine(i + ". " + aggressor.CharacterPrompt(characters)[i - 1]);
                    if (i > max)
                        max = i;
                }

                Console.WriteLine("Please enter a selection between " + 1 + " and " + max + " inclusive");
                string input = Console.ReadLine();

                int result;
                try
                {
                    result = int.Parse(input);
                }
                catch
                {
                    Console.WriteLine(input + " is not a number.");
                    continue;
                }

                if (result >= 1 && result <= max)
                {
                    return result;
                }
            } while (true);
        }

        public string CanAttack(ICharacter pTarget, int weaponCount = 1)
        {
            string error = "";
            if (pTarget.Health <= 0)
                error += pTarget.Name + " is already dead.";

            if (Energy <= 0)
                error += "You need at least 1 energy to attack.";

            if (pTarget == Allied)
                error += "You cannot attack " + pTarget + " because you are allied with them this turn.";

            if (weaponCount <= 0)
            {
                error += "You need at least one weapon to attck";
            }

            return error;
        }

        public int CheckMax(int value, int max)
        {
            if (value > max)
            {
                return max;
            }

            if (value < 0)
            {
                return 0;
            }

            return value;
        }

        public void Rest()
        {
            int energy = 3 + rng.Next(4);
            int health = 3 + rng.Next(4);
            Energy += (energy);
            Health += (health);

            Console.WriteLine("You are well rested.");
            Console.WriteLine("Your energy has increased by {0} to {1} / {2}.", energy, Energy, MaxEnergy);
            Console.WriteLine("Your health has increased by {0} to {1} / {2}.", health, Health, MaxHealth);
        }

        public void TakeTurn(ICharacter player, List<ICharacter> characters)
        {
            bool successfulAction = false;

            do
            {
                Console.WriteLine(player.Name + " it is your turn!");

                int selection = PlayerMenu(player, characters);

                successfulAction = player.TakeAction(player, characters, successfulAction, selection);

            } while (!successfulAction);

            if (Allied != null)
            {
                Console.WriteLine("You are no longer allied with " + Allied.Name);
                Allied = null;
            }
        }

        public bool TakeAction(ICharacter player, List<ICharacter> characters, bool successfulAction, int selection)
        {
            switch (selection)
            {
                case 1:
                    Rest();
                    successfulAction = true; // the rest action is always successful
                    break;
                case 2:
                    successfulAction = player.TakePotion(HealthPotions);
                    break;
                case 3:
                    successfulAction = player.TakePotion(EnergyPotions);
                    break;
                case 4:
                    successfulAction = Attack(player, characters[0]);
                    break;
                case 5:
                    successfulAction = Attack(player, characters[1]);
                    break;
            }

            return successfulAction;
        }

        private bool Attack(ICharacter aggressor, ICharacter target)
        {
            string error = CanAttack(aggressor);
            if (error.Length > 0)
                return false;

            int roll = rng.Next(1, 21);
            Energy--;

            return aggressor.Attack(aggressor, target, roll);
        }

        public bool Attack(ICharacter aggressor, ICharacter target, int roll)
        {
            if (roll < aggressor.AttackMiss)
            {
                Console.WriteLine("The " + aggressor.WeaponName + " misses " + target.Name + " completely!");
            }
            else if (roll < aggressor.AttackGraze)
            {
                Console.WriteLine("The " + aggressor.WeaponName + " grazes " + target.Name + "'s limb dealing 1 damage.");
                target.Health -= 1;
            }
            else if (roll < aggressor.AttackHit)
            {
                Console.WriteLine("The " + aggressor.WeaponName + " hits " + target.Name + "'s torso dealing 2 damage!");
                target.Health -= 2;
            }
            else
            {
                Console.WriteLine("The " + aggressor.WeaponName + " hits " + target.Name + "'s head dealing 3 damage!");
                target.Health -= 3;
            }

            return true;
        }

        public void Status()
        {
            string output = Name + " the " + CharacterType + ":" + Health + "/" + MaxHealth + " Health. " + Energy + "/" + MaxEnergy + " Energy. ";

            output += "\r\n" + Name + " has " + HealthPotions.Name + " health potions and " + EnergyPotions.Name + " energy potions.\r\n";

            if (Allied != null)
            {
                output += Name + " is currently allied with " + Allied.Name;
            }

            Console.WriteLine(output);
        }
    }
}
