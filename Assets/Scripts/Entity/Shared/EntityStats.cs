using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.UIElements;

namespace Minigames.Fight
{
    [Serializable]
    public class EntityStats
    {
        public MovementStats movementStats;
        public CombatStats combatStats;
        

        public void Init(string overrideFilePath = "")
        {
            // TODO: figure out how to get the right file path
            movementStats.Init();
            combatStats.Init();
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

    [Serializable]
    public class CombatStats
    {
        public WeaponStats meleeWeaponStats;
        public WeaponStats projectileWeaponStats;
        public ModifiableStat maxHp = new();

        [NonSerialized]
        public float currentHp;
        [NonSerialized]
        public float DamageTakenThisSecond;
        
        public List<StatusEffectData> hpStatusEffects;
        public List<TimerEffectData> playerTimerEffects;

        public void Init()
        {
            meleeWeaponStats.Init();
            projectileWeaponStats.Init();
            maxHp.Init();

            if (hpStatusEffects == null)
            {
                hpStatusEffects = new();
            }
            if (playerTimerEffects == null)
            {
                playerTimerEffects = new();
            }

            currentHp = maxHp.Calculated;
        }

        public void AddHp(float hpToAdd)
        {
            currentHp += hpToAdd;
            currentHp = Mathf.Clamp(currentHp, 0, maxHp.Calculated);
        }

        public void TakeDamage(float damage)
        {
            currentHp -= damage;
            DamageTakenThisSecond += damage;
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

            foreach(var effect in playerTimerEffects)
            {
                effect.OnTick();
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

        public void AddTimerEffect(ITimerEffect timerEffect, Entity source)
        {
            var newStatusEffect = new TimerEffectData(timerEffect, source);
            playerTimerEffects.Add(newStatusEffect);
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
        [JsonIgnore]
        public float MaxRange;

        [JsonIgnore]
        public ProjectileController projectilePrefab;
        [JsonIgnore]
        public float currentAmmo;
        [JsonIgnore]
        private float _regenTimer;
        [JsonIgnore]
        public LayerMask targetLayers;
        [JsonIgnore]
        public LayerMask destroyOnImpactLayers;

        public Sprite sprite;

        public AnimatorController animation;

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

        

        public List<AoeEffect> AoeEffects = new();
        public List<Effect> OnHitEffects = new();
        public List<StatusEffectData> AmmoStatusEffects = new();

        public void Init()
        {
            baseDamage.Init();
            onHitDamage.Init();
            projectileMoveSpeed.Init();
            projectileLifeTime.Init();
            rateOfFire.Init();
            maxAmmo.Init();
            ammoRegenRate.Init();
            projectilesPerShot.Init();
            projectileSpread.Init();
            projectileSize.Init();

            currentAmmo = maxAmmo.Calculated;
            Platform.EventService.Dispatch(new PlayerAmmoUpdatedEvent((int)currentAmmo, (int)maxAmmo.Calculated));

            if (AoeEffects == null)
            {
                AoeEffects = new();
            }

            if (OnHitEffects == null)
            {
                OnHitEffects = new();
            }

            if (AmmoStatusEffects == null)
            {
                AmmoStatusEffects = new();
            }
        }

        public virtual void ConsumeAmmo(int ammoToConsume)
        {
            currentAmmo-= ammoToConsume;
            Platform.EventService.Dispatch(new PlayerAmmoUpdatedEvent((int)currentAmmo, (int)maxAmmo.Calculated));
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
                _regenTimer = 0;
                currentAmmo++;
                Platform.EventService.Dispatch(new PlayerAmmoUpdatedEvent((int)currentAmmo, (int)maxAmmo.Calculated));
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
    [Serializable]
    public class MovementStats
    {
        public ModifiableStat moveSpeed;

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
    [Serializable]
    // TODO handle types other than float
    public class ModifiableStat
    {
        [SerializeField]
        private float baseValue;
        public float BaseValue => baseValue;

        private float calculated;
        [JsonIgnore]
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
        [JsonIgnore]
        public List<StatModifierEffect> singleUseEffects;
        [JsonIgnore]
        public List<StatusEffectData> statusEffects;
        // If this effect is set, it will override all other effects and negate them
        public StatModifierEffect overrideEffect;

        public List<Effect> OnKillEffects = new();
        public List<Effect> OnDeathEffects = new();
        public List<Effect> OnTakeDamageEffects = new();
        public List<Effect> OnTimerEffects = new();
        public List<Effect> OnPurchaseEffects = new();

        public void Init()
        {
            if(flatEffects == null)
            {
                flatEffects = new();
            }

            if(compoundingEffects == null)
            {
                compoundingEffects = new();
            }

            if (singleUseEffects == null)
            {
                singleUseEffects = new();
            }

            if(statusEffects == null)
            {
                statusEffects = new();
            }

            RecalculateStat();
        }

        public void AddEffect(StatModifierEffect effect)
        {
            if (effect.statImpactType == StatImpactType.Flat)
            {
                flatEffects.Add(effect);
            }
            else if(effect.statImpactType == StatImpactType.Compounding)
            {
                compoundingEffects.Add(effect);
            }
            else
            {
                overrideEffect = effect;
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
            // Allows the designer to set a hard override for any stat with an effect
            if (overrideEffect != null)
            {
                calculated = overrideEffect.ImpactStat(calculated);
                return;
            }

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

    [Serializable]
    public class TimerEffectData
    {
        public float timer;
        public float tickRate;
        public ITimerEffect timerEffect;

        public Entity source;

        public TimerEffectData(ITimerEffect timerEffect, Entity source)
        {
            this.timerEffect = timerEffect;
            this.source = source;
            timer = 0;
        }

        public void OnTick()
        {
            timer += Time.deltaTime;
            if (timer >= tickRate)
            {
                var targets = timerEffect.GetTargets();
                timerEffect.OnTick(source, targets);
                timer = 0;
            }
        }
    }

    [Serializable]
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