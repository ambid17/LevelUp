using System;
using System.Collections;
using System.Collections.Generic;
using Minigames.Fight;
using UnityEngine;

namespace Minigames.Fight
{
    public class ProjectileWeaponController : WeaponController
    {
        public bool SpawnAoeOnDeath => _spawnAoeOnDeath;

        [SerializeField] protected bool destroyOnReachTarget;
        [SerializeField] protected ProjectileWeapon overridenWeapon;

        [SerializeField] private bool _spawnAoeOnDeath;

        protected Transform MyTransform;


        protected override void Start()
        {
            base.Start();
            overridenWeapon = weapon as ProjectileWeapon;
            MyTransform = transform;
        }
    }
}