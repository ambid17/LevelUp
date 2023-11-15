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
        private float moveSpeed;
        [SerializeField]
        private float maxHp = 100;
        [SerializeField]
        private float projectileSpeed;
        [SerializeField]
        private float projectileLifeTime;
        public List<Effect> effects;

        public float GoldValue =>
            goldValue *
            GameManager.SettingsManager.incomeSettings.GoldPerKill;

        public float MoveSpeed => moveSpeed;
        public float MaxHp => maxHp;

        public float ProjectileSpeed => projectileSpeed;

        public float ProjectileLifeTime { get => projectileLifeTime; set => projectileLifeTime = value; }

        public bool canShootTarget = false;
        public bool canMeleeTarget = false;
        public float randomProjectileOffset;
        public bool predictTargetPosition;
        public bool isPassive;
        public float DetectRange;
        public float MeleeAttackRange;
    }
}