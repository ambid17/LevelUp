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
        [SerializeField]
        private RoomController room;
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

        public EnemyEntity MyEntity => myEntity;
        public Transform PlayerTransform => myEntity.Target;
        public Vector2 PlayerPosition => myEntity.Target.position;
        public float Tick => tick + Time.deltaTime;
        public float Speed => myEntity.enemyStats.MoveSpeed;
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
            if (trackDamagePerTick)
            {
                DamageLastTick -= damageDecayRate;
            }
        }
    }
}