using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Minigames.Fight
{
    public class ProgressUI : MonoBehaviour
    {
        [SerializeField] private TMP_Text worldNameText;
        [SerializeField] private Image worldImage;
        [SerializeField] private Slider areaProgressSlider;
        [SerializeField] private Button previousCountryButton;
        [SerializeField] private Button nextCountryButton;

        private void Awake()
        {
        }

        void Start()
        {
            previousCountryButton.onClick.AddListener(PreviousCountry);
            previousCountryButton.interactable = false;
        
            nextCountryButton.onClick.AddListener(NextCountry);
            nextCountryButton.interactable = false;
        
            GameManager.GameStateManager.enemyKilled.AddListener(UpdateProgress);
        
            SetWorld();
            UpdateProgress();
        }

        public void SetWorld()
        {
            World world = GameManager.SettingsManager.progressSettings.CurrentWorld;
            worldNameText.text = world.Name;
            worldImage.sprite = world.WorldSprite;
        }

        private void UpdateProgress()
        {
            float percentComplete = GameManager.SettingsManager.progressSettings.CurrentWorld.CurrentCountry.ConquerPercent;
            areaProgressSlider.value = percentComplete;

            bool hasAnotherCountry = GameManager.SettingsManager.progressSettings.CurrentWorld.CurrentCountry.Index <
                                     GameManager.SettingsManager.progressSettings.CurrentWorld.Countries.Count - 1;

            nextCountryButton.interactable = percentComplete >= 1 && hasAnotherCountry;
        
            previousCountryButton.interactable =
                GameManager.SettingsManager.progressSettings.CurrentWorld.CurrentCountry.Index > 0;
            // TODO update world sprite
        }

        private void PreviousCountry()
        {
            GameManager.SettingsManager.progressSettings.CurrentWorld.TrySetPreviousCountry();
            UpdateProgress();
        }

        private void NextCountry()
        {
            GameManager.SettingsManager.progressSettings.CurrentWorld.TrySetNextCountry();
            UpdateProgress();
        }
    }
}
