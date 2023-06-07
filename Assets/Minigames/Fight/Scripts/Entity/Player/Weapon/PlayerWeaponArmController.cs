using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Minigames.Fight
{
    public class PlayerWeaponArmController : MonoBehaviour
    {
        private void Update()
        {
            Camera cam = GameManager.PlayerEntity.PlayerCamera;
            transform.rotation = PhysicsUtils.LookAt(transform, cam.ScreenToWorldPoint(Input.mousePosition), 180);
        }
    }
}