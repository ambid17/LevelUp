using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyData", menuName = "ScriptableObjects/EnemyData", order = 1)]
[Serializable]
public class EnemyDataContainer : ScriptableObject
{
    public List<EnemyData> enemyContainer;
}


[Serializable]
public class EnemyData
{
    public EnemyType enemyType;
    public GameObject prefab;
}
