using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ProgressSettings", menuName = "ScriptableObjects/ProgressSettings", order = 1)]
[Serializable]
public class ProgressSettings : ScriptableObject
{
    public List<World> Worlds;
}

public class World
{
    public string Name;
    public Sprite WorldSprite;
    public List<Country> Countries;
}

public class Country
{
    public int EnemyKillCount;
    public int MinSpawnWeight;
    public int MaxSpawnWeight;
}
