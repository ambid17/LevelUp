using Cinemachine;
using UnityEngine;

namespace Minigames.Fight
{
    public class RoomController : MonoBehaviour
    {
        [SerializeField]
        private CinemachineVirtualCamera cam;

        private void Start()
        {
            cam.Follow = GameManager.PlayerEntity.transform;
            cam.Priority = 0;
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.layer == PhysicsUtils.PlayerLayer)
            {
                cam.Priority = 10;
            }
        }
        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.gameObject.layer == PhysicsUtils.PlayerLayer)
            {
                cam.Priority = 0;
            }
        }
    }
}
