using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Minigames.Mining
{
    public class PlayerGroundedController : MonoBehaviour
    {
        PlayerController _playerController;

        void Awake()
        {
            _playerController = GetComponentInParent<PlayerController>();
        }

        void OnTriggerEnter2D(Collider2D col)
        {
            if (col.gameObject.layer == PhysicsUtils.GroundLayer)
                _playerController.SetGrounded(true);
        }

        void OnTriggerExit2D(Collider2D col)
        {
            if (col.gameObject.layer == PhysicsUtils.GroundLayer)
                _playerController.SetGrounded(false);
        }
    }
}