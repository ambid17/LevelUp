namespace Minigames.Fight
{
    public interface IStatusEffect
    {
        public float Duration { get; }
        public float TickRate { get; }
        void OnTick(Entity target);
        public float ImpactStat(float stat);
    }
}