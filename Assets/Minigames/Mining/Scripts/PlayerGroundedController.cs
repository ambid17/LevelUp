using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Mining{
public class PlayerGroundedController : MonoBehaviour
    {
        PlayerController _playerController;
        // Start is called before the first frame update
        void Awake()
        {
            _playerController = GetComponentInParent<PlayerController>();
        }

        void OnTriggerEnter2D (Collider2D col)
        {
            if(col.gameObject.layer == PhysicsUtils.GroundLayer)
                _playerController.SetGrounded(true);
        }

        void OnTriggerExit2D(Collider2D col)
        {
            if(col.gameObject.layer == PhysicsUtils.GroundLayer)
                _playerController.SetGrounded(false);
        }
    }
}