using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Minigames.Fight;
using UnityEngine;

namespace Minigames.Fight
{
    public class PlayerEntity : Entity
    {
        private float _deathTimer;
        
        public float CurrentHp
        {
            get => Stats.currentHp;
            set
            {
                float newHp = value;
                // if adding to player hp, clamp it to max
                newHp = Mathf.Clamp(newHp, 0, GameManager.SettingsManager.playerSettings.MaxHp);
                Stats.currentHp = newHp;

                float hpPercent = Stats.currentHp / GameManager.SettingsManager.playerSettings.MaxHp;
                eventService.Dispatch(new PlayerHpUpdatedEvent(hpPercent));
            }
        }

        protected override void Awake()
        {
            base.Awake();
            
            Weapon equippedWeapon = GameManager.SettingsManager.weaponSettings.equippedWeapon;
            var weaponGo = Instantiate(equippedWeapon.Prefab, transform);
            var weaponController = weaponGo.GetComponent<WeaponController>();
            weaponController.Setup(equippedWeapon);
            WeaponController = weaponController;
        }
        
        protected override void Setup()
        {
            base.Setup();
            Stats.currentHp = GameManager.SettingsManager.playerSettings.MaxHp;
            Stats.OnHitEffects = GameManager.SettingsManager.effectSettings.OnHitEffects.OrderBy(e => e.ExecutionOrder).ToList();
        }

        protected override void Update()
        {
            base.Update();
            if (IsDead)
            {
                WaitForRevive();
            }
        }
        
        public override void TakeHit(HitData hit)
        {
            float damage = hit.CalculateDamage();
            CurrentHp -= damage;
            VisualController.StartDamageFx(damage);

            if (IsDead)
            {
                Die();
            }
        }
        
        protected override void Die()
        {
            _deathTimer = 0;
            GameManager.SettingsManager.progressSettings.ResetOnDeath();
            eventService.Dispatch<PlayerDiedEvent>();
        }
        
        private void WaitForRevive()
        {
            _deathTimer += Time.deltaTime;

            if (_deathTimer > GameManager.SettingsManager.incomeSettings.DeathTimer)
            {
                CurrentHp = GameManager.SettingsManager.playerSettings.MaxHp;
                eventService.Dispatch<PlayerRevivedEvent>();
            }
        }
    }
}