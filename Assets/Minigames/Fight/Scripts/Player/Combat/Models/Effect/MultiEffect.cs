using System.Collections.Generic;
using System.Linq;
using Minigames.Fight;

namespace Minigames.Fight
{
    public class MultiEffect : Effect, IExecuteEffect
    {
        public List<Effect> effects;

        public override EffectTriggerType TriggerType => EffectTriggerType.OnHit;

        public void Execute(DamageWorksheet worksheet)
        {
            foreach (var effect in effects.OfType<IExecuteEffect>())
            {
                effect.Execute(worksheet);
            }
        }
    }
}