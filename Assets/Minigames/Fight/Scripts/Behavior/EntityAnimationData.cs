using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Minigames.Fight
{
    public class EntityAnimationData : MonoBehaviour
    {
        public AnimationName IdleAnimation => idleAnimation;
        public AnimationName MoveAnimation => moveAnimation;
        public AnimationName AttackAnimation => attackAnimation;

        [SerializeField] AnimationName idleAnimation;
        [SerializeField] AnimationName moveAnimation;
        [SerializeField] AnimationName attackAnimation;
    }

}