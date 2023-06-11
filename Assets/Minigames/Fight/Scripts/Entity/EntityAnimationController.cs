using UnityEngine;

namespace Minigames.Fight
{
    [RequireComponent(typeof(Animator))]
    public class EntityAnimationController : AnimationManager
    {

        [SerializeField]
        private AnimationName idleAnimation;
        [SerializeField]
        private AnimationName moveAnimation;
        [SerializeField]
        private AnimationName meleeAttackAnimation;
        [SerializeField]
        private AnimationName projectileAttackAnimation;
        [SerializeField]
        private AnimationName dieAnimation;
        [SerializeField]
        private AnimationName onHitAnimation;

        public void PlayIdleAnim()
        {
            PlayAnimation(idleAnimation, 0);
        }

        public void PlayMoveAnim()
        {
            PlayAnimation(moveAnimation, 0);
        }

        public void PlayMeleeAttackAnimation()
        {
            PlayAnimation(meleeAttackAnimation, 0);
        }

        public void PlayProjectileAttackAnimation()
        {
            PlayAnimation(projectileAttackAnimation, 0);
        }

        public void PlayDieAnim()
        {
            PlayAnimation(dieAnimation, 0);
        }
        public void PlayTakeHitAnim()
        {
            PlayAnimation(onHitAnimation, 0);
        }
    }

}