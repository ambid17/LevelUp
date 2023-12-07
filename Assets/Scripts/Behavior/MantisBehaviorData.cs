using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

namespace Minigames.Fight
{
    public class MantisBehaviorData : EntityBehaviorData
    {
        public Vector2 RandomPointOnMap => (Vector3)PathUtilities.GetRandomReachableNode(transform.position).position;
    }
}