using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Animations;
using UnityEngine;

namespace Minigames.Fight
{
    [Serializable]
    public class EntityStats
    {
        public MovementStats movementStats;
        public CombatStats combatStats;
        
        /// <summary>
        /// This exists to handle things that can only be saved in inspector, such as prefabs.
        /// For now, only certain stats can be overridden from a file
        /// </summary>
        /// <param name="stats"></param>
        public void Load(EntityStats stats)
        {
            movementStats.Load(stats.movementStats);
            combatStats.Load(stats.combatStats);
        }

        public void Init()
        {
            movementStats.Init();
            combatStats.Init();
        }

        public void TickStatuses()
        {
            movementStats.TickStatuses();
            combatStats.TickStatuses();
        }

        public void ClearAllStatusEffects()
        {
            movementStats.ClearAllStatusEffects();
            combatStats.ClearAllStatusEffects();
        }
    }
}