namespace Minigames.Fight
{
    public interface IStatusEffect
    {
        public void TryAdd(HitData hit);
        public void OnRemove(Entity target);
        void OnAdd(Entity target);
        void OnTick();
    }
}