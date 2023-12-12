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
                foreach (var effect in GameManager.PlayerEntity.Stats.combatStats.hpStatusEffects)
                {
                    if (effect.statusEffect is PheromoneEffect)
                    {
                        playerHasPheromones = true;
                    }
                }
                return playerHasPheromones;
            }
        }

        public float SmellRadius => PlayerHasPheromones ? MyEntity.enemyStats.DetectRange * smellFactor : MyEntity.enemyStats.DetectRange;

        public Vector2 RandomAroundPlayer => PhysicsUtils.RandomAroundTarget(PlayerPosition, MyEntity.enemyStats.DetectRange);

        public override void OnTick()
        {
            if (IsAlerted)
            {
                return;
            }

            if (WithinVisionRadius || MyEntity.Stats.combatStats.currentHp < MyEntity.Stats.combatStats.maxHp.Calculated)
            {
                IsAlerted = true;
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

