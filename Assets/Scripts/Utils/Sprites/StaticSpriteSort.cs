using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class StaticSpriteSort : MonoBehaviour
{
    [SerializeField]
    private float offset = 0;
        void Awake()
    {
        var spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sortingOrder = transform.GetSortingOrder(offset);
    }
}
