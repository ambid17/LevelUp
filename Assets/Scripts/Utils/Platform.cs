using System;
using System.Collections;
using System.Collections.Generic;
using Minigames.Fight;
using UnityEngine;
using Utils;


public class Platform : Singleton<Platform>
{
    [SerializeField] private ProgressSettings progressSettings;
    [SerializeField] private UpgradeSettings effectSettings;
    public static ProgressSettings ProgressSettings => Instance.progressSettings;
    
    public static bool ShouldSave = true;

    private EventService _eventService;
    public static EventService EventService
    {
        get
        {
            if (Instance._eventService == null)
            {
                Instance._eventService = new EventService();
            }

            return Instance._eventService;
        }
    }

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
        effectSettings.SetDefaults();
#endif
    }

    private void Save()
    {
        ProgressDataManager.Save(ProgressSettings);
        EffectDataManager.Save();
    }
}
