using System;
using Minigames.Fight;
using UnityEngine;

namespace Minigames.Fight
{
    [CreateAssetMenu(fileName = "BaseDamageEffect", menuName = "ScriptableObjects/Fight/Effects/BaseDamageEffect", order = 1)]
    [Serializable]
    public class BaseDamageEffect : StatModifierEffect
    {
        public override StatImpactType statImpactType => StatImpactType.Flat;

        public override ModifiableStat GetStatToAffect(Entity entity)
        {
            return entity.Stats.combatStats.baseDamage;
        }
    }
}