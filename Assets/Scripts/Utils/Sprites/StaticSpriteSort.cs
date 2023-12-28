using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;


public class StaticSpriteSort : MonoBehaviour
{
    [SerializeField]
    private float offset = 0;
        void Awake()
    {
        var spriteRenderer = GetComponent<SpriteRenderer>();

        if (spriteRenderer != null)
        {
            spriteRenderer.sortingOrder = transform.GetSortingOrder(offset);
            return;
        }
        var tileRenderer = GetComponent<TilemapRenderer>();
        tileRenderer.sortingOrder = transform.GetSortingOrder(offset);
    }
}
