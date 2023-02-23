using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Minigames.Fight;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class FightDataLoader : MonoBehaviour
{
    public ProgressSettings progressSettings;
    public UpgradeSettings upgradeSettings;
    public EffectSettings effectSettings;
    public static int targetSceneIndex;


    private void Awake()
    {
        Scene current = SceneManager.GetActiveScene();
        if (current.buildIndex == AnySceneLaunch.ANY_SCENE_LAUNCH_INDEX)
        {
            Load();
            SceneManager.LoadScene(targetSceneIndex);
        }
    }

    public void Load()
    {
        LoadUpgradeData();
        LoadProgressData();
        LoadEffectData();
    }

    private void LoadEffectData()
    {
        var effectContainer = EffectDataManager.Load();
        if (effectContainer != null)
        {
            foreach (var effect in effectContainer.effects)
            {
                effectSettings.LoadSavedEffect(effect);
            }
        }
    }
    
    public void LoadUpgradeData()
    {
        UpgradeData data = UpgradeDataManager.Load();
        if (data != null)
        {
            LoadSerializedUpgrades(data);
        }
    }
    
    public void LoadSerializedUpgrades(UpgradeData container)
    {
        if (container == null || container.upgrades == null)
        {
            return;
        }
        
        foreach (var upgrade in container.upgrades)
        {
            switch (upgrade)
            {
                case PlayerUpgradeModel model:
                    UpdatePlayerUpgrades(model);
                    break;
                case WeaponUpgradeModel model:
                    UpdateWeaponUpgrades(model);
                    break;
                case EnemyUpgradeModel model:
                    UpdateEnemyUpgrades(model);
                    break;
                case IncomeUpgradeModel model:
                    UpdateIncomeUpgrades(model);
                    break;
            }
        }
    }

    private void UpdatePlayerUpgrades(PlayerUpgradeModel model)
    {
        PlayerUpgrade upgrade = upgradeSettings.PlayerUpgrades.First(u => u.upgradeType == model.playerUpgradeType);
        upgrade.numberPurchased = model.numberPurchased;
    }
    
    private void UpdateWeaponUpgrades(WeaponUpgradeModel model)
    {
        WeaponUpgrade upgrade = upgradeSettings.WeaponUpgrades.First(u => u.upgradeType == model.weaponUpgradeType);
        upgrade.numberPurchased = model.numberPurchased;
    }
    
    private void UpdateEnemyUpgrades(EnemyUpgradeModel model)
    {
        EnemyUpgrade upgrade = upgradeSettings.EnemyUpgrades.First(u => u.upgradeType == model.enemyUpgradeType);
        upgrade.numberPurchased = model.numberPurchased;
    }
    
    private void UpdateIncomeUpgrades(IncomeUpgradeModel model)
    {
        IncomeUpgrade upgrade = upgradeSettings.IncomeUpgrades.First(u => u.upgradeType == model.incomeUpgradeType);
        upgrade.numberPurchased = model.numberPurchased;
    }
    
    public void LoadProgressData()
    {
        ProgressModel data = ProgressDataManager.Load();

        if (data != null)
        {
            LoadSerializedProgress(data);
        }
    }
    
    public void LoadSerializedProgress(ProgressModel progressModel)
    {
        progressSettings.Currency = progressModel.Currency;
        
        for (int worldIndex = 0; worldIndex < progressModel.WorldData.Count; worldIndex++)
        {
            progressSettings.Worlds[worldIndex].CurrencyPerMinute = progressModel.WorldData[worldIndex].CurrencyPerMinute;
            progressSettings.Worlds[worldIndex].LastTimeVisited = progressModel.WorldData[worldIndex].LastTimeVisited;
            for (int countryIndex = 0; countryIndex < progressModel.WorldData[worldIndex].CountryData.Count; countryIndex++)
            {
                progressSettings.Worlds[worldIndex].Countries[countryIndex].EnemyKillCount =
                    progressModel.WorldData[worldIndex].CountryData[countryIndex].Kills;
            }
        }
    }
    
    
}
