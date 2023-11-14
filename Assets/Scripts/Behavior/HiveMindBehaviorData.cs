using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Minigames.Fight
{
    public class HiveMindBehaviorData : EntityBehaviorData, IHiveMind
    {
        [SerializeField]
        private float retreatPriorityFactory = 2;
        public int Id { get; set; }
        public HiveMindBehaviorData myBehaviorData => this;

        public HiveMindManager MyManager { get; set; }
        public override bool CanSeeTarget => MyManager.CanSeeTarget;
        public bool IsAlerted => MyManager.IsAlerted;
        public bool IsAggro => IsAlerted && CanSeeTarget;
        public float AttackPriority => MyManager.AttackPriority;
        public float RetreatPriority => DamageLastTick * retreatPriorityFactory;
        public Vector2 PlayerLastKnown => MyManager.PlayerLastKnown;
        public Vector2 RandomFlower
        {
            get
            {
                int i = Random.Range(0, FlowerWaypoints.Count);
                return FlowerWaypoints[i].position;
            }
        }
    }
}

