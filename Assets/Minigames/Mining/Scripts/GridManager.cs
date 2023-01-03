using Minigames.Mining;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
namespace Mining
{
    public class GridManager : Singleton<GridManager>
    {
        [SerializeField]
        Tilemap _rockTilemap;
        [SerializeField]
        Tilemap _oreTilemap;
        GameObject grid;
        [SerializeField] TileSettings _tileSettings;
        [SerializeField] RuleTile stoneTile;
        [SerializeField] int width, height;
        public override void Initialize()
        {
            grid = gameObject;
            
            FillWithStone();
            FillWithOre();
        }

        public void MineCell(Vector3Int hitPos)
        {
            _tileSettings.AddToInventory(_oreTilemap.GetTile<MiningTile>(hitPos).TileType);
            _rockTilemap.SetTile(hitPos, null);
            _oreTilemap.SetTile(hitPos, null);

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
                    _oreTilemap.SetTile(new Vector3Int(x, y, 0), _tileSettings.GetRandomTile().Tile);
                }
            }
        }
    }
}
