using Cinemachine;
using System.Collections;
using UnityEngine;

namespace Minigames.Fight
{
    public class RoomController : MonoBehaviour
    {
        [SerializeField]
        private CinemachineVirtualCamera cam;

        [SerializeField]
        private float zoomSpeed;
        [SerializeField]
        private float zoomSize;

        private float startSize;

        private bool transition;

        private void Start()
        {
            cam.Follow = GameManager.PlayerEntity.transform;
            cam.Priority = 0;
            startSize = cam.m_Lens.OrthographicSize;
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.layer == PhysicsUtils.PlayerLayer)
            {
                if (GameManager.RoomManager.CurrentCam != null)
                {
                    GameManager.RoomManager.CurrentCam.Priority = 0;
                }
                StartCoroutine(StartTransition());
            }
        }
        private void Update()
        {
            if (!transition)
            {
                return;
            }
            if (cam.m_Lens.OrthographicSize >= startSize)
            {
                cam.m_Lens.OrthographicSize = startSize;
                transition = false;
                return;
            }
            if (cam.m_Lens.OrthographicSize < startSize)
            {
                cam.m_Lens.OrthographicSize += zoomSpeed * Time.deltaTime;
            }
        }
        private IEnumerator StartTransition()
        {
            GameManager.RoomManager.CurrentCam = cam;
            cam.m_Lens.OrthographicSize = 1;
            cam.Priority = 10;
            yield return new WaitForSeconds(.5f);
            transition = true;
        }
    }
}
