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

        public List<Effect> OnHitEffects = new();

        public void SetDefaults()
        {
            foreach (var effect in AllEffects)
            {
                effect.AmountOwned = 0;
                effect.IsUnlocked = false;
            }

            OnHitEffects = null;
        }

        public void UnlockAllEffects()
        {
            foreach (var effect in AllEffects)
            {
                effect.Unlock(this);
            }
        }

        public void LoadSavedEffect(EffectModel effectModel)
        {
            Type effectType = effectModel.Type;
            var effectToLoad = AllEffects.FirstOrDefault(e => e.GetType() == effectType);

            if (effectToLoad != null)
            {
                if (effectModel.IsUnlocked)
                {
                    effectToLoad.Unlock(this);
                }
                effectToLoad.AmountOwned = effectModel.AmountOwned;
            }
            else
            {
                Debug.LogError($"No effect found of type: {effectType}");
            }
        }


        [NonSerialized] private int _weightTotal;

        public Effect GetRandomEffect()
        {
            if (_weightTotal == 0)
            {
                _weightTotal = AllEffects.Sum(e => e.DropWeight);
            }

            int randomWeight = UnityEngine.Random.Range(0, _weightTotal);
            foreach (var effect in AllEffects)
            {
                randomWeight -= effect.DropWeight;
                if (randomWeight < 0)
                {
                    return effect;
                }
            }

            return AllEffects[0];
        }

        public List<Effect> GetRandomEffects(int count)
        {
            List<Effect> toReturn = new();

            while (toReturn.Count < count)
            {
                Effect random = GetRandomEffect();
                if (!toReturn.Contains(random))
                {
                    toReturn.Add(random);
                }
            }

            return toReturn;
        }
    }
}