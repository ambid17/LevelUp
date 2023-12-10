using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Minigames.Fight
{
    public class EntityStats
    {
        public MovementStats movementStats;
        public CombatStats combatStats;

        // Effects
        public List<Effect> OnKillEffects = new();
        public List<Effect> OnDeathEffects = new();
        public List<Effect> OnTakeDamageEffects = new();
        public List<Effect> OnTimerEffects = new();
        public List<Effect> OnPurchaseEffects = new();

        public EntityStats()
        {
            movementStats = new MovementStats();
            combatStats = new CombatStats();
        }

        public void TakeDamage(float damage)
        {
            combatStats.currentHp -= damage;
            combatStats.DamageTakenThisSecond += damage;
        }

        public void TickStatuses()
        {
            movementStats.TickStatuses();
            combatStats.TickStatuses();
        }
    }

    public class CombatStats
    {
        public ModifiableStat baseDamage;

        public ModifiableStat onHitDamage;

        public ModifiableStat projectileMoveSpeed;

        public ModifiableStat projectileLifeTime;

        public ModifiableStat maxHp;
        public float currentHp;
        public float DamageTakenThisSecond;

        public List<Effect> OnHitEffects = new();

        public CombatStats()
        {
        }

        public void TickStatuses()
        {
            
        }
    }

    public class MovementStats
    {
        public ModifiableStat moveSpeed;

        public void TickStatuses()
        {
            moveSpeed.TickStatuses();
        }
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

        private List<StatusEffectData> effectsImpactingStat;
        private List<StatusEffectData> EffectsImpactingStat
        {
            get { return effectsImpactingStat;}
            set
            {
                if(effectsImpactingStat == null)
                {
                    effectsImpactingStat = new List<StatusEffectData>();
                }

                effectsImpactingStat = value;
            }
        }

        public void AddOrRefreshStatusEffect(IStatusEffect statusEffect, Entity source, Entity target)
        {
            var existing = EffectsImpactingStat.First(e => e.statusEffect.Equals(statusEffect));
            if (existing != null)
            {
                existing.Reapply();
            }
            else
            {
                var newStatusEffect = new StatusEffectData(statusEffect, source, target);
                EffectsImpactingStat.Add(newStatusEffect);

                statusEffect.ApplyStatEffect(this);
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

        public void TickStatuses()
        {
            foreach(var status in effectsImpactingStat)
            {
                status.OnTick();
            }
        }
    }

    public class TimerEffectData
    {
        public float timer;
        public float tickRate;
        public Effect myEffect;

        public Entity source;
        public Entity target;

        public void OnTick()
        {
            timer += Time.deltaTime;
            if (timer >= tickRate)
            {
                myEffect.Execute(source, target);
                timer = 0;
            }
        }
    }

    public class StatusEffectData
    {
        public float timer;
        public IStatusEffect statusEffect;
        public Entity source;
        public Entity target;


        public StatusEffectData(IStatusEffect statusEffect, Entity source, Entity target)
        {
            this.statusEffect = statusEffect;
            this.source = source;
            this.target = target;

            timer = 0;
        }

        public void OnTick()
        {
            // myEffect.Tick(effectedEntity);
            timer += Time.deltaTime;
            if (timer >= statusEffect.Duration)
            {
                // effectedEntity.AppliedStatusEffects.Remove(this);
            }
        }

        public void Reapply()
        {
            // TODO figure out how the fuck to diferentiate status effects
            // that do or don't reapply on tick.
        }
    }
}