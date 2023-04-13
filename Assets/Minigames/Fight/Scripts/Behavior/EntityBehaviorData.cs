using System.Collections.Generic;
using UnityEngine;
namespace Minigames.Fight
{

    public class EntityBehaviorData : MonoBehaviour
    {
        #region GeneralData
        [SerializeField]
        private EnemyEntity entity;

        public float MoveSpeed => entity.enemyStats.MoveSpeed;
        public GameObject PlayerGo => entity.Target.gameObject;
        public Transform Player => entity.Target;
        public Vector2 PlayerVector => entity.Target.position;
        public Vector2 MyVector => transform.position;
        public List<Transform> Waypoints => GameManager.EnemySpawnManager.Waypoints;
        public float CurrentHealth => entity.Stats.currentHp;
        public float DamageTaken => entity.enemyStats.MaxHp - entity.Stats.currentHp;
        public float DistanceToPlayer => Vector2.Distance(transform.position, PlayerVector);
        #endregion
        #region AntData
        [SerializeField]
        private float _smellRadius;
        [SerializeField]
        private bool _alerted;

        public float SmellRadius => _smellRadius;
        public Vector2 RandomAroundPlayer => new Vector2(Random.Range(PlayerVector.x - SmellRadius, PlayerVector.x + SmellRadius), Random.Range(PlayerVector.y - SmellRadius, PlayerVector.y + SmellRadius));
        public bool Alerted { get => _alerted; set => _alerted = value; }
        public List<Transform> SoldierWaypoints => GameManager.EnemySpawnManager.SoldierWaypoints;
        #endregion
        #region SpiderData
        public Vector2 PlayerVelocity => GameManager.PlayerEntity.MovementController.MyRigidbody2D.velocity;

        #endregion
    }
}