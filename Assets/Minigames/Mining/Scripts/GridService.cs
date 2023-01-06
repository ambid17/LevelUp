using Minigames.Mining;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Utils;

namespace Minigames.Mining
{
    public class GridService : MonoBehaviour
    {
        [SerializeField]
        Tilemap _rockTilemap;
        [SerializeField]
        Tilemap _oreTilemap;
        GameObject grid;
        [SerializeField] RuleTile stoneTile;
        [SerializeField] int width, height;
        private EventService _eventService;
        public Tilemap Tilemap => _rockTilemap;
        void Awake()
        {
            grid = gameObject;
            
            FillWithStone();
            FillWithOre();
        }

        private void Start()
        {
            _eventService = GameManager.EventService;
        }

        public void MineCell(Vector3 hitPos)
        {
            Vector3Int cellPos = _rockTilemap.WorldToCell(hitPos);

            MiningTile miningTile = _oreTilemap.GetTile<MiningTile>(cellPos);

            if(miningTile) 
            {
                if(miningTile.TileType == TileType.Lava)
                {
                    GameManager.MiningProgressSettings.HullHealth -= 1;
                    _eventService.Dispatch<OnPlayerDamageEvent>();
                    _eventService.Dispatch<OnHealthUpdatedEvent>();
                }
                


                GameManager.TileSettings.AddToInventory(miningTile.TileType);
            }
            _rockTilemap.SetTile(cellPos, null);
            _oreTilemap.SetTile(cellPos, null);

        }

        void FillWithStone()
        {
            for (int x = -width / 2; x < width / 2; x++)
            {
                for (int y = 0; y > -height; y--)
                {
                    _rockTilemap.SetTile(new Vector3Int(x, y, 0), stoneTile);
                }
            }
        }

        void FillWithOre()
        {
            for (int x = -width / 2; x < width / 2; x++)
            {
                for (int y = 0; y > -height; y--)
                {
                    _oreTilemap.SetTile(new Vector3Int(x, y, 0), GameManager.TileSettings.GetRandomTile().Tile);
                }
            }
        }
    }
}
