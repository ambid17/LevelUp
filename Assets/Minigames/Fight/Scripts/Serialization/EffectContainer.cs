using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Minigames.Fight
{
    [Serializable]
    public class EffectContainer
    {
        public List<EffectModel> effects;

        public EffectContainer()
        {
        }

        public EffectContainer(List<Effect> effects)
        {
            foreach (var effect in effects)
            {
                TrackEffect(effect);
            }
        }

        private void TrackEffect(Effect effect)
        {
            if (effects == null)
            {
                effects = new List<EffectModel>();
            }

            EffectModel newEffect = new EffectModel()
            {
                Type = effect.GetType(),
                Name = effect.Name,
                AmountOwned = effect.AmountOwned
            };

            effects.Add(newEffect);
        }
    }
}
