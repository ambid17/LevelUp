using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Minigames.Fight
{
    public class EnemyEffectController : MonoBehaviour
    {
        protected Entity MyEntity;
        public List<EffectUpgradeContainer> effectContainers;

        void Start()
        {
            foreach(var effectContainer in effectContainers)
            {
                effectContainer.Init();
                effectContainer.effect.GiveUpgradeInfo(1,UpgradeCategory.None, EffectCategory.None);
                effectContainer.effect.OnCraft(MyEntity);
                effectContainer.effect.ToggleEquip(MyEntity, true);
            }
        }
    }
}