using System;

namespace Minigames.Fight
{
    [Serializable]
    public class MovementStats
    {
        public ModifiableStat moveSpeed;

        public void Load(MovementStats stats)
        {
            moveSpeed = stats.moveSpeed;
        }

        public void Init()
        {
            moveSpeed.Init();
        }

        public void TickStatuses()
        {
            moveSpeed.TickStatuses();
        }

        public void ClearAllStatusEffects()
        {
            moveSpeed.statusEffects.Clear();
        }
    }
}
