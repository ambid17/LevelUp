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


    private float autoSaveTimer;
    private const float autoSaveInterval = 10; 
    
    public override void Initialize()
    {
        Load();
    }

    private void Update()
    {
        autoSaveTimer += Time.deltaTime;

        if (autoSaveTimer > autoSaveInterval)
        {
            Save();
        }
    }

    private void Load()
    {
        ProgressDataManager.Load();
        UpgradeDataManager.Load();
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
