using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;

namespace Minigames.Fight
{
    [Serializable]
    public class WeaponStats
    {
#if UNITY_EDITOR
        [SerializeField] private PolygonCollider2D collider;
#endif
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
        [JsonIgnore]
        public Sprite sprite;
        [JsonIgnore]
        public AnimatorController animation;
        [JsonIgnore]
        public Vector2[] colliderPoints;

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
        public ModifiableStat projectilePenetration = new();

        public List<AoeEffect> AoeEffects = new();
        public List<Effect> OnHitEffects = new();
        public List<Effect> OnKillEffects = new();
        [JsonIgnore]
        public List<StatusEffectData> AmmoStatusEffects = new();

        public void Load(WeaponStats weaponStats)
        {
            baseDamage = weaponStats.baseDamage;
            onHitDamage = weaponStats.onHitDamage;
            projectileMoveSpeed = weaponStats.projectileMoveSpeed;
            projectileLifeTime = weaponStats.projectileLifeTime;
            rateOfFire = weaponStats.rateOfFire;
            maxAmmo = weaponStats.maxAmmo;
            ammoRegenRate = weaponStats.ammoRegenRate;
            projectilesPerShot = weaponStats.projectilesPerShot;
            projectileSpread = weaponStats.projectileSpread;
            projectileSize = weaponStats.projectileSize;
        }

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

#if UNITY_EDITOR

        public void GenerateColliderPoints()
        {
            colliderPoints = collider.GetPath(0);
        }
#endif

        public virtual void ConsumeAmmo(int ammoToConsume)
        {
            currentAmmo -= ammoToConsume;
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
}
