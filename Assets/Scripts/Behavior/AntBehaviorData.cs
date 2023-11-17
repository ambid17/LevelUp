using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Minigames.Fight
{
    public class AntBehaviorData : EntityBehaviorData
    {
        [SerializeField]
        private float smellFactor;

        public bool IsAlerted { get; set; }

        public bool PlayerHasPheromones
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
                return playerHasPheromones;
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
        public float SmellRadius => PlayerHasPheromones ? MyEntity.enemyStats.DetectRange * smellFactor : MyEntity.enemyStats.DetectRange;

        public Vector2 RandomAroundPlayer => PhysicsUtils.RandomAroundTarget(PlayerPosition, MyEntity.enemyStats.DetectRange);

        public override void OnTick()
        {
            if (!IsAlerted)
            {
                if (WithinVisionRadius)
                {
                    IsAlerted = true;
                }
                return;
            }
            foreach (Collider2D col in Physics2D.OverlapCircleAll(transform.position, SmellRadius))
            {
                AntBehaviorData ant = col.GetComponent<AntBehaviorData>();
                if (ant != null)
                {
                    ant.IsAlerted = true;
                }
                
            }
        }
    }
}

