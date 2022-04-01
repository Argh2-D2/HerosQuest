using System;
using System.Collections.Generic;
using static System.Net.Mime.MediaTypeNames;

namespace HerosQuest
{
    public class RangerModel : CharacterModel, ICharacter
    {
        #region fields
        private static readonly PlayerModel _player = new PlayerModel(
            health: rng.Next(10, 15),
            energy: rng.Next(4, 9),
            hPotion: new PotionModel("Health", rng.Next(1, 4)),
            ePotion: new PotionModel("Energy", rng.Next(1, 4)),
            weaponName: "Arrow",
            weaponAction: "Fire"
        );

        private readonly int _attackHit;
        private readonly int _attackGraze;
        private readonly int _attackMiss;

        private readonly int _MaxNumberOfArrows;
        private int _numberOfArrows;
        #endregion

        public RangerModel(string pName) : base(pName, "Ranger", _player)
        {
            _MaxNumberOfArrows = rng.Next(4, 9);
            NumberOfArrows = _MaxNumberOfArrows;

            Health = MaxHealth;
            Energy = MaxEnergy;
            _attackHit = 13;
            _attackGraze = 1;
            _attackMiss = 4;
        }

        #region properties
        public int AttackMiss { get { return _attackMiss; } }
        public int AttackGraze { get { return _attackGraze; } }
        public int AttackHit { get { return _attackHit; } }
        public int NumberOfArrows
        {
            get { return _numberOfArrows; }
            set { _numberOfArrows = CheckMax(value, _MaxNumberOfArrows); }
        }
        #endregion

        public new bool TakeAction(ICharacter player, List<ICharacter> characters, bool successfulAction, int selection)
        {
            switch (selection)
            {
                case 1:
                case 2:
                case 3:
                case 4:
                case 5:
                    successfulAction = base.TakeAction(player, characters, successfulAction, selection);
                    break;
                case 6:
                    successfulAction = PickUpArrows();
                    break;
            }

            return successfulAction;
        }

        public new bool Attack(ICharacter aggressor, ICharacter target, int roll)
        {
            NumberOfArrows--;

            base.Attack(aggressor, target, roll);

            if (roll < aggressor.AttackHit)
            {
                Console.WriteLine("You regain 1 energy.");
                aggressor.Health++;
            }
            else
            {
                Console.WriteLine("You regain 2 energy.");
                aggressor.Health += 2;
            }

            return true;
        }

        public new List<string> CharacterPrompt(IEnumerable<ICharacter> victims)
        {
            List<string> options = base.CharacterPrompt(victims);
            options.Add("Collect Arrows.");

            return options;
        }

        public new void Status()
        {
            string output = Name + " the " + CharacterType + ":" + Health + "/" + MaxHealth + " Health. " + Energy + "/" + MaxEnergy + " Energy. ";

            output += NumberOfArrows + "/" + _MaxNumberOfArrows + " arrows.";
            output += "\r\n" + Name + " has " + HealthPotions.Quantity + " health potions and " + EnergyPotions.Quantity + " energy potions.\r\n";

            if (Allied != null)
            {
                output += Name + " is currently allied with " + Allied.Name;
            }

            Console.WriteLine(output);
        }

        private bool PickUpArrows()
        {
            // error string used to detect is anything is wrong
            string error = "";

            if (Energy < 1)
            {
                error += "You need at least 1 energy to pick up arrows.";
            }

            if ((_MaxNumberOfArrows - NumberOfArrows) < 2)
            {
                error += "You cannot collect fewer than 2 arrows.";
            }


            // If anything is wrong output the error(s) and return false
            if (error.Length > 0)
            {
                Console.WriteLine(error);
                return false;
            }

            // nothing wrong, so do the action, output result and return true
            Energy--;

            int minimumArrows = Math.Min(2, _MaxNumberOfArrows - NumberOfArrows);
            int arrowsCollected = rng.Next(minimumArrows, _MaxNumberOfArrows - NumberOfArrows);
            NumberOfArrows += arrowsCollected;
            Console.WriteLine(Name + " the ranger picked up " + arrowsCollected + " and now has " + NumberOfArrows + "/" + _MaxNumberOfArrows);
            return true;
        }

        public ConsoleColor TextColor()
        {
            return ConsoleColor.Green;
        }
    }
}
