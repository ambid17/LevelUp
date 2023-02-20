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
        [SerializeField] private TextMeshPro damageText; // TODO: move to object pool

        private EnemyEntity _enemyEntity;
        
        private const float MaxDistanceFromPlayer = 100;
        private bool _isMarkedForDeath;

        protected override void Start()
        {
            base.Start();
            _enemyEntity = MyEntity as EnemyEntity;
            defaultColor =
                GameManager.SettingsManager.progressSettings.CurrentWorld.CurrentCountry.EnemyTierColor;
            damageText.enabled = false;
            
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
            damageText.enabled = true;
            damageText.text = damage.ToString();
            Sequence sequence = damageText.transform.DOJump(transform.position, 0.5f, 1, 1);
            yield return sequence.WaitForCompletion();
            damageText.enabled = false;
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
        
            GameManager.EnemySpawnManager.EnemyCount--;

            Destroy(gameObject);
        }
    }
}
