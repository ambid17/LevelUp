using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Minigames.Fight
{
    public class StaticSpriteShadow : MonoBehaviour
    {
        public SpriteRenderer ParentRenderer { get => parentRenderer; set => parentRenderer = value; }
        public SpriteRenderer MyRenderer { get  => myRenderer; set => myRenderer = value; }
        [SerializeField]
        private SpriteRenderer parentRenderer;
        [SerializeField]
        private SpriteRenderer myRenderer;

        private void Start()
        {
            myRenderer.sprite = GameManager.ShadowData.SpriteShadowMappings[parentRenderer.sprite].ShadowSprite(parentRenderer.flipX);
            myRenderer.flipX = parentRenderer.flipX;
            myRenderer.material = parentRenderer.sharedMaterial;
            myRenderer.sortingLayerID = parentRenderer.sortingLayerID;
            myRenderer.sortingOrder = parentRenderer.sortingOrder - 1;
            transform.localScale = transform.parent.localScale;
        }
    }
}