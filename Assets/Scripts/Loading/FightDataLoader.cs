using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Minigames.Fight;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FightDataLoader : MonoBehaviour
{
    public ProgressSettings progressSettings;
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
        
        progressSettings.UnlockWorlds();
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
