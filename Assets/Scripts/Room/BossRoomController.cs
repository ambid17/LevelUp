using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor.MemoryProfiler;
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
        public Vector2 BossEntry => bossEntry.position;

        [SerializeField]
        private Transform playerEntryDestination;
        [SerializeField]
        private Transform bossOrigin;
        [SerializeField]
        private Transform bossEntry;
        [SerializeField]
        private ConstructionChamber constructionChamber;

        private bool _fightOver => _hasActivated && _boss == null;

        private RoomConnection _entrance;
        private EntityBehaviorData _boss;
        private bool _hasActivated = false;
        private bool _bossDefeated = false;
        private List<EntityBehaviorData> _enemiesToReactivate = new();

        private void Start()
        {
            Platform.EventService.Add<PlayerControlledActionFinishedEvent>(OnPlayerEntered);
            Platform.EventService.Add<BossEnteredEvent>(OnBossEntered);
        }

        private void Update()
        {
            if (!_fightOver)
            {
                return;
            }
            if (_bossDefeated)
            {
                return;
            }
            OnBossDefeated();
        }

        // Using initialize enemies for entire boss opening sequence, possibly rename to initialize room for clarity?
        protected override void InitializeEnemies()
        {
            foreach (EntityBehaviorData enemy in GameManager.EnemyObjectPool.AllEnemies)
            {
                if (enemy.gameObject.activeInHierarchy)
                {
                    _enemiesToReactivate.Add(enemy);
                    enemy.gameObject.SetActive(false);
                }
            }
            AstarPath.OnLatePostScan += StartPlayerPathing;

            GridGraph graph = AstarPath.active.graphs.First(g => g.graphIndex == PhysicsUtils.playerGraph) as GridGraph;
            graph.center = MyCollider.bounds.center;
            graph.SetDimensions(Tilemap.cellBounds.size.x * 4, Tilemap.cellBounds.size.y * 4, .25f);
            graph.Scan();

        }

        private void StartPlayerPathing(AstarPath script)
        {
            PlayerPathfindingMovementController pathfindingMovementController = GameManager.PlayerEntity.gameObject.AddComponent<PlayerPathfindingMovementController>();
            pathfindingMovementController.StartPath(playerEntryDestination.position, PlayerControlledActionType.BossRoomEntry);
            AstarPath.OnLatePostScan -= StartPlayerPathing;
        }

        private void OnPlayerEntered(PlayerControlledActionFinishedEvent e)
        {
            if (e.ActionType != PlayerControlledActionType.BossRoomEntry)
            {
                return;
            }
            _entrance = roomConnections.First(r => r.HasConnection);
            foreach (Vector3Int tilePos in _entrance.TilePositions)
            {
                Tilemap.SetTile(tilePos, GameManager.ProgressSettings.CurrentWorld.RoomSettings.wallTile);
                var gou = new GraphUpdateObject(MyCollider.bounds);
                AstarPath.active.UpdateGraphs(gou);
            }
            _boss = GameManager.EnemyObjectPool.AllEnemies.First(b => b.room == this);
            _boss.transform.parent = null;
            _boss.transform.position = bossOrigin.position;
            _boss.gameObject.SetActive(true);
            _hasActivated = true;
        }
        
        private void OnBossEntered()
        {
            GameManager.PlayerEntity.IsControlled = false;
        }

        private void OnBossDefeated()
        {
            _bossDefeated = true;
            constructionChamber.gameObject.SetActive(true);
            foreach (Vector3Int tilePos in _entrance.TilePositions)
            {
                Tilemap.SetTile(tilePos, null);
                var gou = new GraphUpdateObject(MyCollider.bounds);
                AstarPath.active.UpdateGraphs(gou);
            }
            foreach (EntityBehaviorData enemy in _enemiesToReactivate)
            {
                enemy.gameObject.SetActive(true);
            }
            _enemiesToReactivate.Clear();
        }
    }
}