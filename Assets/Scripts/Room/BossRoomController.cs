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
        [SerializeField]
        private GameObject exit;

        private bool _fightOver => _hasActivated && _boss == null;

        private RoomConnection _entrance;
        private EntityBehaviorData _boss;
        private bool _hasActivated = false;
        private bool _bossDefeated = false;
        private List<EntityBehaviorData> _enemiesToReactivate = new();
        private Bounds _entranceBounds;

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

            StartPlayerPathing();
        }

        private void StartPlayerPathing()
        {
            GameManager.CameraLerp.PlayCinematic(GameManager.PlayerEntity.transform);
            PlayerPathfindingMovementController pathfindingMovementController = GameManager.PlayerEntity.gameObject.AddComponent<PlayerPathfindingMovementController>();
            pathfindingMovementController.StartPath(playerEntryDestination.position, PlayerControlledActionType.BossRoomEntry);
        }

        private void OnPlayerEntered(PlayerControlledActionFinishedEvent e)
        {
            GameManager.CameraLerp.EndCinematic();
            if (e.ActionType != PlayerControlledActionType.BossRoomEntry)
            {
                return;
            }
            _entrance = roomConnections.FirstOrDefault(r => r.HasConnection);
            float xMin = float.MaxValue;
            float yMin = float.MaxValue;
            float xMax = float.MinValue;
            float yMax = float.MinValue;
            foreach (Vector3Int tilePos in _entrance.TilePositions)
            {
                Vector3 worldPos = Tilemap.CellToWorld(tilePos);
                if (worldPos.x < xMin)
                {
                    xMin = worldPos.x;
                }
                if (worldPos.y < yMin)
                {
                    yMin = worldPos.y;
                }
                if (worldPos.x > xMax)
                {
                    xMax = worldPos.x;
                }
                if (worldPos.y > yMax)
                {
                    yMax = worldPos.y;
                }

                Tilemap.SetTile(tilePos, GameManager.ProgressSettings.CurrentBiome.RoomSettings.wallTile);
            }
            Vector2 min = new(xMin - 2, yMin -2);
            Vector2 max = new(xMax +2, yMax + 2);

            Vector2 extents = (max - min) * .5f;
            Vector2 center = min + extents;
            _entranceBounds = new(center, extents * 2);

            var gou = new GraphUpdateObject(_entranceBounds);
            AstarPath.active.UpdateGraphs(gou);

            _boss = GameManager.EnemyObjectPool.AllEnemies.FirstOrDefault(b => b.room == this);
            _boss.transform.parent = null;
            _boss.transform.position = bossOrigin.position;
            _boss.gameObject.SetActive(true);
            _hasActivated = true;
            GameManager.CameraLerp.PlayCinematic(_boss.transform);
        }
        
        private void OnBossEntered()
        {
            GameManager.PlayerEntity.IsControlled = false;
            GameManager.CameraLerp.EndCinematic();
        }

        private void OnBossDefeated()
        {
            _bossDefeated = true;
            GameManager.ProgressSettings.CompleteFloor();
            constructionChamber.gameObject.SetActive(true);
            exit.SetActive(true);
            var gou = new GraphUpdateObject(constructionChamber.SpriteRenderer.bounds);
            AstarPath.active.UpdateGraphs(gou);
            foreach (Vector3Int tilePos in _entrance.TilePositions)
            {
                Tilemap.SetTile(tilePos, null);
            }
            gou = new GraphUpdateObject(_entranceBounds);
            AstarPath.active.UpdateGraphs(gou);
            foreach (EntityBehaviorData enemy in _enemiesToReactivate)
            {
                enemy.gameObject.SetActive(true);
            }
            _enemiesToReactivate.Clear();
        }
    }
}