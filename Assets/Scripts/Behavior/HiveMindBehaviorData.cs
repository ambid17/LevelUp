using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Minigames.Fight
{
    public class HiveMindBehaviorData : EntityBehaviorData, IHiveMind
    {
        public int Id { get; set; }
        public HiveMindBehaviorData myBehaviorData => this;

        public HiveMindManager MyManager { get; set; }
        public override bool CanSeeTarget => MyManager.CanSeeTarget;
        public bool IsAlerted => MyManager.IsAlerted;
        public bool IsAggro => IsAlerted && CanSeeTarget;
        public float AttackPriority => MyManager.AttackPriority;
    }
}

