using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HerosQuest
{
    public class PotionModel
    {
        private readonly string _name;

        public int Quantity { get; set; }
        public PotionModel(string name, int quantity)
        {
            _name = name;
            Quantity = quantity;
        }
        public string Name
        {
            get { return _name; }
        }
    }

    public static class PotionModelExtensions
    {
        public static bool TakePotion(this ICharacter character, PotionModel potion)
        {
            string error = "";
            int potionResult = 0;

            try
            {
                if (character.Energy < 1)
                    error += "You need at least 1 energy to use a potion.\r\n";

                if (potion.Quantity < 1)
                    error += "You do not have any " + potion.Name + " potions left.\r\n";

                if (error.Length > 0)
                    throw new Exception(error);

                potion.Quantity--;

                switch (potion.Name)
                {
                    case "Health":
                        potionResult = character.Health += character.MaxHealth / 2;
                        break;
                    case "Energy":
                        potionResult = character.Energy += character.MaxEnergy / 2;
                        break;
                }

                Console.WriteLine("The " + potion.Name + " potion increases\r\n" +
                    "your " + potion.Name + " is now " + potionResult);

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }
    }
}
