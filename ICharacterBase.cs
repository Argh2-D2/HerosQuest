using System.Collections.Generic;

namespace HerosQuest
{
    
    //Abstraction - showing important details of the objects
    public interface ICharacterBase
    {
        CharacterModel Allied { get; set; }
        string CharacterType { get; }
        int Energy { get; set; }
        PotionModel EnergyPotions { get; set; }
        int Health { get; set; }
        PotionModel HealthPotions { get; set; }
        int MaxEnergy { get; }
        int MaxHealth { get; }
        string Name { get; }
        string WeaponAction { get; }
        string WeaponName { get; }

        bool Attack(ICharacter aggressor, ICharacter target, int roll);
        string CanAttack(ICharacter pTarget, int weaponCount = 1);
        List<string> CharacterPrompt(IEnumerable<ICharacter> victims);
        int CheckMax(int value, int max);
        void Rest();
        void Status();
        bool TakeAction(ICharacter player, List<ICharacter> characters, bool successfulAction, int selection);
        void TakeTurn(ICharacter player, List<ICharacter> characters);
    }
}