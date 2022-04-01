using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace HerosQuest
{
    public class Game
    {
        public static ICharacter Winner { get; set; }
        private static BarbarianModel Barbarian;
        private static MageModel Mage;
        private static RangerModel Ranger;
        public static bool Menu()
        {
            SetupCharacters();
            Winner = GameLoop();
            Console.WriteLine("Congratulations " + Winner.Name + ", you win!");

            return PlayAgain();
        }

        /// <summary>
        /// Sets up the three heroes by asking the user to input names for each
        /// </summary>
        /// <param name="ranger">Ranger character to be set up</param>Output
        /// <param name="mage">Mage character to be set up</param>
        /// <param name="barbarian">Barbarian character to be set up</param>
        private static void SetupCharacters()
        {
            string name = NewCharacter("Player 1 - You are the Ranger! Name your character!", ConsoleColor.Green);
            Ranger = new RangerModel(name);
            Console.WriteLine("Welcome " + Ranger.Name + ", the fastest ranger in all of Hullidian!");

            name = NewCharacter("Player 2 - You are the Mage! Name your character!", ConsoleColor.Cyan);
            Mage = new MageModel(name);
            Console.WriteLine("Welcome " + Mage.Name + ", the wisest mage in all of Hullidian!");

            name = NewCharacter("Player 3 - You are the Barbarian! Name your character!", ConsoleColor.Red);
            Barbarian = new BarbarianModel(name);
            Console.WriteLine("Welcome " + Barbarian.Name + ", the strongest barbarian in all of Hullidian!");
        }

        private static string NewCharacter(string message, ConsoleColor forecolor)
        {
            string name;
            do
            {
                try
                {
                    Console.ForegroundColor = forecolor;
                    Console.WriteLine(message);
                    name = Console.ReadLine();
                    if (name.Length == 0 || !name.All(char.IsLetter))
                        throw new Exception();
                    break;
                }
                catch (Exception)
                {
                    Console.WriteLine("Name should only contain letters.");
                    continue;
                }
            } while (true);

            return name;
        }

        private static ICharacter GameLoop()
        {
            int turn = 0;
            var players = new List<ICharacter>()
            {
                Ranger,
                Mage,
                Barbarian,
            };
            while (Winner == null)
            {
                /* Turn order is always ranger (the quickest), then mage, then barbarian (the slowest).
                 * Before each character takes their turn a check is performed to see if they are still alive.
                 * After each character takes their turn there is a check to see if the other two characters are still alive or not.
                 * if they are not then the character who just too their turn is assigned to the winner Character reference, and the
                 * continue keywork skips ahead to the loop condition, breaking the loop.
                 */

                var player = players[turn % 3];
                var opponents = players.Where(x => !x.Equals(player));

                if (player.Health > 0)
                {
                    OutputState();
                    Console.ForegroundColor = player.TextColor();
                    player.TakeTurn(player, opponents.ToList());
                    if (opponents.All(x => x.Health <= 0))
                    {
                        Winner = player;
                        continue;
                    }
                }

                turn++;
            }; // winner == null loop end
            return Winner;
        }

        private static bool PlayAgain()
        {
            do
            {
                Console.WriteLine("Would you like to play again? Enter yes or no");
                switch (Console.ReadLine().ToUpper())
                {
                    case null:
                    case "N":
                    case "NO":
                        return false;
                    case "Y":
                    case "YES":
                        return true;
                    default:
                        break;
                }
            } while (true);
        }

        public static void OutputState()
        {
            Console.ForegroundColor = Ranger.TextColor();
            Ranger.Status();
            Console.ForegroundColor = Mage.TextColor();
            Mage.Status();
            Console.ForegroundColor = Barbarian.TextColor();
            Barbarian.Status();
        }

        //Detects whether user inputs upper and lower case letter
        //if not throws exeception
        public static string ValidateName(string name)
        {
            if (name.Length == 0 || !name.All(char.IsLetter))
                throw new Exception();

            return name;
        }
    }
}