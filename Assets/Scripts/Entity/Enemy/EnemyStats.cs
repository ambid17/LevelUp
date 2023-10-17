using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Minigames.Fight
{
    [Serializable]
    public class EnemyStats
    {
        [SerializeField]
        private float goldValue;
        [SerializeField]
        private float fireRate;
        [SerializeField]
        private float moveSpeed;
        [SerializeField]
        private float acceleration;
        [SerializeField]
        private float maxHp = 100;
        [SerializeField]
        private float meleeWeaponDamage;
        [SerializeField]
        private float projectileWeaponDamage;
        [SerializeField]
        private float projectileSpeed;
        [SerializeField]
        private float projectileLifeTime;
        [SerializeField]
        private float armor;
        [SerializeField]
        private float magicResistance;
        public List<Effect> effects;

        public float GoldValue =>
            goldValue *
            GameManager.SettingsManager.incomeSettings.GoldPerKill;

        public float FireRate => fireRate;
        public float MoveSpeed => moveSpeed;
        public float Acceleration => acceleration;

        public float MaxHp =>
            maxHp;

        public float MeleeWeaponDamage => meleeWeaponDamage;
        public float ProjectileWeaponDamage => projectileWeaponDamage;

        public float ProjectileSpeed => projectileSpeed;

        public float ProjectileLifeTime { get => projectileLifeTime; set => projectileLifeTime = value; }

        public float Armor => armor;
        public float MagicResist => magicResistance;
        public bool canShootTarget = false;
        public bool canMeleeTarget = false;
        public float randomProjectileOffset;
        public bool predictTargetPosition;
        public bool isPassive;
    }
}