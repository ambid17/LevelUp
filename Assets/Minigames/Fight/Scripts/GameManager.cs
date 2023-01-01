using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : Singleton<GameManager>
{
    [SerializeField] private GameObject player;
    [SerializeField] private EnemySpawner enemySpawner;
    [SerializeField] private SettingsManager settingsManager;
    [SerializeField] private GameStateManager gameStateManager;
    
    public static GameObject Player => Instance.player;
    public static EnemySpawner EnemySpawner => Instance.enemySpawner;
    public static SettingsManager SettingsManager => Instance.settingsManager;
    public static GameStateManager GameStateManager => Instance.gameStateManager;

    public static event Action dataLoaded;
    private bool isLoaded;
    public bool IsLoaded => isLoaded;

    private float autoSaveTimer;
    private const float autoSaveInterval = 10;

    private void Start()
    {
        Load();
    }

    private void Update()
    {
        autoSaveTimer += Time.deltaTime;

        if (autoSaveTimer > autoSaveInterval)
        {
            autoSaveTimer = 0;
            //Save();
        }
    }

    private void OnApplicationQuit()
    {
        Save();
        
        // In the editor we want to clear scriptable object changes that way they aren't saved and always in the git history, and messing up tests
        // This isn't a problem in the built application as scriptable object changes don't save
        #if UNITY_EDITOR
            SettingsManager.SetDefaults();
        #endif
    }

    private void Load()
    {
        SettingsManager.Init();
        ProgressDataManager.LoadAndApplyData();
        UpgradeDataManager.LoadAndApplyData();
        isLoaded = true;
        dataLoaded?.Invoke();
    }

    private void Save()
    {
        ProgressDataManager.Save();
        UpgradeDataManager.Save();
    }
}
