using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Minigames.Fight
{
    public class ParentEffect : Effect
    {
        public Effect positive;
        public Effect negative;
        public float chanceToApply;
        public float chanceToBackfire;

        public override void ApplyOverrides(EffectOverrides overrides)
        {
            base.ApplyOverrides(overrides);
            chanceToApply = overrides.applicationChance;
            chanceToBackfire = overrides.chanceToBackfire;
        }
        public override void Execute(Entity source, Entity target)
        {
            bool doesApply = Random.value > chanceToApply;
            if (doesApply)
            {
                bool doesBackfire = Random.value > chanceToBackfire;
                if (doesBackfire)
                {
                    negative.Execute(source, target);
                    return;
                }
                positive.Execute(source, target);
            }
        }
    }
}
