using Cinemachine;
using System.Collections.Generic;
using UnityEngine;

namespace Minigames.Fight
{
    public class RoomManager : Singleton<RoomManager>
    {
        public CinemachineVirtualCamera CurrentCam { get; set; }

        private RoomSettings _roomSettings;
        protected override void Initialize()
        {
            _roomSettings = GameManager.SettingsManager.progressSettings.CurrentWorld.RoomSettings;

            List<RoomController> availableRooms = new();

            // Instantiate start room.
            var startRoom = Instantiate(_roomSettings.startRoom);
            availableRooms.Add(startRoom);
            var lastRoom = startRoom;

            int roomCount = Random.Range(_roomSettings.minRooms, _roomSettings.maxRooms + 1);
            List<RoomController> roomsToInstantiate = new();
            Vector2 direction = Vector2.zero;
            for (int i = 0; i < roomCount; i++)
            {
                roomsToInstantiate.Add(_roomSettings.GetRandomRoom());
            }
            foreach (RoomController roomToInstantiate in roomsToInstantiate)
            {
                // Pick direction(s).
                bool valid = false;
                var room = roomToInstantiate;

                Vector2 center = Vector2.zero;

                while (!valid)
                {
                    int roomToBranchOffOf = Random.Range(0, availableRooms.Count);
                    lastRoom = availableRooms[roomToBranchOffOf];
                    direction = GetRandomCardinalDirection();
                    float x = (lastRoom.col.bounds.extents.x + (room.Tilemap.cellBounds.AsVector2().x / 2)) * direction.x;
                    float y = (lastRoom.col.bounds.extents.y + (room.Tilemap.cellBounds.AsVector2().y / 2)) * direction.y;
                    center = lastRoom.col.bounds.center.AsVector2() + new Vector2(x, y);


                    // Abort before doing the overlap check if the chosen direction has already been used.
                    RoomConnection roomConnection = null;
                    foreach (RoomConnection connection in lastRoom.roomConnections)
                    {
                        if (connection.Direction == direction)
                        {
                            if (connection.HasConnection)
                            {
                                continue;
                            }
                            roomConnection = connection;
                        }
                    }

                    if (!Physics2D.OverlapBox(center, room.Tilemap.cellBounds.AsVector2() * 0.99f, 0))
                    {
                        valid = true;
                        roomConnection.HasConnection = true;
                    }
                }

                // Instantiate room in direction.
                var roomInstance = Instantiate(room, center - room.Tilemap.cellBounds.center.AsVector2(), Quaternion.identity);

                foreach (RoomConnection connection in roomInstance.roomConnections)
                {
                    if (connection.Direction == -direction)
                    {
                        connection.HasConnection = true;
                    }
                }

                availableRooms.Add(roomInstance);
            }

            // Close off unused exits.
            foreach (RoomController room in availableRooms)
            {
                room.CloseExits(_roomSettings.wallTile);
            }

            // Set pathfinding grid bounds.


            // Rescan pathfinding.
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