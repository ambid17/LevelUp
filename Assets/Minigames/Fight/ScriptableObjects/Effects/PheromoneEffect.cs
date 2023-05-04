using System;
using UnityEngine;
namespace Minigames.Fight
{
    [CreateAssetMenu(fileName = "SlowEffect", menuName = "ScriptableObjects/Fight/PheromoneEffect", order = 1)]
    [Serializable]
    public class PheromoneEffect : Effect, IStatusEffect
    {
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

        public override string GetNextUpgradeDescription(int purchaseCount)
        {
            return string.Empty;
        }

        public override EffectTriggerType TriggerType => EffectTriggerType.OnHit;
        public override string UpgradePath => string.Empty;

        public override void Execute(HitData hit)
        {
            TryApplyEffect(hit);
        }

        public void TryApplyEffect(HitData hit)
        {
            StatusEffectInstance.Create(hit, this);
        }

        public void ApplyEffect(Entity target)
        {
            if (target.gameObject.layer == PhysicsUtils.PlayerLayer)
            {
                storedVisual = Instantiate(visualEffectPrefab, GameManager.PlayerEntity.transform.position, GameManager.PlayerEntity.transform.rotation, GameManager.PlayerEntity.transform);             
            }
            if (target.gameObject.layer == PhysicsUtils.EnemyLayer)
            {
                EntityBehaviorData behaviorData = target.GetComponent<EntityBehaviorData>();
                if (behaviorData.EnemyType == SpecialEnemyType.Ant)
                {
                    behaviorData.Alerted = true;
                }
            }
        }

        public void RemoveEffect(Entity target)
        {
            if (target.gameObject.layer == PhysicsUtils.PlayerLayer)
            {
                Destroy(storedVisual);
            }
        }

        public void OnTick(Entity target)
        {

        }
    }
}