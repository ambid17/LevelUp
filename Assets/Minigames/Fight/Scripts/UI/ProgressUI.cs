using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Utils;

namespace Minigames.Fight
{
    public class ProgressUI : MonoBehaviour
    {
        [SerializeField] private TMP_Text worldNameText;
        [SerializeField] private Image worldImage;
        [SerializeField] private Slider areaProgressSlider;
        [SerializeField] private Button previousCountryButton;
        [SerializeField] private Button nextCountryButton;
        [SerializeField] private TMP_Text progressText;

        private EventService _eventService;
        
        private void Awake()
        {
        }

        void Start()
        {
            previousCountryButton.onClick.AddListener(PreviousCountry);
            previousCountryButton.interactable = false;
        
            nextCountryButton.onClick.AddListener(NextCountry);
            nextCountryButton.interactable = false;
            
            _eventService = GameManager.EventService;
            _eventService.Add<EnemyKilledEvent>(UpdateProgress);
            _eventService.Add<PlayerDiedEvent>(UpdateProgress);
        
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

            float killCount = GameManager.SettingsManager.progressSettings.CurrentWorld.CurrentCountry.EnemyKillCount;
            float maxKills = GameManager.SettingsManager.progressSettings.CurrentWorld.CurrentCountry
                .EnemyKillsToComplete;
            progressText.text = $"{killCount.ToKillString()} / {maxKills.ToKillString()}";
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
