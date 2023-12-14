using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Minigames.Fight
{
    public class MinimapRoomRender : MonoBehaviour
    {
        [SerializeField]
        private Color activeColor;
        [SerializeField]
        private SpriteRenderer spriteRenderer;

        public void Activate()
        {
            spriteRenderer.color = activeColor;
        }
    }
}