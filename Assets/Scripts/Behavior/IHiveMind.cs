namespace Minigames.Fight
{
    public interface IHiveMind
    {
        int Id { get; set; }
        HiveMindBehaviorData myBehaviorData { get;}
    }
}