using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Minigames.Fight
{
    public class MinimapRoomRender : MonoBehaviour
    {
        public Color activeColor;
        public Color defaultColor;

        public SpriteRenderer spriteRenderer;

        private void Start()
        {
            spriteRenderer.color = defaultColor;
        }

        public void Activate()
        {
            spriteRenderer.color = activeColor;
        }
    }
}