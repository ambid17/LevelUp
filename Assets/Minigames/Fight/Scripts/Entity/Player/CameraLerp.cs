using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Minigames.Fight
{
    public class CameraLerp : MonoBehaviour
    {
        [SerializeField]
        private float lerpFactor;

        private Camera cam;
        private void Start()
        {
            cam = Camera.main;
        }
        private void Update()
        {
            Vector2 mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
            Vector2 playerPos = GameManager.PlayerEntity.transform.position;

            transform.position = Vector2.Lerp(playerPos, mousePos, lerpFactor);
        }
    }
}