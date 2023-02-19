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

        private EnemyMovementController _movementController;
        
        private const float MaxDistanceFromPlayer = 100;
        private bool isMarkedForDeath;

        protected override void Start()
        {
            base.Start();
            _defaultColor =
                GameManager.SettingsManager.progressSettings.CurrentWorld.CurrentCountry.EnemyTierColor;
            _damageText.enabled = false;
            
            eventService.Add<PlayerDiedEvent>(Cull);
        }

        protected override void Update()
        {
            base.Update();
            FlipSprite();
            TryCull();
        }

        protected void  FlipSprite()
        {
            //Flip the sprite based on velocity
            if(_movementController.myRigidbody.velocity.x < 0) 
                spriteRenderer.flipX = true;
            else 
                spriteRenderer.flipX = false;
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
            Vector2 offsetFromPlayer = _movementController.target.position - transform.position;
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
