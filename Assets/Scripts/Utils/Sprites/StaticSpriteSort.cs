using System.Collections;
using System.Collections.Generic;
using UnityEngine;


    public class StaticSpriteSort : MonoBehaviour
    {
        void Awake()
        {
            var spriteRenderer = GetComponent<SpriteRenderer>();
            spriteRenderer.sortingOrder = transform.GetSortingOrder();
        }
    }
