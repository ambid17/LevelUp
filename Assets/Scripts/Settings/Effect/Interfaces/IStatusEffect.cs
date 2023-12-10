namespace Minigames.Fight
{
    public interface IStatusEffect
    {
        public float Duration { get; }
        public float TickRate { get; }
        public void RemoveEffect(Entity target);
        void OnTick(Entity target);
        public void ApplyStatEffect(ModifiableStat statToModify);
    }
}