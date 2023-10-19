using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Utils;

namespace Minigames.Fight
{
    
    public class PlayerWeaponArmController : MonoBehaviour
    {
        public PlayerWeaponArm CurrentArm => _currentArm ?? leftArm;

        [SerializeField]
        private PlayerWeaponArm leftArm;
        [SerializeField]
        private PlayerWeaponArm rightArm;

        private PlayerWeaponArm _currentArm;
        private Camera _cam;

        private void Start()
        {
            _currentArm = leftArm;
            _cam = GameManager.PlayerEntity.PlayerCamera;

            GameManager.EventService.Add<PlayerChangedDirectionEvent>(SwitchDirection);
        }

        public void SwitchDirection(PlayerChangedDirectionEvent e)
        {
            int leftSortingOrder = 0;
            int rightSortingOrder = 0;
            switch (e.NewDirection)
            {
                case Direction.Down:
                    leftSortingOrder = 1;
                    rightSortingOrder = 1;
                    break;
                case Direction.Up:
                    leftSortingOrder = -1;
                    rightSortingOrder = -1;
                    break;
                case Direction.Left:
                    leftSortingOrder = -1;
                    rightSortingOrder = 1;
                    break;
                case Direction.Right:
                    leftSortingOrder = 1;
                    rightSortingOrder = -1;
                    break;
            }
            leftArm.MySpriteRenderer.sortingOrder = leftSortingOrder;
            rightArm.MySpriteRenderer.sortingOrder = rightSortingOrder;
        }
        
        private void Update()
        {
            if (GameManager.PlayerEntity.IsDead)
            {
                return;
            }
            float currentRotation = _currentArm.transform.rotation.eulerAngles.z;
            if (currentRotation < _currentArm.MinRotation && currentRotation > _currentArm.MaxRotation)
            {
                _currentArm.ReturnToIdle();
                _currentArm = SwitchArms();
                _currentArm.StopAllCoroutines();
            }

            _currentArm.transform.rotation = PhysicsUtils.LookAt(transform, _cam.ScreenToWorldPoint(Input.mousePosition), _currentArm.StartRotation);
        }
        private PlayerWeaponArm SwitchArms()
        {
            if (_currentArm == leftArm)
            {
                return rightArm;
            }
            return leftArm;
        }
        public void Playshoot()
        {
            _currentArm.AnimationController.PlayShootAnimation();
        }
    }
}