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

        public override void OnStart()
        {
            base.OnStart();

            animationManager.PlayAnimation(Animation.Value, 0);
        }
        public override TaskStatus OnUpdate()
        {
            if (animationManager.CurrentAnimation != Animation.Value)
            {
                animationManager.PlayAnimation(Animation.Value, 0);
                return TaskStatus.Running;
            }
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