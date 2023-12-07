using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Minigames.Fight
{
    public class UpgradeCategorySelector : MonoBehaviour
    {
        [SerializeField]
        private Button meleeButton;
        [SerializeField]
        private Button rangeButton;
        [SerializeField]
        private Button playerButton;

        [SerializeField]
        private UpgradeUI upgradeUI;

        void Start()
        {
            meleeButton.onClick.AddListener(() => upgradeUI.OnUpgradeCategorySelected(UpgradeCategory.Melee));
            rangeButton.onClick.AddListener(() => upgradeUI.OnUpgradeCategorySelected(UpgradeCategory.Range));
            playerButton.onClick.AddListener(() => upgradeUI.OnUpgradeCategorySelected(UpgradeCategory.Player));
        }
    }
}