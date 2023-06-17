using Cinemachine;
using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Minigames.Fight
{
    public class RoomManager : Singleton<RoomManager>
    {
        public CinemachineVirtualCamera CurrentCam { get; set; }

        [SerializeField]
        private ProgressSettings progressSettings;
        [SerializeField]
        private int minCaches = 5;
        [SerializeField]
        private int maxCaches = 10;
        [SerializeField]
        private AstarPath pathFinderPrefab;
        [SerializeField]
        private ResourceCache resourceCachePrefab;
        [SerializeField]
        ResourceTypeSpriteDictionary cacheSpriteDictionary;

        private RoomSettings _roomSettings;

        private const string groundGraph = "GroundGraph";

        protected override void Initialize()
        {
            _roomSettings = GameManager.SettingsManager.progressSettings.CurrentWorld.RoomSettings;

            List<RoomController> availableRooms = new();

            // Instantiate start room.
            var startRoom = Instantiate(_roomSettings.startRoom);

            // Add start room to our list of rooms to branch from.
            availableRooms.Add(startRoom);
            var targetRoom = startRoom;

            // Deterimine how many rooms we'll spawn.
            int roomCount = Random.Range(_roomSettings.minRooms, _roomSettings.maxRooms + 1);


            List<RoomController> roomsToInstantiate = new();
            Vector2 direction = Vector2.zero;

            // Populate our rooms to instantiate list with the determined number of random rooms
            for (int i = 0; i < roomCount; i++)
            {
                roomsToInstantiate.Add(_roomSettings.GetRandomRoom());
            }

            // Generate the map
            foreach (RoomController roomToInstantiate in roomsToInstantiate)
            {
                bool valid = false;
                var room = roomToInstantiate;

                Vector2 center = Vector2.zero;

                // Repeat until valid placement has been found
                while (!valid)
                {
                    // Select a random room to branch off of.
                    int roomToBranchOffOf = Random.Range(0, availableRooms.Count);
                    targetRoom = availableRooms[roomToBranchOffOf];

                    // Select random direction to branch in.
                    direction = GetRandomCardinalDirection();

                    // Calculate the location the new room will spawn in.
                    float x = (targetRoom.myPolygonCollider.bounds.extents.x + (room.Tilemap.cellBounds.AsVector2().x / 2)) * direction.x;
                    float y = (targetRoom.myPolygonCollider.bounds.extents.y + (room.Tilemap.cellBounds.AsVector2().y / 2)) * direction.y;
                    center = targetRoom.myPolygonCollider.bounds.center.AsVector2() + new Vector2(x, y);


                    // Abort before doing the overlap check if the chosen direction has already been used (slight performance boost).
                    RoomConnection roomConnection = targetRoom.roomConnections.First(r => r.Direction == direction);
                    if (roomConnection.HasConnection)
                    {
                        continue;
                    }

                    // Check perposed room location to ensure it won't overlap with an existing room
                    if (!Physics2D.OverlapBox(center, room.Tilemap.cellBounds.AsVector2() * 0.99f, 0))
                    {
                        valid = true;

                        // Once room has spawned mark the old rooms connection as having been used.
                        roomConnection.HasConnection = true;
                    }
                }

                // Instantiate room in direction.
                var roomInstance = Instantiate(room, center - room.Tilemap.cellBounds.center.AsVector2(), Quaternion.identity);

                // Mark the new rooms connection with the old room.
                RoomConnection connectionInstance = roomInstance.roomConnections.First(r => r.Direction == -direction);
                connectionInstance.HasConnection = true;

                // Add to available rooms so it too can be branched off of.
                availableRooms.Add(roomInstance);
            }

            // Close off unused exits.
            foreach (RoomController room in availableRooms)
            {
                room.CloseExits(_roomSettings.wallTile);
            }

            // Determine total min and max of the full level
            Vector2 max = Vector2.negativeInfinity;
            Vector2 min = Vector2.positiveInfinity;

            foreach (RoomController room in availableRooms)
            {
                Vector2 roomMin = room.Tilemap.CellToWorld(room.Tilemap.origin);
                Vector2 roomMax = room.Tilemap.CellToWorld(room.Tilemap.origin + room.Tilemap.cellBounds.size);

                if (roomMax.x > max.x)
                {
                    max.x = roomMax.x;
                }
                if (roomMax.y > max.y)
                {
                    max.y = roomMax.y;
                }
                if (roomMin.x < min.x)
                {
                    min.x = roomMin.x;
                }
                if (roomMin.y < min.y)
                {
                    min.y = roomMin.y;
                }
            }

            // Generate the AStar Pathfinder.
            AstarPath path = Instantiate(pathFinderPrefab);

            // Set the center and size for all grid graphs in the pathfinder.
            foreach (NavGraph navGraph in path.graphs)
            {
                GridGraph graph = navGraph as GridGraph;
                graph.center = (min + max) / 2;
                graph.SetDimensions(((int)max.x * 2) - ((int)min.x * 2), ((int)max.y * 2) - ((int)min.y * 2), .5f);
            }
            StartCoroutine(RecalculateGraph());
        }

        private IEnumerator RecalculateGraph()
        {
            yield return new WaitForSeconds(0);
            AstarPath.active.Scan();
            yield return new WaitForSeconds(5);

            foreach (NavGraph navGraph in AstarPath.active.graphs)
            {
                GridGraph graph = navGraph as GridGraph;
                if (graph.name == groundGraph)
                {
                    SpawnResources(graph, GameManager.PlayerEntity.transform.position);
                }
            }
            progressSettings.IsDoneScanning = true;
        }

        private void SpawnResources(GridGraph graph, Vector3 start)
        {
            List<GraphNode> nodes = PathUtilities.GetReachableNodes(AstarPath.active.GetNearest(start, NNConstraint.Default).node);
            int numberToSpawn = Random.Range(minCaches, maxCaches);

            for (int i = 0; i < numberToSpawn; i++)
            {
                int randomNode = Random.Range(0, nodes.Count);

                ResourceCache cache = Instantiate(resourceCachePrefab, ((Vector3)nodes[randomNode].position), Quaternion.identity);


                // TODO set up scalable type randomization with weight.
                int type = Random.Range(0, 2);
                ResourceType resourceType = type == 1 ? ResourceType.Dirt : ResourceType.Grass;

                cache.Setup(cacheSpriteDictionary[resourceType], resourceType);
            }
        }

        private Vector2 GetRandomCardinalDirection()
        {
            int direction = Random.Range(0, 4);

            switch (direction)
            {
                case 0:
                    return Vector2.right;
                case 1:
                    return Vector2.up;
                case 2:
                    return Vector2.left;
                case 3:
                    return Vector2.down;
                default:
                    return Vector2.zero;
            }
        }
    }

}