using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Minigames.Fight
{
    public class PlayerMeleeAoE : PlayerProjectile
    {
        public override void Setup(Entity myEntity, Vector2 direction)
        {
            base.Setup(myEntity, direction);
            transform.rotation = PhysicsUtils.LookAt(transform, GameManager.PlayerEntity.PlayerCamera.ScreenToWorldPoint(Input.mousePosition), 180);
        }
    }
}