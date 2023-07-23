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
        public ProjectileController projectilePrefab;
        public int magazineSize;
        public int bulletsInMagazine;
        public float reloadTime;
        public int projectileCount;
        public Sprite bulletSpriteOverride;
    }
}