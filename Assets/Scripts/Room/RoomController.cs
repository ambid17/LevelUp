using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Random = UnityEngine.Random;
using System.Linq;
using UnityEditor;

namespace Minigames.Fight
{
    public class RoomController : MonoBehaviour
    {
        public List<Transform> FlowerWaypoints;
        public List<Transform> WorkerWaypoints;
        public List<Transform> PatrolWaypoints;
        public Tilemap Tilemap;
        public List<RoomConnection> roomConnections;
        public BoxCollider2D MyCollider;

        [SerializeField]
        private HiveMindManager hiveMindManagerPrefab;

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

        [SerializeField]
        private MinimapRoomRender minimapGraphic;
        [SerializeField]
        private MinimapRoomRender minimapGraphicPrefab;

        protected bool hasInitialized;

        protected virtual void InitializeEnemies()
        {
            Dictionary<int, List<HiveMindBehaviorData>> hiveMinds = new();

            foreach (EntityBehaviorData enemy in GameManager.EnemyObjectPool.AllEnemies.Where(e => e.room == this).ToList())
            {
                int randomInt = Random.Range(0, spawnPoints.Count);
                enemy.transform.parent = null;
                enemy.transform.position = spawnPoints[randomInt].position;
                enemy.gameObject.SetActive(true);

                IHiveMind hiveMind = enemy.GetComponent<IHiveMind>();
                if (hiveMind != null)
                {
                    if (!hiveMinds.ContainsKey(hiveMind.Id))
                    {
                        hiveMinds.Add(hiveMind.Id, new());
                    }

                    hiveMinds[hiveMind.Id].Add(hiveMind.myBehaviorData);
                }

                spawnPoints.Remove(spawnPoints[randomInt]);
            }
            foreach (List<HiveMindBehaviorData> hiveMindList in hiveMinds.Values)
            {
                HiveMindManager hiveMindManager = Instantiate(hiveMindManagerPrefab);
                hiveMindManager.Initialize(hiveMindList);
            }
        }

        public void SpawnEnemies()
        {
            foreach (EnemyToSpawn enemy in enemiesToSpawn)
            {
                for (int i = 0; i < enemy.numberToSpawn; i++)
                {
                    EntityBehaviorData behavior = Instantiate(enemy.EnemyPrefab.MyEnemyPrefab, GameManager.EnemyObjectPool.transform);
                    behavior.room = this;
                    GameManager.EnemyObjectPool.AllEnemies.Add(behavior);
                    behavior.gameObject.SetActive(false);
                }
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.layer == PhysicsUtils.PlayerLayer)
            {
                
            }
        }

        private void OnTriggerStay2D(Collider2D collision)
        {
            if (collision.gameObject.layer != PhysicsUtils.PlayerLayer)
            {
                return;
            }
            if (!PhysicsUtils.IsWithinBounds(GameManager.PlayerEntity.transform.position, MyCollider.bounds))
            {
                return;
            }
            // Spawn enemies when player first enters the room instead of on start.
            if (hasInitialized)
            {
                return;
            }
            minimapGraphic.Activate();
            InitializeEnemies();
            GameManager.MinimapCamera.transform.position = new Vector3(MyCollider.bounds.center.x, MyCollider.bounds.center.y, -10);
            GameManager.CameraLerp.Transition(MyCollider.bounds);
            hasInitialized = true;
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

#if UNITY_EDITOR
        [ContextMenu("Setup Room Graphic")]
        public void SetupRoomGraphics()
        {
            minimapGraphic = Instantiate(minimapGraphicPrefab, transform);
            minimapGraphic.transform.position = MyCollider.bounds.center;
            minimapGraphic.transform.localScale = MyCollider.bounds.size;
            EditorUtility.SetDirty(this);
        }

        [ContextMenu("getcomponents")]
        public void GetComponents()
        {
            Tilemap = GetComponentsInChildren<Tilemap>().FirstOrDefault(t => t.gameObject.layer == PhysicsUtils.wallLayer);
            Tilemap.CompressBounds();

            var waypoints = GetComponentsInChildren<Waypoint>();

            spawnPoints = waypoints.Where(w => w.waypointType == WaypointType.Spawn).Select(waypoint => waypoint.transform).ToList();
            PatrolWaypoints = waypoints.Where(w => w.waypointType == WaypointType.Patrol).Select(waypoint => waypoint.transform).ToList();
            FlowerWaypoints = waypoints.Where(w => w.waypointType == WaypointType.Flower).Select(waypoint => waypoint.transform).ToList();
            WorkerWaypoints = waypoints.Where(w => w.waypointType == WaypointType.Worker).Select(waypoint => waypoint.transform).ToList();

            if (PatrolWaypoints.Count == 0)
            {
                PatrolWaypoints = spawnPoints;
            }

            MyCollider = GetComponent<BoxCollider2D>();

            MyCollider.size = Tilemap.size.AsVector2();
            MyCollider.offset = Tilemap.cellBounds.AsBounds().center;
            
            EditorUtility.SetDirty(this);
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

                int horizontalLength = 0;
                int verticalLength = 0;
                int verticalOffset = 0;
                int horizontalOffset = 0;
                
                switch (connection.Label)
                {
                    case "Right":
                        location = new Vector2(max.x, center.y);
                        horizontalLength = 2;
                        verticalLength = 4;
                        horizontalOffset = -1;
                        break;
                    case "Up":
                        location = new Vector2(center.x, max.y);
                        horizontalLength = 4;
                        verticalLength = 2;
                        verticalOffset = -1;
                        break;
                    case "Left":
                        location = new Vector2(min.x, center.y);
                        horizontalLength = 2;
                        verticalLength = 4;
                        horizontalOffset = 1;
                        break;
                    case "Down":
                        location = new Vector2(center.x, min.y);
                        horizontalLength = 4;
                        verticalLength = 2;
                        verticalOffset = 1;
                        break;
                }
                connection.Location = location;

                for (int x = -horizontalLength / 2; x < horizontalLength / 2; x++)
                {
                    for (int y = -verticalLength / 2; y < verticalLength / 2; y++)
                    {
                        var tileCenter = Tilemap.WorldToCell(connection.Location);
                        connection.TilePositions.Add(tileCenter + new Vector3Int(x + horizontalOffset, y + verticalOffset, 0));
                    }
                }

                roomConnections.Add(connection);
            }
            EditorUtility.SetDirty(this);
        }

        private void OnDrawGizmos()
        {

            foreach (var c in roomConnections)
            {
                Gizmos.color = Color.yellow;
                Gizmos.DrawSphere(c.Location, 0.5f);
                Gizmos.color = Color.blue;
                foreach (var tilePosition in c.TilePositions)
                {
                    Gizmos.DrawCube(Tilemap.CellToWorld(tilePosition), Vector3.one);
                }
            }
            
            
        }
#endif

    }

    [Serializable]
    public class EnemyToSpawn
    {
        public EnemyType EnemyPrefab;
        public int numberToSpawn;
    }
}
