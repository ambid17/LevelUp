using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Minigames.Fight
{
    public class MantisAnimationData : EntityAnimationData
    {
        public AnimationName TakeOffAnimation => takeOffAnimation;
        public AnimationName LandAnimation => landAnimation;
        public AnimationName ComboStart => comboStart;
        public AnimationName ComboEnd => comboEnd;
        public AnimationName Exhaust => exhaust;

        public override AnimationName RangedAttackAnimation
        {
            get
            {
                int i = Random.Range(0, comboAnimations.Count);
                return comboAnimations[i];
            }
        }

        public override AnimationName MeleeAttackAnimation
        {
            get
            {
                int i = Random.Range(0, attackAnimations.Count);
                return attackAnimations[i];
            }
        }

        [SerializeField]
        private AnimationName takeOffAnimation;
        [SerializeField]
        private AnimationName landAnimation;
        [SerializeField]
        private AnimationName comboStart;
        [SerializeField]
        private AnimationName comboEnd;
        [SerializeField]
        private AnimationName exhaust;

        [SerializeField]
        private List<AnimationName> attackAnimations;
        [SerializeField]
        private List<AnimationName> comboAnimations;
    }
}

