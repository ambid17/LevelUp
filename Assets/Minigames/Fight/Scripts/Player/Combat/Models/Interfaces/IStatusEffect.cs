namespace Minigames.Fight
{
    public interface IStatusEffect
    {
        public void TryAdd(DamageWorksheet worksheet);
        public void OnRemove(Entity target);
        void OnAdd(Entity target);
        void OnTick();
    }
}