using Cinemachine;
using Pathfinding;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Minigames.Fight
{
    public class RoomManager : Singleton<RoomManager>
    {
        public CinemachineVirtualCamera CurrentCam { get; set; }

        [SerializeField]
        private AstarPath pathFinderPrefab;

        private RoomSettings _roomSettings;
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
                graph.SetDimensions((int)max.x - (int)min.x, (int)max.y - (int)min.y, 1);
            }

            // Rescan pathfinding.
            path.Scan();
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