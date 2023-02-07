using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Minigames.Fight
{
    [CreateAssetMenu(fileName = "MeleeWeapon", menuName = "ScriptableObjects/Fight/Weapons/MeleeWeapon", order = 1)]
    [Serializable]
    public class MeleeWeapon : Weapon
    {
        public float animationLength;
        public float stunDuration;
        public float knockbackDistance;
    }
}