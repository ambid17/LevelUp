using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Minigames.Fish
{
    [CreateAssetMenu(fileName = "ProjectileSettings", menuName = "ScriptableObjects/Fish/ProjectileSettings", order = 1)]
    [Serializable]
    public class ProjectileSettings : ScriptableObject
    {
        public List<Projectile> Projectiles;

        public Projectile CurrentProjectile => Projectiles[0];
    }

    [Serializable]
    public class Projectile
    {
        public Sprite Sprite;
        public float MaxDepth;
        public float HorizontalMoveSpeed;
        public float FallSpeed;
        public float Acceleration;
    }
}