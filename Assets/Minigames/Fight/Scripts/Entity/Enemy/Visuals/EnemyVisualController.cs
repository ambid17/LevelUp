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
        [SerializeField] private TextMeshPro _damageText; // TODO: move to object pool

        private EnemyEntity _enemyEntity;
        
        private const float MaxDistanceFromPlayer = 100;
        private bool isMarkedForDeath;

        protected override void Start()
        {
            base.Start();
            _enemyEntity = MyEntity as EnemyEntity;
            _defaultColor =
                GameManager.SettingsManager.progressSettings.CurrentWorld.CurrentCountry.EnemyTierColor;
            _damageText.enabled = false;
            
            EventService.Add<PlayerDiedEvent>(Cull);
        }

        protected override void Update()
        {
            base.Update();
            TryCull();
        }
        
        public override void StartDamageFx(float damage)
        {
            base.StartDamageFx(damage);
            StartCoroutine(ShowDamage(damage));
        }

        private IEnumerator ShowDamage(float damage)
        {
            _damageText.enabled = true;
            _damageText.text = damage.ToString();
            Sequence sequence = _damageText.transform.DOJump(transform.position, 0.5f, 1, 1);
            yield return sequence.WaitForCompletion();
            _damageText.enabled = false;
        }

        // If the player runs too far from the enemy, kill it off
        private void TryCull()
        {
            Vector2 offsetFromPlayer = _enemyEntity.target.position - transform.position;
            if (offsetFromPlayer.magnitude > MaxDistanceFromPlayer)
            {
                Cull();
            }
        }

        private void Cull()
        {
            if (isMarkedForDeath)
            {
                return;
            }
        
            isMarkedForDeath = true;
        
            GameManager.EnemySpawnManager.EnemyCount--;

            Destroy(gameObject);
        }
    }
}
