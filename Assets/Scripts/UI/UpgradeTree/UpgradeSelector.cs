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

        private List<Effect> effects;
        private List<GameObject> buttonInstances;

        void Awake()
        {
            effects = GameManager.SettingsManager.effectSettings.AllEffects;
            buttonInstances = new List<GameObject>();
        }

        private void OnEnable()
        {
            var filteredEffects = effects.Where(e =>
                e.UpgradeCategory == upgradeUI.upgradeCategory &&
                e.EffectCategory == upgradeUI.effectCategory &&
                e.TierCategory == upgradeUI.tierCategory
            ).ToList();

            foreach( var effect in filteredEffects )
            {
                var upgradeButton = Instantiate(upgradeButtonPrefab, transform);

                var text = upgradeButton.GetComponentInChildren<TMP_Text>();
                text.text = effect.Name;

                upgradeButton.onClick.AddListener(() => upgradeUI.OnUpgradeSelected(effect));
                buttonInstances.Add(upgradeButton.gameObject);
            }
        }


        private void OnDisable()
        {
            foreach(var button in buttonInstances)
            {
                Destroy(button.gameObject);
            }
        }

    }
}