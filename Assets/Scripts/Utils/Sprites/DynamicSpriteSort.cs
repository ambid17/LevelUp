using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

    public class DynamicSpriteSort : MonoBehaviour
    {
        private int _baseSortingOrder;
        private Transform myTransform;

        [SerializeField] private List<SortableSprite> _sortableSprites;

        [ContextMenu("Gather sortableSprite from children")]
        public void Gather()
        {
            _sortableSprites.Clear();
            
            SpriteRenderer[] children = transform.GetComponentsInChildren<SpriteRenderer>(true);
            foreach (SpriteRenderer renderer in children)
            {
                _sortableSprites.Add(new SortableSprite(renderer, renderer.sortingOrder));
            }
            
            #if UNITY_EDITOR
            EditorUtility.SetDirty(gameObject);
            #endif
        } 

        private void Awake()
        {
            myTransform = GetComponent<Transform>();
        }

        private void Update()
        {
            _baseSortingOrder = myTransform.GetSortingOrder();

            foreach (var sortableSprites in _sortableSprites)
            {
                sortableSprites.spriteRenderer.sortingOrder = _baseSortingOrder + sortableSprites.relativeOrder;
            }
        }


        [Serializable]
        public struct SortableSprite
        {
            public SpriteRenderer spriteRenderer;
            // This is used so that a parent responsible for multiple sprites can order them
            // For example, the lavaling has the lavaling sprite, and the shadow
            // So since the lavaling should be in front of the shadow,
            // Lavaling relativeOrder = 1
            // Shadow relativeOrder = 0
            public int relativeOrder;

            public SortableSprite(SpriteRenderer spriteRenderer, int relativeOrder)
            {
                this.spriteRenderer = spriteRenderer;
                this.relativeOrder = relativeOrder;
            }
        }
    }