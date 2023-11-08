using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Minigames.Fight
{
    public class EntityAnimationData : MonoBehaviour
    {
        public AnimationName IdleAnimation => idleAnimation;
        public AnimationName MoveAnimation => moveAnimation;
        public AnimationName VariableMoveAnimation => MyEntity.rb.velocity == Vector2.zero ? idleAnimation : moveAnimation;
        public AnimationName MeleeAttackAnimation => meleeAttackAnimation;
        public AnimationName RangedAttackAnimation => rangedAttackAnimation;
        public AnimationName AlternateMoveAnimation => alternateMoveAnimation;

        [SerializeField] AnimationName idleAnimation;
        [SerializeField] AnimationName moveAnimation;
        [SerializeField] AnimationName meleeAttackAnimation;
        [SerializeField] AnimationName rangedAttackAnimation;
        [SerializeField] AnimationName alternateMoveAnimation;
        [SerializeField] BasicAIMovement MyEntity;
    }

}