using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Minigames.Fight
{
    /// <summary>
    /// player enter room
    /// setup player pathfinder grid
    /// remove control from player
    /// player walk to center of room
    /// room exit closes
    /// boss enters
    /// return control to player
    /// boss dies
    /// exit opens
    /// construction chamber appears
    /// </summary>
    public class BossRoomController : RoomController
    {
        public ConstructionChamber ConstructionChamber => constructionChamber;

        [SerializeField]
        private Transform playerEntryDestination;
        [SerializeField]
        private ConstructionChamber constructionChamber;

        private bool _fightOver => hasInitialized && _boss == null;

        private RoomConnection _entrance;
        private EntityBehaviorData _boss;

        private void Start()
        {
            Platform.EventService.Add<PlayerControlledActionFinishedEvent>(OnPlayerEntered);
        }

        // Using initialize enemies for entire boss opening sequence, possibly rename to initialize room for clarity?
        protected override void InitializeEnemies()
        {
            GridGraph graph = AstarPath.active.graphs.First(g => g.graphIndex == PhysicsUtils.playerGraph) as GridGraph;
            graph.center = MyCollider.bounds.center;
            graph.SetDimensions(Tilemap.cellBounds.size.x * 4, Tilemap.cellBounds.size.y * 4, .25f);
            graph.Scan();

            PlayerPathfindingMovementController pathfindingMovementController = GameManager.PlayerEntity.gameObject.AddComponent<PlayerPathfindingMovementController>();
            pathfindingMovementController.StartPath(playerEntryDestination.position, PlayerControlledActionType.BossRoomEntry);
        }

        private void OnPlayerEntered(PlayerControlledActionFinishedEvent e)
        {

        }
    }
}