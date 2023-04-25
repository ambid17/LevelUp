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
            if (otherBehavior == null)
            {
                return;
            }
            if (otherBehavior.EnemyType != SpecialEnemyType.Ant)
            {
                return;
            }

            // If one is alerted and the other isn't set both to alerted.
            if (otherBehavior.Alerted != behavior.Alerted)
            {
                behavior.Alerted = true;
                otherBehavior.Alerted = true;
            }
        }
    }

}