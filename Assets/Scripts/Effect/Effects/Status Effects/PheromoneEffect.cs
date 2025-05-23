using System;
using UnityEngine;
namespace Minigames.Fight
{
    [CreateAssetMenu(fileName = "SlowEffect", menuName = "ScriptableObjects/Effects/PheromoneEffect", order = 1)]
    [Serializable]
    public class PheromoneEffect : Effect, IStatusEffect
    {
        [Header("Effect specific")]
        public float duration = 2f;
        public float Duration => duration;
        public float TickRate => 0;

        [SerializeField]
        private GameObject visualEffectPrefab;

        private GameObject storedVisual;

        private readonly string _description = "Marked with pheromones for {0} seconds";


        public override string GetDescription()
        {
            return string.Format(_description, duration);
        }

        public override void OnCraft(Entity target)
        {
            // This should never be used by players
            // This should be manually added to the Ant's effect list for its weapon
            throw new NotImplementedException();
        }

        public override void Execute(Entity source, Entity target)
        {
            if (target.gameObject.layer == PhysicsUtils.PlayerLayer)
            {
                storedVisual = Instantiate(visualEffectPrefab, GameManager.PlayerEntity.transform.position, GameManager.PlayerEntity.transform.rotation, GameManager.PlayerEntity.transform);
            }

            target.Stats.combatStats.AddOrRefreshStatusEffect(this, source, target);
        }

        public override float ImpactStat(float stat)
        {
            return stat;
        }

        public void OnTick(Entity source, Entity target)
        {
        }

        public void OnComplete()
        {
            Destroy(storedVisual);
        }
    }
}