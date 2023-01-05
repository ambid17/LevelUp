using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Minigames.Mining
{
    [CreateAssetMenu(fileName = "PlayerSettings", menuName = "ScriptableObjects/Mining/PlayerSettings", order = 1)]
    [Serializable]
    public class PlayerSettings : ScriptableObject
    {
        [Header("Set in Editor")]
        public Vector3 spawnPoint;
        public float acceleration, jetpackAcceleration;
        public float dragCoefficient;
        public float maxMoveSpeed;
        public float maxFallSpeed;
        public float maxJetpackSpeed;
        public float digRange;
        public float jetpackFuelUse;
        public float walkFuelUse;
        public ContactFilter2D digContactFilter;
        public float deathAnimationLength;

        [Header("Runtime Values")]
        public bool isDead;

    }

}
