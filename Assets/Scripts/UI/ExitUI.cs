using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Minigames.Fight
{
    public class ExitUI : UIPanel
    {
        [SerializeField]
        private Button closeButton;
        [SerializeField]
        private Button floorButton;
        [SerializeField]
        private Button biomeButton;
        [SerializeField]
        private GameObject popup;
        [SerializeField]
        private TMP_Text text;
        [SerializeField]
        private TMP_Text details;
        [SerializeField]
        private Image textBackground;
        [SerializeField]
        private LoadingManager loadingManager;

        void Start()
        {
            closeButton.onClick.AddListener(Close);
            floorButton.onClick.AddListener(LoadNextFloor);
            biomeButton.onClick.AddListener(LoadNextBiome);
            Platform.EventService.Add<PlayerInteractedEvent>(OnPlayerInteract);
            Platform.EventService.Add<MouseHoverEvent>(OnHover);
        }

        public override void Toggle(bool shouldBeActive)
        {
            base.Toggle(shouldBeActive);
            var biome = GameManager.ProgressSettings.CurrentBiome;
            biomeButton.gameObject.SetActive(biome.IsCompleted);
            text.text = $"Biome: {biome.Name} \n Level: {biome.FloorsCompleted} \n {(biome.IsCompleted ? "New biome available!" : $"Complete {biome.FloorsToComplete - biome.FloorsCompleted} more levels to upgrade your drone") }";
            textBackground.rectTransform.sizeDelta = new(textBackground.rectTransform.sizeDelta.x, text.preferredHeight + 1);
        }

        private void OnPlayerInteract(PlayerInteractedEvent e)
        {
            if (e.InteractionType == InteractionType.Exit)
            {
                GameManager.UIManager.ToggleUiPanel(UIPanelType.Exit, true);
            }
        }

        private void OnHover(MouseHoverEvent e)
        {
            details.text = e.Message;
        }

        private void Close()
        {
            GameManager.UIManager.ToggleUiPanel(UIPanelType.Exit, false);
        }

        private void LoadNextFloor()
        {
            //TODO: play takeoff animation.
            Instantiate(loadingManager);
        }

        private void LoadNextBiome()
        {
            //TODO: play takeoff animation.
            popup.SetActive(true);
        }
    }
}