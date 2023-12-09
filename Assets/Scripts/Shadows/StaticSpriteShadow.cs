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

        private void Awake()
        {
            myRenderer.sprite = GameManager.ShadowData.SpriteShadowMappings[parentRenderer.sprite].ShadowSprite(parentRenderer.flipX);
        }
    }
}