using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Minigames.Fight
{
    public class HiveMindBehaviorData : EntityBehaviorData, IHiveMind
    {
        [SerializeField]
        private int myId;
        public int Id => myId;
        public HiveMindBehaviorData myBehaviorData => this;

        
    }
}

