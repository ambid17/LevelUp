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
        public PlayerAnimationController AnimationController => _animationControllerOverride;
        public Camera PlayerCamera => playerCamera;
        public PlayerWeaponArmController WeaponArmController => weaponArmController;

        [SerializeField]
        private Camera playerCamera;
        [SerializeField]
        private PlayerWeaponArmController weaponArmController;

        private float _deathTimer;

        private PlayerAnimationController _animationControllerOverride;

        private bool debugDead;
        
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
                StartCoroutine(WaitForRevive());
            }
        }

        protected override void Update()
        {
            base.Update();
            if (Input.GetKeyDown(KeyCode.K))
            {
                TakeDamage(GameManager.SettingsManager.playerSettings.MaxHp);
            }
        }
        
        protected override void Die()
        {
            _deathTimer = 0;
            //GameManager.SettingsManager.progressSettings.ResetOnDeath();
            _animationControllerOverride.PlayDieAnimation();
            eventService.Dispatch<PlayerDiedEvent>();
        }
        
        private IEnumerator WaitForRevive()
        {
            while (!_animationControllerOverride.IsAnimFinished)
            {
                yield return null;
            }

            yield return new WaitForSeconds(GameManager.SettingsManager.incomeSettings.DeathTimer);

            CurrentHp = GameManager.SettingsManager.playerSettings.MaxHp;
            eventService.Dispatch<PlayerRevivedEvent>();
            _animationControllerOverride.ResetAnimations();
            _animationControllerOverride.PlayRunAnimation();
            transform.position = GameManager.RoomManager.StartRoom.Tilemap.cellBounds.center;
        }
    }
}