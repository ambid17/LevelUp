using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

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

        public void ClearAllStatusEffects()
        {
            movementStats.ClearAllStatusEffects();
            combatStats.ClearAllStatusEffects();
        }
    }

    public class CombatStats
    {
        public WeaponStats meleeWeaponStats;
        public WeaponStats projectileWeaponStats;
        public ModifiableStat maxHp = new();

        public float currentHp;
        public float DamageTakenThisSecond;
        
        public List<StatusEffectData> hpStatusEffects;
        public List<TimerEffectData> playerAoeEffects;

        public CombatStats()
        {
        }

        public void TickStatuses()
        {
            meleeWeaponStats.TickStatuses();
            projectileWeaponStats.TickStatuses();
            maxHp.TickStatuses();

            foreach (var status in hpStatusEffects)
            {
                bool didFinish = status.OnTick();
                if (didFinish)
                {
                    hpStatusEffects.Remove(status);
                }
            }
        }

        public void AddOrRefreshStatusEffect(IStatusEffect statusEffect, Entity source, Entity target)
        {
            var existing = hpStatusEffects.First(e => e.statusEffect.Equals(statusEffect));
            if (existing != null)
            {
                existing.Reapply();
            }
            else
            {
                var newStatusEffect = new StatusEffectData(statusEffect, source, target);
                hpStatusEffects.Add(newStatusEffect);
            }
        }

        public void ClearAllStatusEffects()
        {
            meleeWeaponStats.ClearAllStatusEffects();
            projectileWeaponStats.ClearAllStatusEffects();
            maxHp.statusEffects.Clear();
            hpStatusEffects.Clear();
        }
    }

    [Serializable]
    public class WeaponStats
    {
        public float MaxRange => projectileLifeTime.Calculated * projectileMoveSpeed.Calculated;

        public ProjectileController projectilePrefab;

        public ModifiableStat baseDamage = new();
        public ModifiableStat onHitDamage = new();
        public ModifiableStat projectileMoveSpeed = new();
        public ModifiableStat projectileLifeTime = new();
        public ModifiableStat rateOfFire = new();
        public ModifiableStat maxAmmo = new();
        public ModifiableStat ammoRegenRate = new();
        public ModifiableStat projectilesPerShot = new();
        public ModifiableStat projectileSpread = new();
        public ModifiableStat projectileSize = new();

        public float currentAmmo;

        public List<AoeEffect> AoeEffects = new();
        public List<Effect> OnHitEffects = new();
        public List<StatusEffectData> ammoStatusEffects = new();

        private float _regenTimer;

        public virtual void ConsumeAmmo(int ammoToConsume)
        {
            currentAmmo-= ammoToConsume;
        }

        public virtual void TryRegenAmmo() 
        {
            if (currentAmmo >= maxAmmo.Calculated)
            {
                return;
            }
            _regenTimer += Time.deltaTime;
            if (_regenTimer >= ammoRegenRate.Calculated)
            {
                currentAmmo++;
            }
        }

        public virtual void TickStatuses()
        {
            baseDamage.TickStatuses();
            onHitDamage.TickStatuses();
            projectileMoveSpeed.TickStatuses();
            projectileLifeTime.TickStatuses();
        }

        public virtual void ClearAllStatusEffects()
        {
            baseDamage.statusEffects.Clear();
            onHitDamage.statusEffects.Clear();
            projectileMoveSpeed.statusEffects.Clear();
            projectileLifeTime.statusEffects.Clear();
        }
    }

    public class MovementStats
    {
        public ModifiableStat moveSpeed;

        public void TickStatuses()
        {
            moveSpeed.TickStatuses();
        }

        public void ClearAllStatusEffects()
        {
            moveSpeed.statusEffects.Clear();
        }
    }

    // TODO handle types other than float
    public class ModifiableStat
    {
        private float baseValue;
        public float BaseValue => baseValue;

        private float calculated;
        public float Calculated
        {
            get
            {
                singleUseEffects.Clear();
                return calculated;
            }
        }

        public List<StatModifierEffect> flatEffects;
        public List<StatModifierEffect> compoundingEffects;
        public List<StatModifierEffect> singleUseEffects;
        public List<StatusEffectData> statusEffects;

        public ModifiableStat()
        {
            flatEffects = new();
            compoundingEffects = new();
            statusEffects = new();
        }

        public void AddEffect(StatModifierEffect effect)
        {
            if (effect.statImpactType == StatImpactType.Flat)
            {
                flatEffects.Add(effect);
            }
            else
            {
                compoundingEffects.Add(effect);
            }
            RecalculateStat();
        }

        public void AddSingleUseEffect(StatModifierEffect effect)
        {
            singleUseEffects.Add(effect);
            RecalculateStat();
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
                RecalculateStat();
            }
        }

        public void RecalculateStat()
        {
            calculated = baseValue;

            foreach (var effect in flatEffects)
            {
                calculated = effect.ImpactStat(calculated);
            }

            foreach (var effect in compoundingEffects)
            {
                calculated = effect.ImpactStat(calculated);
            }

            foreach (var effect in singleUseEffects)
            {
                calculated = effect.ImpactStat(calculated);
            }

            foreach (var effect in statusEffects)
            {
                calculated = effect.statusEffect.ImpactStat(calculated);
            }
        }

        public void Clear()
        {
            calculated = 0;
        }

        public void TickStatuses()
        {
            foreach (var status in statusEffects)
            {
                bool didFinish = status.OnTick();
                if (didFinish)
                {
                    statusEffects.Remove(status);
                    RecalculateStat();
                }
            }
        }

        public void OverrideStat(float value)
        {
            calculated = value;
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
        public float tickTimer;
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
            tickTimer += Time.deltaTime;

            if (tickTimer > statusEffect.TickRate)
            {
                statusEffect.OnTick(source, target);
                tickTimer = 0;
            }

            bool didFinish = timer >= statusEffect.Duration;
            if (didFinish)
            {
                statusEffect.OnComplete();
            }

            return didFinish;
        }

        public void Reapply()
        {
            timer = 0;
            // TODO figure out how the fuck to diferentiate status effects
            // that do or don't reapply on tick.
        }
    }
}