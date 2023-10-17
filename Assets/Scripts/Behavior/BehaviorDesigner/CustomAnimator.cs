using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks
{
    public abstract class CustomAnimator : Action
    {
        protected AnimationManager animationManager;

        public override void OnAwake()
        {
            animationManager = GetComponent<AnimationManager>();
        }
    }
}