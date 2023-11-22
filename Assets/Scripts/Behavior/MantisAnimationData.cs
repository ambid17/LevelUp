using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Minigames.Fight
{
    public class MantisAnimationData : EntityAnimationData
    {
        public AnimationName TakeOffAnimation => takeOffAnimation;
        public AnimationName LandAnimation => landAnimation;

        [SerializeField]
        private AnimationName takeOffAnimation;
        [SerializeField]
        private AnimationName landAnimation;
    }
}

