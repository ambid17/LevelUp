using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Minigames.Fight;
using UnityEditor;
using UnityEngine;

namespace Minigames.Fight
{
    public class PlayerEntity : Entity
    {
        public WeaponController WeaponController => _equippedWeaponController;
        public PlayerAnimationController AnimationController => _animationControllerOverride;
        public Camera PlayerCamera => playerCamera;

        [SerializeField]
        private Camera playerCamera;

        private PlayerWeaponController _equippedWeaponController;
        private float _deathTimer;
        private PlayerProjectileWeaponController _projectileWeaponController;
        private PlayerMeleeWeaponController _meleeWeaponController;
        private PlayerAnimationController _animationControllerOverride;
        
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

        public bool CanMove = true;

        protected override void Awake()
        {
            base.Awake();

            SetupWeaponController();
        }

        private void SetupWeaponController()
        {
            _projectileWeaponController = GetComponent<PlayerProjectileWeaponController>();
            _meleeWeaponController = GetComponent<PlayerMeleeWeaponController>();
            _equippedWeaponController = _projectileWeaponController;
            _equippedWeaponController.IsEquipped = true;
        }
        
        protected override void Setup()
        {
            base.Setup();
            Stats.currentHp = GameManager.SettingsManager.playerSettings.MaxHp;
            eventService.Add<OnHitEffectUnlockedEvent>(SetupOnHitEffects);
            SetupOnHitEffects(); // go ahead and query the onHit effects that were populated on load
            _animationControllerOverride = animationController as PlayerAnimationController;
        }

        private void SetupOnHitEffects()
        {
            Stats.OnHitEffects = GameManager.SettingsManager.effectSettings.OnHitEffects.OrderBy(e => e.ExecutionOrder).ToList();
        }

        public override void TakeDamage(float damage)
        {

            CurrentHp -= damage;

            // Damage FX are confusing if a hit only applies status effects.
            if (damage > 0)
            {
                VisualController.StartDamageFx(damage);
            }

            if (IsDead)
            {
                Die();
            }
        }

        protected override void Update()
        {
            base.Update();
            if (IsDead)
            {
                WaitForRevive();
            }
            if (Input.mouseScrollDelta.y != 0)
            {
                _equippedWeaponController.IsEquipped = false;
                _equippedWeaponController = SwitchWeapon();
                _equippedWeaponController.IsEquipped = true;
            }
        }

        private PlayerWeaponController SwitchWeapon()
        {
            if (_equippedWeaponController == _projectileWeaponController)
            {
                Debug.Log("equipped melee");
                return _meleeWeaponController;
            }
            Debug.Log("equipped projectile");
            return _projectileWeaponController;
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