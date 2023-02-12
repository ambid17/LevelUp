using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Minigames.Fight
{

    public class PlayerStatusController : FightBehavior
    {
        [SerializeField]
        private float _currentPlayerHp;

        public float CurrentPlayerHp
        {
            get => _currentPlayerHp;
            set
            {
                float newHp = value;
                // if adding to player hp, clamp it to max
                newHp = Mathf.Clamp(newHp, 0, GameManager.SettingsManager.playerSettings.MaxHp);
                _currentPlayerHp = newHp;

                float hpPercent = _currentPlayerHp / GameManager.SettingsManager.playerSettings.MaxHp;
                eventService.Dispatch(new PlayerHpUpdatedEvent(hpPercent));
            }
        }

        private float _deathTimer;
        private bool _isDead;
        public bool IsDead => _isDead;

        void Start()
        {
            CurrentPlayerHp = GameManager.SettingsManager.playerSettings.MaxHp;
            eventService.Add<OnPlayerDamageEvent>(TakeDamage);
        }

        void Update()
        {
            if (_isDead)
            {
                WaitForRevive();
            }
        }

        public void TakeDamage(OnPlayerDamageEvent eventType)
        {
            // TODO: calculate based on resistances
            CurrentPlayerHp -= eventType.Damage;
            eventService.Dispatch<PlayerDamagedEvent>();

            if (CurrentPlayerHp <= 0)
            {
                _deathTimer = 0;
                _isDead = true;
                eventService.Dispatch<PlayerDiedEvent>();
            }
        }

        private void WaitForRevive()
        {
            _deathTimer += Time.deltaTime;

            if (_deathTimer > GameManager.SettingsManager.incomeSettings.DeathTimer)
            {
                _isDead = false;
                CurrentPlayerHp = GameManager.SettingsManager.playerSettings.MaxHp;
                eventService.Dispatch<PlayerRevivedEvent>();
            }
        }
    }
}
