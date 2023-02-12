using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Minigames.Fight
{
    [CreateAssetMenu(fileName = "EffectSettings", menuName = "ScriptableObjects/Fight/EffectSettings", order = 1)]
    [Serializable]
    public class EffectSettings : ScriptableObject
    {
        [Header("Set in Editor")] public List<Effect> AllEffects;

        public OrderedList<Effect> UnlockedEffects = new();

        public void SetDefaults()
        {
            UnlockedEffects = null;
        }

        public void UnlockAllEffects()
        {
            foreach (var effect in AllEffects)
            {
                UnlockedEffects.Add(effect);
            }
        }

        public void UnlockEffect(Effect effect)
        {
            if (!UnlockedEffects.Contains(effect))
            {
                UnlockedEffects.Add(effect);
            }
            else
            {
                Effect toUpgrade = UnlockedEffects.FirstOrDefault(eff => eff.Name == effect.Name);

                if (toUpgrade != null)
                {
                    toUpgrade.AmountOwned++;
                }
                else
                {
                    Debug.LogError($"No effect found with name: {effect.Name}");
                }
            }
        }


        [NonSerialized] private int _weightTotal;

        public Effect GetRandomEffect()
        {
            if (_weightTotal == 0)
            {
                _weightTotal = AllEffects.Sum(e => e.SpawnWeight);
            }

            int randomWeight = UnityEngine.Random.Range(0, _weightTotal);
            foreach (var effect in AllEffects)
            {
                randomWeight -= effect.SpawnWeight;
                if (randomWeight < 0)
                {
                    return effect;
                }
            }

            return AllEffects[0];
        }
    }
}