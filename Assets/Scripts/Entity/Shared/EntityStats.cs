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

        public List<Effect> effects;
        public List<StatusEffectData> statusEffects;

        public ModifiableStat()
        {
            effects = new();
            statusEffects = new();
        }

        public void AddEffect(Effect effect)
        {
            effects.Add(effect);
            Refresh();
        }

        public void AddOrRefreshStatusEffect(IStatusEffect statusEffect, Entity source, Entity target)
        {
            var existing = statusEffects.First(e => e.statusEffect.Equals(statusEffect));
            if (existing != null)
            {
                existing.Reapply();
            }
            else
            {
                var newStatusEffect = new StatusEffectData(statusEffect, source, target);
                statusEffects.Add(newStatusEffect);
                Refresh();
            }
        }

        public void Refresh()
        {
            calculated = baseValue;

            foreach (var mod in effects)
            {
                calculated = mod.ImpactStat(calculated);
            }

            foreach(var mod in statusEffects)
            {
                calculated = mod.statusEffect.ImpactStat(calculated);
            }
        }

        public void Clear()
        {
            calculated = 0;
        }

        public void TickStatuses()
        {
            foreach(var status in statusEffects)
            {
                bool didFinish = status.OnTick();
                if (didFinish)
                {
                    statusEffects.Remove(status);
                }
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

        public bool OnTick()
        {
            timer += Time.deltaTime;
            return timer >= statusEffect.Duration;
        }

        public void Reapply()
        {
            timer = 0;
            // TODO figure out how the fuck to diferentiate status effects
            // that do or don't reapply on tick.
        }
    }
}