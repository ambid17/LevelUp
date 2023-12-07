using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks
{
    public enum SuccessType
    {
        OnStart,
        OnComplete,
        None,
    }

    [TaskDescription("Play an animation with custom animation manager.")]
    [TaskCategory("Movement/Animation")]
    [TaskIcon("Assets/Behavior Designer Movement/Editor/Icons/{SkinColor}AnimationIcon.png")]
    public class PlayAnimation : CustomAnimator
    {
        [Tooltip("The animation to be played")]
        public SharedAnimationName Animation;
        [Tooltip("When success should be returned")]
        public SuccessType SuccessType;
        [Tooltip("Whether the animation should fail if it's interrupted")]
        public SharedBool failOnInterrupt;

        private bool _waitOneFrame = true;
        private bool _hasStarted;

        public override void OnStart()
        {
            base.OnStart();

            animationManager.PlayAnimation(Animation.Value, 0);
            _waitOneFrame = true;
            _hasStarted = false;
        }
        public override TaskStatus OnUpdate()
        {
            if (_hasStarted && failOnInterrupt.Value && animationManager.CurrentAnimation != Animation.Value)
            {
                return TaskStatus.Failure;
            }
            if (!animationManager.IsAnimPlaying(Animation.Value))
            {
                animationManager.PlayAnimation(Animation.Value, 0);
                return TaskStatus.Running;
            }
            _hasStarted = true;
            if (SuccessType == SuccessType.None)
            {
                return TaskStatus.Running;
            }
            if (SuccessType == SuccessType.OnStart)
            {
                return TaskStatus.Success;
            }
            if (SuccessType == SuccessType.OnComplete)
            {
                if (animationManager.IsAnimFinished)
                {
                    return TaskStatus.Success;
                }
            }
            return TaskStatus.Running;
        }
    }
}