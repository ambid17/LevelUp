using Minigames.Fight;

public interface IImpactsStat
{
    /// <summary>
    /// Used to know which order is should be Calculated in ModifiableStat
    /// </summary>
    public StatImpactType statImpactType { get; }
    public float ImpactStat(float stat);
}
