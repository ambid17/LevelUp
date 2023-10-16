using System.Collections;
using System.Collections.Generic;
using TMPro;
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

//    public class RewardUI : UIPanel
//    {
//        [SerializeField] private RewardItem rewardItemPrefab;
//        [SerializeField] private Transform itemParent;
//        [SerializeField] private Button selectButton;
//        [SerializeField] private TMP_Text titleText;

//        private Effect selectedEffect;
//        private EventService _eventService;
//        private List<RewardItem> _rewardItems;
//        private int _rewardItemCount = 3; // TODO: look at settings for this
        
//        void Awake()
//        {
//            _eventService = GameManager.EventService;
//            //_eventService.Add<CountryCompletedEvent>(OnCountryCompleted);
//            //_eventService.Add<WorldCompletedEvent>(OnWorldCompleted);
//            selectButton.interactable = false;
//            //selectButton.onClick.AddListener(TryAcceptReward);
//            CreateRewardItems();
//        }

//        private void CreateRewardItems()
//        {
//            _rewardItems = new();

//            for (int i = 0; i < _rewardItemCount; i++)
//            {
//                RewardItem newItem = Instantiate(rewardItemPrefab, itemParent);
//                _rewardItems.Add(newItem);
//            }
//        }

//        //private void TryAcceptReward()
//        //{
//        //    selectedEffect.Unlock(GameManager.SettingsManager.effectSettings);

//        //    // Update the player entity's managed list of on-hit effects
//        //    if (selectedEffect.TriggerType == EffectTriggerType.OnHit)
//        //    {
//        //        GameManager.EventService.Dispatch<OnHitEffectUnlockedEvent>();
//        //    }
            
//        //    GameManager.UIManager.ToggleUiPanel(UIPanelType.Reward, false);
//        //}

//        //private void OnCountryCompleted()
//        //{
//        //    SetupReward(RewardType.Country);
//        //    GameManager.UIManager.ToggleUiPanel(UIPanelType.Reward, true);
//        //}

//        //private void OnWorldCompleted()
//        //{
//        //    SetupReward(RewardType.World);
//        //    GameManager.UIManager.ToggleUiPanel(UIPanelType.Reward, true);
//        //}

//        //void SetupReward(RewardType rewardType)
//        //{
//        //    if (rewardType == RewardType.Country)
//        //    {
//        //        titleText.text = $"You completed country {GameManager.SettingsManager.progressSettings.CurrentWorld.CurrentCountry.Index + 1} of {GameManager.SettingsManager.progressSettings.CurrentWorld.Countries.Count} in {GameManager.SettingsManager.progressSettings.CurrentWorld.Name}";
//        //    }else if (rewardType == RewardType.World)
//        //    {
//        //        titleText.text = $"You completed {GameManager.SettingsManager.progressSettings.CurrentWorld.Name}!";
//        //    }
            
//        //    List<Effect> randomEffects = GameManager.SettingsManager.effectSettings.GetRandomEffects(_rewardItemCount);

//        //    for(int i = 0; i < _rewardItems.Count; i++)
//        //    {
//        //        _rewardItems[i].Setup(randomEffects[i], SelectEffect);
//        //    }
//        //}

//        void SelectEffect(Effect effect)
//        {
//            selectedEffect = effect;
//            selectButton.interactable = true;
//            _eventService.Dispatch(new EffectSelectedEvent(effect));
//        }
//    }
}