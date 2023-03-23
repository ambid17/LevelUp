using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : Singleton<Platform>
{
    protected override void Initialize()
    {
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
