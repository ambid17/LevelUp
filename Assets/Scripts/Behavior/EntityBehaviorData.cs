using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Minigames.Fight
{
    public class EntityBehaviorData : MonoBehaviour
    {
        public LayerMask ObstacleLayerMask;

        [SerializeField]
        private EnemyEntity myEntity;
        [SerializeField]
        private float tick;
        public RoomController room;
        [SerializeField]
        private float tickRandomizer;
        [SerializeField]
        private bool trackDamagePerTick;
        [SerializeField]
        private float damageDecayRate;

        public float DamageLastTick
        {
            get => myEntity.Stats.DamageTakenThisSecond;
            set => myEntity.Stats.DamageTakenThisSecond = value;
        }

        public virtual bool CanSeeTarget
        {
            get
            {
                GameObject player = PhysicsUtils.HasLineOfSight(transform, GameManager.PlayerEntity.transform, MyEntity.enemyStats.DetectRange, 360, ObstacleLayerMask);
                return player != null;
            }
        }

        public Vector2 RandomSoldierWaypoint
        {
            get
            {
                int i = Random.Range(0, PatrolWaypoints.Count);
                return PatrolWaypoints[i].position;
            }
        }

        public Vector2 RandomWorkerWaypoint
        {
            get
            {
                int i = Random.Range(0, WorkerWaypoints.Count);
                return WorkerWaypoints[i].position;
            }
        }

        public EnemyEntity MyEntity => myEntity;
        public Transform PlayerTransform => myEntity.Target;
        public Vector2 PlayerPosition => myEntity.Target.position;
        public float Tick => tick + Time.deltaTime;
        public float Speed => myEntity.enemyStats.MoveSpeed;
        public float DistanceToPlayer => Vector2.Distance(transform.position, PlayerPosition);
        public float PursueDistance => (MyEntity.enemyStats.MeleeAttackRange * 0.5f);
        public bool CanShoot => MyEntity.enemyStats.canShootTarget;
        public bool CanMelee => MyEntity.enemyStats.canMeleeTarget;
        public virtual bool WithinVisionRadius => DistanceToPlayer <= MyEntity.enemyStats.DetectRange;



        public List<Transform> FlowerWaypoints => room.FlowerWaypoints;
        public List<Transform> WorkerWaypoints => room.WorkerWaypoints;
        public List<Transform> PatrolWaypoints => room.PatrolWaypoints;


        private float _tickTimer;

        private void Start()
        {
            tick += Random.Range(-tickRandomizer, tickRandomizer);
        }

        private void Update()
        {
            if (_tickTimer < tick)
            {
                _tickTimer += Time.deltaTime;
                return;
            }
            OnTick();
            _tickTimer = 0;
        }

        public virtual void OnTick()
        {
            if (trackDamagePerTick && DamageLastTick > 0)
            {
                DamageLastTick -= damageDecayRate;
            }
        }
    }
}