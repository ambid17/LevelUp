using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Minigames.Fight
{
    [Serializable]
    public class CombatStats
    {
        public WeaponStats meleeWeaponStats;
        public WeaponStats projectileWeaponStats;
        public ModifiableStat maxHp = new();

        [JsonIgnore]
        public float currentHp;
        [JsonIgnore]
        public float DamageTakenThisSecond;
        [JsonIgnore]
        public List<StatusEffectData> hpStatusEffects;
        [JsonIgnore]
        public List<TimerEffectData> playerTimerEffects;

        public void Load(CombatStats stats)
        {
            maxHp = stats.maxHp;
            meleeWeaponStats.Load(stats.meleeWeaponStats);
            projectileWeaponStats.Load(stats.projectileWeaponStats);
        }

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

            foreach (var status in hpStatusEffects.ToList())
            {
                bool didFinish = status.OnTick();
                if (didFinish)
                {
                    hpStatusEffects.Remove(status);
                }
            }

            foreach (var effect in playerTimerEffects)
            {
                effect.OnTick();
            }
        }

        public void AddOrRefreshStatusEffect(IStatusEffect statusEffect, Entity source, Entity target)
        {
            var existing = hpStatusEffects.FirstOrDefault(e => e.statusEffect.Equals(statusEffect));
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
}
