using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using Utils;

namespace Minigames.Fight
{
    public class EnemyVisualController : VisualController
    {
        private EnemyEntity _enemyEntity;
        
        private const float MaxDistanceFromPlayer = 100;
        private bool _isMarkedForDeath;
        private Vector2 _lastRecordedSpeed = new(-1, 0);

        protected override void Start()
        {
            base.Start();
            _enemyEntity = MyEntity as EnemyEntity;
            defaultColor =
                GameManager.SettingsManager.progressSettings.CurrentWorld.CurrentCountry.EnemyTierColor;
            SpriteRenderer.color = defaultColor;
            flashColor = Color.white;
            
            //EventService.Add<PlayerDiedEvent>(Cull);
        }

        private void OnDestroy()
        {
            //EventService.Remove<PlayerDiedEvent>(Cull);
        }

        protected override void Update()
        {
            base.Update();
            //TryCull();
        }
        
        public override void StartDamageFx(float damage)
        {
            base.StartDamageFx(damage);
            DamageTextController damageText = GameManager.DamageTextPool.Get();
            damageText.Setup(damage.ToString(), transform.position);
        }

        // If the player runs too far from the enemy, kill it off
        private void TryCull()
        {
            Vector2 offsetFromPlayer = _enemyEntity.Target.position - transform.position;
            if (offsetFromPlayer.magnitude > MaxDistanceFromPlayer)
            {
                Cull();
            }
        }

        private void Cull()
        {
            if (_isMarkedForDeath)
            {
                return;
            }
        
            _isMarkedForDeath = true;

            Destroy(gameObject);
        }
    }
}
