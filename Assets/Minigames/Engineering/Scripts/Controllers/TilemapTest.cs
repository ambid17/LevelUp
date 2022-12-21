using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TilemapTest : MonoBehaviour
{
    private Tilemap tileMap;

    [SerializeField]
    private float minX;
    [SerializeField]
    private float maxX;
    private void Awake()
    {
        tileMap = GetComponent<Tilemap>();
        Vector2 min = tileMap.transform.TransformPoint(tileMap.cellBounds.min);
        Vector2 max = tileMap.transform.TransformPoint(tileMap.cellBounds.max);
        minX = min.x;
        maxX = max.x;
    }
}
