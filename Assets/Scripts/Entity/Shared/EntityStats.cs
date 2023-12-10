using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Minigames.Fight
{
    public class EntityStats
    {
        public float currentHp;
        public float maxHp;
        public float DamageTakenThisSecond;
        public List<StatusEffectInstance> StatusEffects = new();

        public MovementStats movementStats;
        public CombatStats combatStats;

        // Effects
        
        public List<Effect> OnKillEffects = new();
        public List<Effect> OnDeathEffects = new();
        public List<Effect> OnTakeDamageEffects = new();
        public List<Effect> OnTimerEffects = new();
        public List<Effect> OnPurchaseEffects = new();

        public void TakeDamage(float damage)
        {
            currentHp -= damage;
            DamageTakenThisSecond += damage;
        }

        public bool AddStatusEffect(StatusEffectInstance instance)
        {
            if (StatusEffects.Contains(instance))
            {
                StatusEffects[StatusEffects.IndexOf(instance)].remainingTime = instance.remainingTime;
                return false;
            }
            else
            {
                StatusEffects.Add(instance);
                return true;
            }
        }

        public void SetupFromEnemy(EnemyStats enemyStats)
        {
            currentHp = enemyStats.MaxHp;
            maxHp = enemyStats.MaxHp;
        }
    }

    public class CombatStats
    {
        public ModifiableStat baseDamage;

        public ModifiableStat onHitDamage;

        public ModifiableStat projectileMoveSpeed;

        public ModifiableStat projectileLifeTime;

        public List<Effect> OnHitEffects = new();
    }

    public class MovementStats
    {
        public ModifiableStat moveSpeed;
    }

    // TODO handle types other than float
    public class ModifiableStat
    {
        private float baseValue;
        public float BaseValue => baseValue;

        private float calculated;
        public float Calculated => calculated;

        private List<float> baseModifiers;
        public List<float> BaseModifiers
        {
            get { return baseModifiers; }
            set {
                if (baseModifiers == null)
                {
                    baseModifiers = new List<float>();
                }
                baseModifiers = value;
                Refresh();
            }
        }
        private List<float> compoundingModifiers;
        public List<float> CompoundingModifiers
        {
            get { return compoundingModifiers; }
            set
            {
                if(compoundingModifiers == null)
                {
                    compoundingModifiers = new List<float>();
                }
                compoundingModifiers = value;
                Refresh();
            }
        }

        public void Refresh()
        {
            calculated = baseValue;

            foreach (var mod in baseModifiers)
            {
                calculated += mod;
            }

            foreach (var mod in compoundingModifiers)
            {
                calculated *= mod;
            }
        }

        public void Clear()
        {
            BaseModifiers.Clear();
            CompoundingModifiers.Clear();
        }
    }
}