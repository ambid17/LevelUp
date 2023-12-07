using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class DynamicSpriteSort : MonoBehaviour
{
    private int _baseSortingOrder;
    private Transform myTransform;

    [SerializeField] private SpriteRenderer spriteRenderer;

    
    private void Awake()
    {
        myTransform = GetComponent<Transform>();
    }

    [ContextMenu("GetComponent")]
    public void GetComponent()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        _baseSortingOrder = myTransform.GetSortingOrder();

        spriteRenderer.sortingOrder = _baseSortingOrder;
    }
}