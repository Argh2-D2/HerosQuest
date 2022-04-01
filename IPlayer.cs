namespace HerosQuest
{
    //Abstraction
    public interface IPlayer
    {
        int Energy { get; set; }
        PotionModel EnergyPotions { get; set; }
        int Health { get; set; }
        PotionModel HealthPotions { get; set; }
        string WeaponAction { get; set; }
        string WeaponName { get; set; }
    }
}