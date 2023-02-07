using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Minigames.Fight
{
    [CreateAssetMenu(fileName = "Enemy", menuName = "ScriptableObjects/Fight/Enemy", order = 1)]
    [Serializable]
    public class Enemy : ScriptableObject
    {
        public GameObject Prefab;
        public int SpawnWeight;
        public EnemyInstanceSettings Settings;
    }

    [Serializable]
    public class EnemyInstanceSettings
    {
        public float goldValue;
        public float fireRate;
        public float moveSpeed;
        public float acceleration;
        public float maxHp = 100;
        public float weaponDamage;
        public float projectileSpeed;
        public float projectileLifeTime;

        public float GoldValue =>
            goldValue * GameManager.SettingsManager.progressSettings.CurrentWorld.CurrentCountry.EnemyStatScalar *
            GameManager.SettingsManager.incomeSettings.GoldPerKill;

        public float FireRate => fireRate * GameManager.SettingsManager.enemySpawnerSettings.FireRate;
        public float MoveSpeed => moveSpeed;

        public float MaxHp =>
            maxHp * GameManager.SettingsManager.progressSettings.CurrentWorld.CurrentCountry.EnemyStatScalar *
            GameManager.SettingsManager.enemySpawnerSettings.Hp;

        public float WeaponDamage => weaponDamage *
                                     GameManager.SettingsManager.progressSettings.CurrentWorld.CurrentCountry
                                         .EnemyStatScalar;

        public float ProjectileSpeed => projectileSpeed;

        public float ProjectileLifeTime => projectileLifeTime;
    }
}
