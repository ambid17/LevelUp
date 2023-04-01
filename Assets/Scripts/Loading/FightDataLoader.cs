using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Minigames.Fight;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FightDataLoader : MonoBehaviour
{
    public EffectSettings effectSettings;
    public static int TargetSceneIndex;


    private void Awake()
    {
        Scene current = SceneManager.GetActiveScene();
        if (current.buildIndex == AnySceneLaunch.ANY_SCENE_LAUNCH_INDEX)
        {
            Load();
            SceneManager.LoadScene(TargetSceneIndex);
        }
    }

    public void Load()
    {
        LoadProgressData();
        LoadEffectData();
        LoadWeaponData();
    }

    private void LoadEffectData()
    {
        var effectContainer = EffectDataManager.Load();
        if (effectContainer != null && effectContainer.effects != null)
        {
            foreach (var effect in effectContainer.effects)
            {
                effectSettings.LoadSavedEffect(effect);
            }
        }
    }
    
    public void LoadProgressData()
    {
        ProgressModel data = ProgressDataManager.Load();

        if (data != null)
        {
            LoadSerializedProgress(data);
        }
        
        Platform.ProgressSettings.UnlockWorlds();
    }
    
    public void LoadSerializedProgress(ProgressModel progressModel)
    {
        Platform.ProgressSettings.Currency = progressModel.Currency;
        Platform.ProgressSettings.TutorialState = progressModel.TutorialState;
        
        for (int worldIndex = 0; worldIndex < progressModel.WorldData.Count; worldIndex++)
        {
            Platform.ProgressSettings.Worlds[worldIndex].CurrencyPerMinute = progressModel.WorldData[worldIndex].CurrencyPerMinute;
            Platform.ProgressSettings.Worlds[worldIndex].LastTimeVisited = progressModel.WorldData[worldIndex].LastTimeVisited;
            for (int countryIndex = 0; countryIndex < progressModel.WorldData[worldIndex].CountryData.Count; countryIndex++)
            {
                Platform.ProgressSettings.Worlds[worldIndex].Countries[countryIndex].EnemyKillCount =
                    progressModel.WorldData[worldIndex].CountryData[countryIndex].Kills;
            }
        }
    }
    
    private void LoadWeaponData()
    {
        var weaponModel = WeaponDataManager.Load();
        if (weaponModel != null)
        {
            Platform.WeaponSettings.FromModel(weaponModel);
        }
    }
}
