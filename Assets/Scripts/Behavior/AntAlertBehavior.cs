using UnityEngine;

namespace Minigames.Fight
{
    public class AntAlertBehavior : MonoBehaviour
    {
        [SerializeField]
        private EntityBehaviorData behavior;

        private CircleCollider2D col;

        private void Start()
        {
            col = GetComponent<CircleCollider2D>();
            col.radius = behavior.SmellRadius;
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.layer != PhysicsUtils.EnemyLayer)
            {
                return;
            }
            EntityBehaviorData otherBehavior = collision.GetComponent<EntityBehaviorData>();
            if (otherBehavior == null || otherBehavior.EnemyType != SpecialEnemyType.Ant)
            {
                return;
            }

            // If other ant is alerted we become alerted
            if (otherBehavior.Alerted)
            {
                behavior.Alerted = true;
            }
        }
    }

}