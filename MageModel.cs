using System;
using System.Collections.Generic;

namespace HerosQuest
{
    //Inheritance
    public class MageModel : CharacterModel, ICharacter
    {
        //Fields
        private static readonly PlayerModel _player = new PlayerModel(
            health: rng.Next(8, 11),
            energy: rng.Next(6, 13),
            hPotion: new PotionModel("Health", rng.Next(2, 4)),
            ePotion: new PotionModel("Energy", rng.Next(1, 3)),
            weaponName: "Fireball",
            weaponAction: "Throw"
        );

        private readonly int _attackHit;
        private readonly int _attackGraze;
        private readonly int _attackMiss;
        

        public MageModel(string pName) : base(pName, "Mage", _player)
        {
            Health = MaxHealth;
            Energy = MaxEnergy;

            _attackHit = 17;
            _attackGraze = 8;
            _attackMiss = 3;

        }

        //Properties
        public int AttackMiss { get { return _attackMiss; } }

        public int AttackGraze { get { return _attackGraze; } }

        public int AttackHit { get { return _attackHit; } }
        

        public new List<string> CharacterPrompt(IEnumerable<ICharacter> victims)
        {
            List<string> options = base.CharacterPrompt(victims);
            options.Add("Cast healing spell on yourself.");

            foreach (ICharacter victim in victims)
            {
                options.Add("Cast healing spell on " + victim.Name + " the " + victim.CharacterType + ".");
            }

            return options;
        }

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
                    successfulAction = HealPlayer(player);
                    break;
                case 7:
                    successfulAction = HealPlayer(characters[0]);
                    break;
                case 8:
                    successfulAction = HealPlayer(characters[1]);
                    break;
            }

            return successfulAction;
        }

        private bool HealPlayer(ICharacter pTarget)
        {
            // error string used to detect is anything is wrong
            string error = "";

            if (pTarget.Health <= 0)
            {
                error += pTarget.Name + " is already dead.";
            }

            if (Energy <= 0)
            {
                error += "You need at least 1 energy to heal a player.";
            }

            // If anything is wrong output the error(s) and return false
            if (error.Length > 0)
            {
                Console.WriteLine(error);
                return false;
            }

            // nothing wrong, so do the action, output result and return true
            pTarget.Health += rng.Next(3, 7);
            if (pTarget != this)
            {
                pTarget.Allied = this;
            }
            return true;
        }

        public ConsoleColor TextColor()
        {
            return ConsoleColor.Cyan;
        }
    }
}
