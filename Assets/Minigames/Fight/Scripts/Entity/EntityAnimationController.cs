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
        private AnimationName attackAnimation;
        [SerializeField]
        private AnimationName dieAnimation;
        [SerializeField]
        private AnimationName onHitAnimation;

        public void PlayIdleAnim()
        {
            PlayAnimation(idleAnimation);
        }

        public void PlayMoveAnim()
        {
            PlayAnimation(moveAnimation);
        }

        public void PlayAttackAnim()
        {
            PlayAnimation(attackAnimation);
        }

        public void PlayDieAnim()
        {
            PlayAnimation(dieAnimation);
        }
        public void PlayTakeHitAnim()
        {
            PlayAnimation(onHitAnimation);
        }
    }

}