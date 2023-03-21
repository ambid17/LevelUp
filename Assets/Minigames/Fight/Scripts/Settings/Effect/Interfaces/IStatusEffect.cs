namespace Minigames.Fight
{
    public interface IStatusEffect
    {
        public void TryApplyEffect(HitData hit);
        public void RemoveEffect(Entity target);
        void ApplyEffect(Entity target);
        void OnTick();
    }
}