using System.Collections;
using System.Collections.Generic;
using UnityEditor.MPE;
using UnityEngine;

namespace Minigames.Fight
{
    public class PlayerWeaponController : WeaponController
    {
        protected PlayerEntity _overridenEntity;

        public PlayerWeaponArm CurrentArm => _currentArm ?? leftArm;

        [SerializeField]
        private PlayerWeaponArm leftArm;
        [SerializeField]
        private PlayerWeaponArm rightArm;

        private PlayerWeaponArm _currentArm;
        private Camera _cam;
        private bool _isTryingToShoot;

        private int _leftSortingOrder = 0;
        private int _rightSortingOrder = 0;

        protected override void Start()
        {
            base.Start();

            _currentArm = leftArm;
            _cam = GameManager.PlayerCamera;

            Platform.EventService.Add<PlayerChangedDirectionEvent>(SwitchDirection);
            Platform.EventService.Dispatch(new PlayerChangedWeaponEvent(CurrentWeaponMode));
        }

        protected override void Update()
        {
            base.Update();

            if (GameManager.PlayerEntity.IsDead)
            {
                return;
            }
            ControlArms();
            if (CanShoot())
            {
                TryShoot();
            }

            if (CanMelee())
            {
                TryMelee();
            }
            if (Input.mouseScrollDelta.y != 0 || Input.GetKeyDown(KeyCode.Space))
            {
                WeaponMode otherWeapon = CurrentWeaponMode == WeaponMode.Projectile ? WeaponMode.Melee : WeaponMode.Projectile;
                Platform.EventService.Dispatch(new PlayerChangedWeaponEvent(otherWeapon));
                if (otherWeapon == WeaponMode.Projectile)
                {
                    Platform.EventService.Dispatch(new PlayerAmmoUpdatedEvent((int)_combatStats.projectileWeaponStats.currentAmmo, (int)_combatStats.projectileWeaponStats.maxAmmo.Calculated));
                }
                CurrentWeaponMode = otherWeapon;
            }
            if (!_isTryingToShoot)
            {
                _combatStats.projectileWeaponStats.TryRegenAmmo();
            }
        }

        private void LateUpdate()
        {
            // Moved to LateUpdate because it always seemed to apply before the player setting it's layer.
            leftArm.MySpriteRenderer.sortingOrder = MyEntity.VisualController.SpriteRenderer.sortingOrder + _leftSortingOrder;
            rightArm.MySpriteRenderer.sortingOrder = MyEntity.VisualController.SpriteRenderer.sortingOrder + _rightSortingOrder;
        }

        private void ControlArms()
        {
            float currentRotation = _currentArm.transform.rotation.eulerAngles.z;
            if (currentRotation < _currentArm.MinRotation && currentRotation > _currentArm.MaxRotation)
            {
                _currentArm.ReturnToIdle();
                _currentArm = SwitchArms();
                _currentArm.StopAllCoroutines();
            }

            _currentArm.transform.rotation = PhysicsUtils.LookAt(transform, _cam.ScreenToWorldPoint(Input.mousePosition), _currentArm.StartRotation);
        }

        private void TryShoot()
        {
            if (CurrentWeaponMode == WeaponMode.Melee)
            {
                return;
            }
            CurrentArm.Attack(_combatStats.projectileWeaponStats.rateOfFire.Calculated);
        }

        private void TryMelee()
        {
            if (CurrentWeaponMode == WeaponMode.Projectile)
            {
                return;
            }
            CurrentArm.Attack(_combatStats.meleeWeaponStats.rateOfFire.Calculated);
        }

        public void SwitchDirection(PlayerChangedDirectionEvent e)
        {

            switch (e.NewDirection)
            {
                case Direction.Down:
                    _leftSortingOrder = + 1;
                    _rightSortingOrder = + 1;
                    break;
                case Direction.Up:
                    _leftSortingOrder = - 1;
                    _rightSortingOrder = - 1;
                    break;
                case Direction.Left:
                    _leftSortingOrder = -1;
                    _rightSortingOrder = + 1;
                    break;
                case Direction.Right:
                    _leftSortingOrder = + 1;
                    _rightSortingOrder = - 1;
                    break;
            }
        }

        private PlayerWeaponArm SwitchArms()
        {
            if (_currentArm == leftArm)
            {
                return rightArm;
            }
            return leftArm;
        }

        public override bool CanShoot()
        {
            _isTryingToShoot = Input.GetKey(KeyCode.Mouse0) &&
                _combatStats.projectileWeaponStats.currentAmmo > 0 && 
                CurrentWeaponMode == WeaponMode.Projectile;
            return ShootTimer >=
                _combatStats.projectileWeaponStats.rateOfFire.Calculated && _isTryingToShoot;
                
        }
        public override bool CanMelee()
        {
            return MeleeTimer >= 
                _combatStats.meleeWeaponStats.rateOfFire.Calculated && 
                Input.GetKey(KeyCode.Mouse0);
        }

        public override void Shoot()
        {
            for (int i = 0; i < _combatStats.projectileWeaponStats.projectilesPerShot.Calculated; i++)
            {
                ProjectileController projectile = Instantiate(_combatStats.projectileWeaponStats.projectilePrefab);

                Vector2 direction = GameManager.PlayerCamera.ScreenToWorldPoint(Input.mousePosition) - transform.position;

                // Map the indices to start from the leftmost projectile and spawn them to the right using the offset
                float indexOffset = (float)i - i / 2;
                Vector2 offset = Vector2.Perpendicular(direction).normalized * indexOffset;

                float spreadX = Random.Range(-_combatStats.projectileWeaponStats.projectileSpread.Calculated, _combatStats.projectileWeaponStats.projectileSpread.Calculated);
                float spreadY = Random.Range(-_combatStats.projectileWeaponStats.projectileSpread.Calculated, _combatStats.projectileWeaponStats.projectileSpread.Calculated);

                Vector2 spread = new Vector2(spreadX, spreadY);

                direction += spread.normalized;

                projectile.transform.position = CurrentArm.ProjectileOrigin.position.AsVector2() + offset;

                projectile.Setup(MyEntity, MyEntity.Stats.combatStats.projectileWeaponStats, direction.normalized);
            }
            _combatStats.projectileWeaponStats.ConsumeAmmo(1);

            ShootTimer = 0;
        }

        public override void Melee()
        {
            for (int i = 0; i < _combatStats.meleeWeaponStats.projectilesPerShot.Calculated; i++)
            {
                ProjectileController projectile = Instantiate(_combatStats.meleeWeaponStats.projectilePrefab);

                Vector2 direction = GameManager.PlayerCamera.ScreenToWorldPoint(Input.mousePosition) - transform.position;

                // Map the indices to start from the leftmost projectile and spawn them to the right using the offset
                float indexOffset = (float)i - i / 2;
                Vector2 offset = Vector2.Perpendicular(direction).normalized * indexOffset * _combatStats.meleeWeaponStats.projectileSpread.Calculated;

                projectile.transform.position = CurrentArm.ProjectileOrigin.position.AsVector2() + offset;

                projectile.transform.rotation = PhysicsUtils.LookAt(projectile.transform, GameManager.PlayerCamera.ScreenToWorldPoint(Input.mousePosition), 180);

                projectile.Setup(MyEntity, MyEntity.Stats.combatStats.meleeWeaponStats, direction);
            }
            MeleeTimer = 0;
        }
    }
}