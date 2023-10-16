using System.Collections;
using System.Collections.Generic;
using Minigames.Fight;
using UnityEngine;

namespace Minigames.Fight
{
    public class PlayerVisualController : VisualController
    {
        protected override void Start()
        {
            base.Start();
            EventService.Add<PlayerDiedEvent>(Die);
            EventService.Add<PlayerRevivedEvent>(Revive);
        }

        private void Revive()
        {
            SpriteRenderer.color = Color.white;
        }

        private void Die()
        {
            // since the player takes damage as they are dying
            // we need to stop the flash effect from overriding the death fx
            IsFlashing = false;
            SpriteRenderer.material = defaultMaterial;
        }
        protected override IEnumerator DamageAnimation()
        {
            if (GameManager.PlayerEntity.IsDead)
            {
                yield break;
            }
            GameManager.PlayerEntity.Stunned = true;
            takeHitAnimation = GameManager.PlayerEntity.AnimationController.PlayTakeHitAnimation();

            while (GameManager.PlayerEntity.AnimationController.CurrentAnimation != takeHitAnimation)
            {
                takeHitAnimation = GameManager.PlayerEntity.AnimationController.PlayTakeHitAnimation();
                yield return null;
            }

            while (!GameManager.PlayerEntity.AnimationController.IsAnimFinished)
            {
                yield return null;
            }
            GameManager.PlayerEntity.Stunned = false;
        }
    }
}