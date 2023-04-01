using System;
using System.Collections;
using System.Collections.Generic;
using Minigames.Fight;
using UnityEngine;

public class Platform : Singleton<Platform>
{
    [SerializeField] private ProgressSettings progressSettings;
    [SerializeField] private WeaponSettings weaponSettings;
    public static ProgressSettings ProgressSettings => Instance.progressSettings;
    public static WeaponSettings WeaponSettings => Instance.weaponSettings;
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
}
