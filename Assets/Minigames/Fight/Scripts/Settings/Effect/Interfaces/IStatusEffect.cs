namespace Minigames.Fight
{
    public interface IStatusEffect
    {
        public float Duration { get; }
        public float TickRate { get; }
        public void TryApplyEffect(HitData hit);
        public void RemoveEffect(Entity target);
        void ApplyEffect(Entity target);
        void OnTick(Entity target);
    }
}