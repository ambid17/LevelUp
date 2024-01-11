using Minigames.Fight;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Minigames.Fight
{
    public class UpgradeSelector : MonoBehaviour
    {
        [SerializeField]
        private UpgradeUI upgradeUI;
        [SerializeField]
        private Button upgradeButtonPrefab;

        private List<Upgrade> allUpgrades;
        private List<GameObject> buttonInstances;

        void Awake()
        {
            allUpgrades = GameManager.UpgradeSettings.AllUpgrades;
            buttonInstances = new List<GameObject>();
        }

        private void OnEnable()
        {
            List<Upgrade> upgrades = new List<Upgrade>();
            if (upgradeUI.upgradeUiState == UpgradeUiState.Upgrade)
            {
                upgrades = allUpgrades.Where(e =>
                    e.UpgradeCategory == upgradeUI.upgradeCategory &&
                    e.EffectCategory == upgradeUI.effectCategory &&
                    e.TierCategory == upgradeUI.tierCategory
                ).ToList();
            }
            else if (upgradeUI.upgradeUiState == UpgradeUiState.Craft)
            {
                upgrades = allUpgrades.Where(e =>
                    e.UpgradeCategory == upgradeUI.upgradeCategory &&
                    e.EffectCategory == upgradeUI.effectCategory &&
                    e.TierCategory == upgradeUI.tierCategory &&
                    e.IsUnlocked == true && 
                    e.IsCrafted == false
                ).ToList();
            }


            foreach (var upgrade in upgrades)
            {
                var upgradeButton = Instantiate(upgradeButtonPrefab, transform);

                var text = upgradeButton.GetComponentInChildren<TMP_Text>();
                text.text = upgrade.Name;

                upgradeButton.onClick.AddListener(() => upgradeUI.OnUpgradeSelected(upgrade));
                buttonInstances.Add(upgradeButton.gameObject);
            }
        }




        private void OnDisable()
        {
            foreach (var button in buttonInstances)
            {
                Destroy(button.gameObject);
            }
        }

    }
}