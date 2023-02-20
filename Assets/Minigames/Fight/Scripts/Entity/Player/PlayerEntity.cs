using System.Collections;
using System.Collections.Generic;
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
            Stats.OnHitEffects = GameManager.SettingsManager.effectSettings.OnHitEffects;
            eventService.Add<OnPlayerDamageEvent>(TakeDamage);
        }

        protected override void Update()
        {
            base.Update();
            if (IsDead)
            {
                WaitForRevive();
            }
        }
        
        public void TakeDamage(OnPlayerDamageEvent eventType)
        {
            // TODO: calculate based on resistances
            CurrentHp -= eventType.Damage;
            eventService.Dispatch<PlayerDamagedEvent>();

            if (IsDead)
            {
                _deathTimer = 0;
                eventService.Dispatch<PlayerDiedEvent>();
            }
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