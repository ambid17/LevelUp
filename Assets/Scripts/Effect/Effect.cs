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
        OnTakeDamage,
        OnCraft,
        OnTimer
    }

    [Serializable]
    public class Effect : ScriptableObject, IEquatable<Effect>
    {
        [Header("Trigger info")]
        public int ExecutionOrder;
        public EffectTriggerType TriggerType;

        [Header("Set at runtime")]
        protected int _amountOwned;
        protected UpgradeCategory _upgradeCategory;
        protected EffectCategory _effectCategory;

        public virtual string GetDescription()
        {
            return string.Empty;
        }

        public bool Equals(Effect other)
        {
            if (other == null) return false;
            return other.GetType().Equals(this.GetType());
        }

        public virtual void GiveUpgradeInfo(int amountOwned, UpgradeCategory upgradeCategory, EffectCategory effectCategory)
        {
            _amountOwned = amountOwned;
            _upgradeCategory = upgradeCategory;
            _effectCategory = effectCategory;
        }

        public virtual void ApplyOverrides(EffectOverrides overrides)
        {
            throw new NotImplementedException();
        }

        public virtual void OnCraft(Entity target)
        {
            throw new NotImplementedException();
        }

        public virtual void Execute(Entity source, Entity target)
        {
            throw new NotImplementedException();
        }

        public virtual float ImpactStat(float stat)
        {
            return stat;
        }
    }
}