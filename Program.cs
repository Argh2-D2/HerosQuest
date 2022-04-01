using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace HerosQuest
{
    class Program
    {
        private static bool gameMenu = true;

        static void Main()
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Welcome to hero's quest! - A quest for heroes!");

            while (gameMenu)
            {
                gameMenu = Game.Menu();
            }

            //string playAgain = "yes";
            //while (playAgain == "yes") // this loop gives users the opportunity to play again
            //{
            //    SetupCharacters();

            //    Character winner = GameLoop();

            //    Console.WriteLine("Congratulations " + winner.Name + ", you win!");
            //    do
            //    {
            //        Console.WriteLine("Would you like to play again? Enter yes or no");
            //        playAgain = Console.ReadLine();
            //    } while (playAgain != "yes" || playAgain != "no");

            //} // play again loop end
            

            Console.WriteLine("Thank you for playing Hero's Quest");
            Console.WriteLine("Press any key to exit.");
            Console.ReadKey();

        } // Main method end
    }
}
