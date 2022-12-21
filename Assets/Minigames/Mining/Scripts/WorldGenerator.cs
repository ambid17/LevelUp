using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
namespace Mining
{
    public class WorldGenerator : MonoBehaviour
    {
        [SerializeField] RuleTile stoneTile;
        [SerializeField] int width, height;
        void Awake()
        {
            FillWithStone();
        }


        /// <summary>
        /// Fill the TileMap this script is attached to with the ruleTile provided in the inspector, 
        /// creating a solid rectangle with corners [-width/2, 0] and [width/2, -height]
        /// </summary>
        void FillWithStone()
        {
            Tilemap tilemap = GetComponent<Tilemap>();
            for(int x = -width/2; x < width/2; x++)
            {
                for(int y = 0; y > -height; y--)
                {
                    tilemap.SetTile(new Vector3Int(x, y, 0), stoneTile);
                }
            }
        }
    }
}

