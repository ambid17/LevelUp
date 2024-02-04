using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Unity.VisualScripting.Member;

namespace Minigames.Fight
{
    // AOE Slow
    // AOE fire damage over time
    //  - take damage in AOE, apply status on leaving

    public class AoeEffect : Effect
    {
        public AoeInstanceController aoePrefab;
        public Effect baseEffect;
        public Effect statusEffect;
        public float duration;
        public float tickRate;
        public float chanceToPlace;

        public override void ApplyOverrides(EffectOverrides overrides)
        {
            duration = overrides.duration;
            tickRate = overrides.tickRate;
            chanceToPlace = overrides.applicationChance;
        }

        public override void OnCraft(Entity target)
        {
            if (_upgradeCategory == UpgradeCategory.Range)
            {
                target.Stats.combatStats.projectileWeaponStats.AoeEffects.Add(this);
            }
            else if( _upgradeCategory == UpgradeCategory.Melee)
            {
                target.Stats.combatStats.meleeWeaponStats.AoeEffects.Add(this);
            }
        }

        public void Place(Entity source, Vector3 position)
        {
            bool doesApply = UnityEngine.Random.value < chanceToPlace;
            if (doesApply)
            {
                var instance = Instantiate(aoePrefab);
                instance.transform.position = position;
                instance.Setup(source, this);
            }
        }
    }
}