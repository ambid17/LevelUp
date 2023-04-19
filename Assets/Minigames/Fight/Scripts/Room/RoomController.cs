using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Minigames.Fight
{
    public class RoomController : MonoBehaviour
    {
        public List<Transform> FlowerWaypoints;
        public List<Transform> WorkerWaypoints;
        public List<Transform> PatrolWaypoints;

        [SerializeField]
        private CinemachineVirtualCamera cam;

        [SerializeField]
        private float zoomSpeed;
        [SerializeField]
        private float zoomSize;

        [SerializeField]
        private List<Transform> spawnPoints;

        [SerializeField]
        private List<EnemyToSpawn> enemiesToSpawn;

        private float startSize;

        private void Start()
        {
            cam.Follow = GameManager.PlayerEntity.transform;
            cam.Priority = 0;
            startSize = cam.m_Lens.OrthographicSize;
            SpawnEnemies();
        }

        private void SpawnEnemies()
        {
            foreach (EnemyToSpawn enemy in enemiesToSpawn)
            {
                for (int i = 0; i < enemy.numberToSpawn; i++)
                {
                    int randomInt = Random.Range(0, spawnPoints.Count);
                    EntityBehaviorData behavior = Instantiate(enemy.EnemyPrefab, spawnPoints[randomInt].position, transform.rotation);
                    behavior.roomController = this;
                    spawnPoints.Remove(spawnPoints[randomInt]);
                }
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.layer == PhysicsUtils.PlayerLayer)
            {
                if (GameManager.RoomManager.CurrentCam != null)
                {
                    GameManager.RoomManager.CurrentCam.Priority = 0;
                }
                GameManager.RoomManager.CurrentCam = cam;
                cam.m_Lens.OrthographicSize = 1;
                cam.Priority = 10;
            }
        }
        private void Update()
        {
            if (cam.m_Lens.OrthographicSize == startSize)
            {
                return;
            }
            if (cam.m_Lens.OrthographicSize < startSize)
            {
                cam.m_Lens.OrthographicSize = Mathf.Lerp(cam.m_Lens.OrthographicSize, startSize, zoomSpeed *Time.deltaTime);
            }
        }
    }
    [Serializable]
    public class EnemyToSpawn
    {
        public EntityBehaviorData EnemyPrefab;
        public int numberToSpawn;
    }
}
