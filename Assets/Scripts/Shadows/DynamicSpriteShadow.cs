using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Minigames.Fight
{
    public class DynamicSpriteShadow : MonoBehaviour
    {
        [SerializeField]
        private SpriteRenderer parentRenderer;
        [SerializeField]
        private SpriteRenderer myRenderer;

        private Sprite _lastKnownSprite;
        private bool _lastKnownFlipX;
        private void Start()
        {
            _lastKnownFlipX = parentRenderer.flipX;
            _lastKnownSprite = parentRenderer.sprite;
            myRenderer.sprite = GameManager.ShadowData.SpriteShadowMappings[parentRenderer.sprite].ShadowSprite(parentRenderer.flipX);
            myRenderer.material = parentRenderer.sharedMaterial;
            myRenderer.sortingLayerID = parentRenderer.sortingLayerID;
            myRenderer.sortingOrder = parentRenderer.sortingOrder - 1;
        }

        private void Update()
        {
            myRenderer.sortingOrder = parentRenderer.sortingOrder - 1;
            if (parentRenderer.sprite != _lastKnownSprite || _lastKnownFlipX != parentRenderer.flipX)
            {
                _lastKnownFlipX = parentRenderer.flipX;
                _lastKnownSprite = parentRenderer.sprite;

                myRenderer.sprite = GameManager.ShadowData.SpriteShadowMappings[parentRenderer.sprite].ShadowSprite(parentRenderer.flipX);
            }
        }
    }
}