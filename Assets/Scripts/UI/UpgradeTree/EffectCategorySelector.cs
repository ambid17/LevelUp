using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Minigames.Fight
{
    public class EffectCategorySelector : MonoBehaviour
    {
        [SerializeField]
        private Button aoe;
        [SerializeField]
        private Button onHit;
        [SerializeField]
        private Button physical;

        [SerializeField]
        private UpgradeUI upgradeUI;

        void Start()
        {
            aoe.onClick.AddListener(() => upgradeUI.OnEffectCategorySelected(EffectCategory.AoE));
            onHit.onClick.AddListener(() => upgradeUI.OnEffectCategorySelected(EffectCategory.OnHit));
            physical.onClick.AddListener(() => upgradeUI.OnEffectCategorySelected(EffectCategory.Physical));
        }
    }
}