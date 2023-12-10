using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Minigames.Fight
{
    public class StaticSpriteShadow : MonoBehaviour
    {
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
        }
    }
}