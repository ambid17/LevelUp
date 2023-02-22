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
        private List<RewardItem> _rewardItems;
        private int _rewardItemCount = 3; // TODO: look at settings for this
        
        void Awake()
        {
            _eventService = GameManager.EventService;
            _eventService.Add<EnemyKilledEvent>(OnEnemyKilled);
            visualContainer.SetActive(false);
            selectButton.interactable = false;
            selectButton.onClick.AddListener(TryAcceptReward);
            CreateRewardItems();
        }

        private void CreateRewardItems()
        {
            _rewardItems = new();

            for (int i = 0; i < _rewardItemCount; i++)
            {
                RewardItem newItem = Instantiate(rewardItemPrefab, itemParent);
                _rewardItems.Add(newItem);
            }
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
            
            List<Effect> randomEffects = GameManager.SettingsManager.effectSettings.GetRandomEffects(_rewardItemCount);

            for(int i = 0; i < _rewardItems.Count; i++)
            {
                _rewardItems[i].Setup(randomEffects[i], SelectEffect);
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