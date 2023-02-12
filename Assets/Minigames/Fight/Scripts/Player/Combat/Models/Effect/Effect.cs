using System;
using System.Collections;
using Minigames.Fight;
using UnityEngine;

namespace Minigames.Fight
{
    public enum EffectTriggerType
    {
        OnHit,
        OnKill,
        OnDeath,
        OnTakeDamage
    }

    [CreateAssetMenu(fileName = "Effect", menuName = "ScriptableObjects/Fight/Effect", order = 1)]
    [Serializable]
    public abstract class Effect : ScriptableObject, IComparable<Effect>
    {
        public string Name;
        public string Description;
        public Sprite Icon;
        public int AmountOwned;
        public int ExecutionOrder;
        public abstract EffectTriggerType TriggerType { get; }
        public int SpawnWeight = 1;

        public int CompareTo(Effect other)
        {
            return ExecutionOrder.CompareTo(other.ExecutionOrder);
        }
    }
}