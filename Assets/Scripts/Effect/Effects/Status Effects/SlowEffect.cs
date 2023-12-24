using System;
using Minigames.Fight;
using Unity.Burst.CompilerServices;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Minigames.Fight
{
    [CreateAssetMenu(fileName = "SlowEffect", menuName = "ScriptableObjects/Effects/SlowEffect", order = 1)]
    [Serializable]
    public class SlowEffect : Effect, IStatusEffect
    {
        [Header("Effect specific")]
        public float duration = 2f;
        public float Duration => duration;
        public float TickRate => 0;

        public float slowAmount = 0.5f;
        public float chanceScalar = 0.01f;
        

        public float slowChance = 0.5f;
        public float SlowChance => slowChance + (chanceScalar * _amountOwned);
        
        private readonly string _description = "{0}% chance to slow enemies by {1}% for {2} seconds";
        
        public override string GetDescription()
        {
            return string.Format(_description, SlowChance * 100, slowAmount * 100, duration);
        }

        public override void OnCraft(Entity target)
        {
            if (_upgradeCategory == UpgradeCategory.Range)
            {
                target.Stats.combatStats.projectileWeaponStats.OnHitEffects.Add(this);
            }
            else
            {
                target.Stats.combatStats.meleeWeaponStats.OnHitEffects.Add(this);
            }
        }

        public override void Execute(Entity source, Entity target)
        {
            bool doesSlow = Random.value < SlowChance;
            if (doesSlow)
            {
                target.Stats.movementStats.moveSpeed.AddOrRefreshStatusEffect(this, source, target);
            }
        }

        public override float ImpactStat(float stat)
        {
            return stat * slowAmount;
        }

        public void OnTick(Entity source, Entity target)
        {
        }

        public void OnComplete()
        {
        }
    }
}