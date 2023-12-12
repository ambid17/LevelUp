using Minigames.Fight;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Minigames.Fight
{
    public class EnemyProjectile : ProjectileController
    {
        protected override bool IsValidTarget(int layer)
        {
            return layer == PhysicsUtils.EnemyLayer;
        }
    }
}