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
        public override void StartDamageFx(float damage)
        {
            base.StartDamageFx(damage);
            DamageTextController damageText = GameManager.DamageTextPool.Get();
            damageText.Setup(damage.ToString(), transform.position);
        }

        public void FaceTarget(Vector2 target)
        {
            float direction = target.x - transform.position.x;
            MyEntity.VisualController.SpriteRenderer.flipX = direction < 0 ? false : true;
        }
    }
}
