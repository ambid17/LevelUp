using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Minigames.Fight
{
    public class PlayerVisualController : VisualController
    {
        protected override void DamageAnimation()
        {
            PlayerAnimationController animationController = MyEntity.AnimationController as PlayerAnimationController;
            MyEntity.Stunned = true;
            StartCoroutine(animationController.Stun(animationController.PlayerTakeHitAnimation(), AfterStun));
        }
    }
}