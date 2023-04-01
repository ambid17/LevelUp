using System.Collections.Generic;
using UnityEngine;
namespace Minigames.Fight
{

    public class EntityBehaviorData : MonoBehaviour
    {

        [SerializeField]
        private EnemyEntity entity;

        public float MoveSpeed => entity.enemyStats.MoveSpeed;
        public GameObject playerGo => entity.Target.gameObject;
        public Transform player => entity.Target;
        public List<Transform> Waypoints => GameManager.EnemySpawnManager.Waypoints;
    }
}