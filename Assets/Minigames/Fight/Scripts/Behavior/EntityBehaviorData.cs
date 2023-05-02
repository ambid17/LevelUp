using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Minigames.Fight
{
    public enum SpecialEnemyType
    {
        None,
        Bee,
        Ant,
    }

    public class EntityBehaviorData : MonoBehaviour
    {
        #region GeneralData
        [NonSerialized]
        public RoomController roomController;

        [SerializeField]
        private EnemyEntity entity;

        [SerializeField]
        private SpecialEnemyType enemyType;

        public float MoveSpeed => entity.enemyStats.MoveSpeed;
        public GameObject PlayerGo => entity.Target.gameObject;
        public Transform Player => entity.Target;
        public Vector2 PlayerVector => entity.Target.position;
        public Vector2 MyVector => transform.position;
        public List<Transform> FlowerWaypoints => roomController.FlowerWaypoints;
        public float CurrentHealth => entity.Stats.currentHp;
        public float DamageTaken => entity.enemyStats.MaxHp - entity.Stats.currentHp;
        public float DpsTaken => entity.Stats.damageTakenThisSecond;
        public float DistanceToPlayer => Vector2.Distance(transform.position, PlayerVector);
        public SpecialEnemyType EnemyType => enemyType;
        #endregion
        #region BeeData

        public float TotalDamageTaken => roomController.TotalBeeDamageTaken;

        #endregion
        #region AntData
        [SerializeField]
        private float _smellRadius;

        public float SmellRadius
        {
            get
            {
                bool playerHasPheromones = false;
                foreach (StatusEffectInstance effectInstance in GameManager.PlayerEntity.Stats.StatusEffects)
                {
                    if (effectInstance.effect is PheromoneEffect)
                    {
                        playerHasPheromones = true;
                    }
                }
                return playerHasPheromones ? _smellRadius * 1.5f : _smellRadius;
            }
        }
        public Vector2 RandomAroundPlayer => new Vector2(Random.Range(PlayerVector.x - SmellRadius, PlayerVector.x + SmellRadius), Random.Range(PlayerVector.y - SmellRadius, PlayerVector.y + SmellRadius));
        public bool Alerted { get; set; }
        public List<Transform> SoldierWaypoints => roomController.FlowerWaypoints;
        public List<Transform> WorkerWaypoints => roomController.WorkerWaypoints;
        #endregion
        #region SpiderData
        public Vector2 PlayerVelocity => GameManager.PlayerEntity.MovementController.MyRigidbody2D.velocity;
        public bool CanShoot { get => entity.enemyStats.canShootTarget; set => entity.enemyStats.canShootTarget = value; }
        public bool IsTargetSlowed
        {
            get
            {
                foreach (StatusEffectInstance effectInstance in GameManager.PlayerEntity.Stats.StatusEffects)
                {
                    if (effectInstance.effect is SlowEffect)
                    {
                        return true;
                    }
                }
                return false;
            }
        }
        #endregion
    }
}