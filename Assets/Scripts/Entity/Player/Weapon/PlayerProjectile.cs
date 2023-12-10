using System;
using UnityEngine;
using Utils;

namespace Minigames.Fight
{
    public class PlayerProjectile : ProjectileController
    {
        protected override bool IsValidTarget(int layer)
        {
            return layer == PhysicsUtils.EnemyLayer;
        }
    }
}
