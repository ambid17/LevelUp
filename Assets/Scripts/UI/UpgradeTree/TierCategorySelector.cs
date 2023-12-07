using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Minigames.Fight
{
    public class TierCategorySelector : MonoBehaviour
    {
        [SerializeField]
        private Button tier1Button;
        [SerializeField]
        private Button tier2button;
        [SerializeField]
        private Button tier3button;

        [SerializeField]
        private UpgradeUI upgradeUI;

        void Start()
        {
            tier1Button.onClick.AddListener(() => upgradeUI.OnTierCategorySelected(TierCategory.Tier1));
            tier2button.onClick.AddListener(() => upgradeUI.OnTierCategorySelected(TierCategory.Tier2));
            tier3button.onClick.AddListener(() => upgradeUI.OnTierCategorySelected(TierCategory.Tier3));
        }
    }
}