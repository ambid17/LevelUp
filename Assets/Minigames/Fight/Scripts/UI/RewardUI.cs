using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Utils;

namespace Minigames.Fight
{
    public enum RewardType
    {
        Country,
        World
    }

    public class RewardUI : MonoBehaviour
    {
        [SerializeField] private RewardItem rewardItemPrefab;
        [SerializeField] private Transform itemParent;
        [SerializeField] private GameObject visualContainer;
        [SerializeField] private Button selectButton;

        private Effect selectedEffect;
        private EventService _eventService;
        
        void Awake()
        {
            _eventService = GameManager.EventService;
            _eventService.Add<EnemyKilledEvent>(OnEnemyKilled);
            visualContainer.SetActive(false);
            selectButton.interactable = false;
            selectButton.onClick.AddListener(TryAcceptReward);
        }

        private void TryAcceptReward()
        {
            GameManager.SettingsManager.effectSettings.UnlockEffect(selectedEffect);
            visualContainer.SetActive(false);
        }

        private void OnEnemyKilled()
        {
            if (GameManager.SettingsManager.progressSettings.CurrentWorld.IsConquered())
            {
                ShowReward(RewardType.World);
            }
            else if (GameManager.SettingsManager.progressSettings.CurrentWorld.CurrentCountry.IsConquered)
            {
                ShowReward(RewardType.Country);
            }
        }

        void ShowReward(RewardType rewardType)
        {
            visualContainer.SetActive(true);

            int rewardCount = 3;

            for (int i = 0; i < rewardCount; i++)
            {
                Effect randomEffect = GameManager.SettingsManager.effectSettings.GetRandomEffect();
                RewardItem newItem = Instantiate(rewardItemPrefab, itemParent);
                newItem.Setup(randomEffect, SelectEffect);
            }
        }

        void SelectEffect(Effect effect)
        {
            selectedEffect = effect;
            selectButton.interactable = true;
            _eventService.Dispatch(new EffectSelectedEvent(effect));
        }
    }
}