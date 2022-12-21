using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : Singleton<GameManager>
{
    [SerializeField] private GameObject player;
    [SerializeField] private EnemySpawner enemySpawner;
    [SerializeField] private UpgradeManager upgradeManager;
    [SerializeField] private GameStateManager gameStateManager;
    
    public static GameObject Player => Instance.player;
    public static EnemySpawner EnemySpawner => Instance.enemySpawner;
    public static UpgradeManager UpgradeManager => Instance.upgradeManager;
    public static GameStateManager GameStateManager => Instance.gameStateManager;

    public static event Action dataLoaded;

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
            Save();
        }
    }

    private void OnApplicationQuit()
    {
        Save();
        
        // In the editor we want to clear scriptable object changes that way we are using the save files properly when testing
        #if UNITY_EDITOR
            UpgradeManager.SetDefaults();
        #endif
    }

    private void Load()
    {
        UpgradeManager.SetDefaults();
        ProgressDataManager.Load();
        UpgradeDataManager.Load();
        //dataLoaded();
    }

    private void Save()
    {
        ProgressDataManager.Save();
        UpgradeDataManager.Save();
    }

    public Progress GetProgress()
    {
        Progress toReturn = new Progress();
        
        toReturn.Currency = GameStateManager.Currency;

        return toReturn;
    }

    public void ApplyProgress(Progress progress)
    {
        GameStateManager.LoadCurrency(progress.Currency);
    }
}
