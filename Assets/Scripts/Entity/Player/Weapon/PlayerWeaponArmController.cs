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

        private int _leftSortingOrder = 0;
        private int _rightSortingOrder = 0;

        private void Start()
        {
            _currentArm = leftArm;
            _cam = GameManager.PlayerEntity.PlayerCamera;

            Platform.EventService.Add<PlayerChangedDirectionEvent>(SwitchDirection);
        }

        public void SwitchDirection(PlayerChangedDirectionEvent e)
        {
            int baseLayer = GameManager.PlayerEntity.VisualController.SpriteRenderer.sortingOrder;
            
            switch (e.NewDirection)
            {
                case Direction.Down:
                    _leftSortingOrder = baseLayer + 1;
                    _rightSortingOrder = baseLayer + 1;
                    break;
                case Direction.Up:
                    _leftSortingOrder = baseLayer -1;
                    _rightSortingOrder = baseLayer -1;
                    break;
                case Direction.Left:
                    _leftSortingOrder = baseLayer -1;
                    _rightSortingOrder = baseLayer + 1;
                    break;
                case Direction.Right:
                    _leftSortingOrder = baseLayer + 1;
                    _rightSortingOrder = baseLayer -1;
                    break;
            }
        }
        
        private void Update()
        {
            if (GameManager.PlayerEntity.IsDead)
            {
                return;
            }
            leftArm.MySpriteRenderer.sortingOrder = _leftSortingOrder;
            rightArm.MySpriteRenderer.sortingOrder = _rightSortingOrder;
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

        public void PlayShootAnimation()
        {
            _currentArm.TryAttack();
        }
    }
}