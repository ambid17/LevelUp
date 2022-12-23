using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
namespace Mining
{
    public class GridManager : Singleton<GridManager>
    {
        [SerializeField]
        List<Tilemap> tilemaps;
        GameObject grid;
        public override void Initialize()
        {
            grid = gameObject;
            tilemaps = new List<Tilemap>();
            foreach(Tilemap tilemap in grid.GetComponentsInChildren<Tilemap>())
            {
                tilemaps.Add(tilemap);
            }
        }

        public void MineCell(Vector3Int hitPos)
        {
            foreach (Tilemap tilemap in tilemaps)
            {
                tilemap.SetTile(hitPos, null);
            }
                
        }

    }
}
