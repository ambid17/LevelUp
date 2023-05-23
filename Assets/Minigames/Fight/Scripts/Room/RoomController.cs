using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Random = UnityEngine.Random;
using System.Linq;

namespace Minigames.Fight
{
    public class RoomController : MonoBehaviour
    {
        public List<Transform> FlowerWaypoints;
        public List<Transform> WorkerWaypoints;
        public List<Transform> PatrolWaypoints;
        public PolygonCollider2D col;
        public Tilemap Tilemap;
        public List<RoomConnection> roomConnections;

        [SerializeField]
        private int tilesPerConnection = 4;

        public float TotalBeeDamageTaken
        {
            get
            {
                float totalCurrentHealth = 0;
                foreach (EntityBehaviorData behavior in beesInRoom)
                {
                    if (behavior != null)
                    {
                        totalCurrentHealth += behavior.CurrentHealth;
                    }
                }
                return beeHealthSum - totalCurrentHealth;
            }
        }

        [SerializeField]
        private CinemachineVirtualCamera cam;

        [SerializeField]
        private float zoomSpeed;
        [SerializeField]
        private float zoomSize;
        [SerializeField]
        private float startSize = 5;

        [SerializeField]
        private List<Transform> spawnPoints;

        [SerializeField]
        private List<EnemyToSpawn> enemiesToSpawn;

        private float beeHealthSum;
        private float maxBeeHealth;

        private bool hasInitialized;

        private List<EntityBehaviorData> beesInRoom = new List<EntityBehaviorData>();

        private void Start()
        {
            cam.Follow = GameManager.CameraLerp.transform;
            cam.Priority = 0;
            cam.m_Lens.OrthographicSize = startSize;
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
                    if (behavior.EnemyType == SpecialEnemyType.Bee)
                    {
                        maxBeeHealth = behavior.CurrentHealth;
                        beeHealthSum += maxBeeHealth;
                        beesInRoom.Add(behavior);
                    }
                    spawnPoints.Remove(spawnPoints[randomInt]);
                }
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.layer == PhysicsUtils.PlayerLayer)
            {
                // Spawn enemies when player first enters the room instead of on start.
                if (!hasInitialized)
                {
                    SpawnEnemies();
                    hasInitialized = true;
                }
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

        // If a connection is not being used close it off.
        public void CloseExits(Tile tileToPlace)
        {
            foreach (RoomConnection connection in roomConnections)
            {
                if (!connection.HasConnection)
                {
                    foreach (Vector3Int tilePos in connection.TilePositions)
                    {
                        Tilemap.SetTile(tilePos, tileToPlace);
                    }
                }
            }
        }

        [ContextMenu("getcomponents")]
        public void GetComponents()
        {
            col = GetComponent<PolygonCollider2D>();
            Tilemap = GetComponentsInChildren<Tilemap>().FirstOrDefault(t => t.gameObject.layer == PhysicsUtils.wallLayer);
            Tilemap.CompressBounds();
            UnityEditor.EditorUtility.SetDirty(this);
        }
        [ContextMenu("Generate Connections")]
        public void GenerateConnections()
        {
            // Clear list before regenerating.
            roomConnections.Clear();
            for (int i = 0; i < 4; i++)
            {
                RoomConnection connection = new();

                // Label for easy reference and set directions.
                switch (i)
                {
                    case 0:
                        connection.Direction = Vector2.right;
                        connection.Label = "Right";
                        break;
                    case 1:
                        connection.Direction = Vector2.up;
                        connection.Label = "Up";
                        break;
                    case 2:
                        connection.Direction = Vector2.left;
                        connection.Label = "Left";
                        break;
                    case 3:
                        connection.Direction = Vector2.down;
                        connection.Label = "Down";
                        break;
                }

                // Set location of connection to the correct edge of the tilemap.
                Vector2 location = Vector2.zero;
                Vector2 max = Tilemap.origin.AsVector2() + Tilemap.cellBounds.size.AsVector2();
                Vector2 min = Tilemap.origin.AsVector2();
                Vector2 center = Tilemap.cellBounds.center.AsVector2();
                switch (connection.Label)
                {
                    case "Right":
                        location = new Vector2(max.x, center.y);
                        break;
                    case "Up":
                        location = new Vector2(center.x, max.y);
                        break;
                    case "Left":
                        location = new Vector2(min.x, center.y);
                        break;
                    case "Down":
                        location = new Vector2(center.x, min.y);
                        break;
                }
                connection.Location = location;

                // Generate a list of empty tiles by checking every coordinate for a tile and adding it to the list if it returns null
                // Yes this is the most effecient way to do this other than setting them manually, Unity be dumb.
                TileBase[] allTiles = Tilemap.GetTilesBlock(Tilemap.cellBounds);
                List<Vector3Int> emptyTiles = new();
                
                for (int x = Tilemap.origin.x; x < Tilemap.origin.x + Tilemap.cellBounds.size.x; x++)
                {
                    for (int y = Tilemap.origin.y; y < Tilemap.origin.y + Tilemap.cellBounds.size.y; y++)
                    {
                        Vector3Int tilePos = new Vector3Int(x, y, 0);
                        TileBase tile = Tilemap.GetTile(tilePos);
                        if (tile == null)
                        {
                            emptyTiles.Add(tilePos);
                        }
                    }
                }

                for (int tilesToAdd = 0; tilesToAdd < tilesPerConnection; tilesToAdd++)
                {
                    float distance = Mathf.Infinity;
                    Vector3Int nearest = Vector3Int.zero;

                    foreach (Vector3Int tilePosition in emptyTiles.ToList())
                    {
                        float newDistance = Vector2.Distance(tilePosition.AsVector2(), connection.Location);
                        if (newDistance < distance)
                        {
                            distance = newDistance;
                            nearest = tilePosition;
                        }
                    }
                    emptyTiles.Remove(nearest);
                    connection.TilePositions.Add(nearest);
                }

                roomConnections.Add(connection);
            }
            UnityEditor.EditorUtility.SetDirty(this);
        }
    }
    [Serializable]
    public class RoomConnection
    {
        public string Label;
        public Vector2 Location;
        public Vector2 Direction;
        public List<Vector3Int> TilePositions = new();
        public bool HasConnection;
    }

    [Serializable]
    public class EnemyToSpawn
    {
        public EntityBehaviorData EnemyPrefab;
        public int numberToSpawn;
    }
}
