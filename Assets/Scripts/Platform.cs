using System;
using System.Collections;
using System.Collections.Generic;
using Minigames.Fight;
using UnityEngine;

public class Platform : Singleton<Platform>
{
    [SerializeField] private ProgressSettings progressSettings;
    public static ProgressSettings ProgressSettings => Instance.progressSettings;
    
    public static bool ShouldSave = true;

    protected override void Initialize()
    {
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }
    
    private void OnApplicationQuit()
    {
        if (ShouldSave)
        {
            Save();
        }

        // Only clear progress on quit. If done on destroy it will delete loaded progress when loading into a game
#if UNITY_EDITOR
        ProgressSettings.SetDefaults();
#endif
    }

    private void Save()
    {
        ProgressDataManager.Save(ProgressSettings);
    }
}
