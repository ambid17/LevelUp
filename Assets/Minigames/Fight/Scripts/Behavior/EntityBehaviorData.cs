using UnityEngine;
namespace Minigames.Fight
{

    public class EntityBehaviorData : MonoBehaviour
    {
        [SerializeField]
        private EnemyEntity entity;

        public float MoveSpeed => entity.enemyStats.MoveSpeed;
        public Transform player => entity.Target;
    }
}