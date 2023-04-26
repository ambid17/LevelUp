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
        private float weaponDamage;
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
            goldValue * GameManager.SettingsManager.progressSettings.CurrentWorld.CurrentCountry.EnemyStatScalar *
            GameManager.SettingsManager.incomeSettings.GoldPerKill;

        public float FireRate => fireRate * GameManager.SettingsManager.enemySpawnerSettings.FireRate;
        public float MoveSpeed => moveSpeed;
        public float Acceleration => acceleration;

        public float MaxHp =>
            maxHp * GameManager.SettingsManager.progressSettings.CurrentWorld.CurrentCountry.EnemyStatScalar *
            GameManager.SettingsManager.enemySpawnerSettings.Hp;

        public float WeaponDamage => weaponDamage *
                                     GameManager.SettingsManager.progressSettings.CurrentWorld.CurrentCountry
                                         .EnemyStatScalar;

        public float ProjectileSpeed => projectileSpeed;

        public float ProjectileLifeTime { get => projectileLifeTime; set => projectileLifeTime = value; }

        public float Armor => armor;
        public float MagicResist => magicResistance;
        public bool canShootTarget = false;
        public float randomProjectileOffset;
        public bool predictTargetPosition;
    }
}