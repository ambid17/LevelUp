using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Utils;

namespace Minigames.Fight
{
    
    public class PlayerWeaponArmController : MonoBehaviour
    {
        [SerializeField]
        private PlayerWeaponArm leftArm;
        [SerializeField]
        private PlayerWeaponArm rightArm;

        private PlayerWeaponArm _currentArm;
        private Camera _cam;
        private Direction _currentDirection;

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
                    leftSortingOrder = 1;
                    rightSortingOrder = -1;
                    break;
                case Direction.Right:
                    leftSortingOrder = -1;
                    rightSortingOrder = 1;
                    break;
            }
            leftArm.MySpriteRenderer.sortingOrder = leftSortingOrder;
            rightArm.MySpriteRenderer.sortingOrder = rightSortingOrder;
        }

        private void Update()
        {
            float currentRotation = TransformUtils.GetInspectorRotation(_currentArm.transform).z;
            float withinRange = Mathf.Clamp(currentRotation, _currentArm.MinRotation, _currentArm.MaxRotation);
            if (withinRange != currentRotation)
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
    }
}