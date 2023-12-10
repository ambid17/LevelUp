namespace Minigames.Fight
{
    public interface IStatusEffect
    {
        public float Duration { get; }
        public float TickRate { get; }
        public void OnTick(Entity source, Entity target);
        public float ImpactStat(float stat);
        public void OnComplete();
    }
}