using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerSettings", menuName = "ScriptableObjects/PlayerSettings", order = 1)]
[Serializable]
public class PlayerSettings : ScriptableObject
{
    public float moveSpeed;
    public float acceleration;
    public float shotSpeed;
    public float shotDamage;
}
