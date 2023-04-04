using System.Collections.Generic;
using UnityEngine;
namespace Minigames.Fight
{

    public class EntityBehaviorData : MonoBehaviour
    {

        [SerializeField]
        private EnemyEntity entity;

        public float MoveSpeed => entity.enemyStats.MoveSpeed;
        public GameObject PlayerGo => entity.Target.gameObject;
        public Transform Player => entity.Target;
        public Vector2 PlayerVector => entity.Target.position;
        public List<Transform> Waypoints => GameManager.EnemySpawnManager.Waypoints;
        public float CurrentHealth => entity.Stats.currentHp;
        public float DamageTaken => entity.enemyStats.MaxHp - entity.Stats.currentHp;
        public float DistanceToPlayer => Vector2.Distance(transform.position, PlayerVector);
    }
}