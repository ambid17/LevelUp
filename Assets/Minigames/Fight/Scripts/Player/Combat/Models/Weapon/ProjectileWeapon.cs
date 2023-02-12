using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Minigames.Fight
{
    [CreateAssetMenu(fileName = "ProjectileWeapon", menuName = "ScriptableObjects/Fight/Weapons/ProjectileWeapon",
        order = 1)]
    [Serializable]
    public class ProjectileWeapon : Weapon
    {
        public PlayerProjectile ProjectilePrefab;
        public float ProjectileSpread = 0.15f;
    }
}