using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Utils;

namespace Minigames.Fight
{
    public class WeaponController : MonoBehaviour
    {
        [SerializeField] protected Weapon _weapon;
        public Weapon Weapon => _weapon;
        protected float _shotTimer = 0;
        protected Camera _camera;
        protected EventService _eventService;
        public Entity myEntity;

        void Awake()
        {
            _camera = Camera.main;

            if (myEntity == null)
            {
                myEntity = GetComponentInParent<Entity>();
            }

            _eventService = GameManager.EventService;
        }

        public void Setup(Weapon weapon)
        {
            _weapon = weapon;
        }

        void Update()
        {
            if (ShouldPreventUpdate())
            {
                return;
            }

            _shotTimer += Time.deltaTime;

            if (CanShoot())
            {
                _shotTimer = 0;
                Shoot();
            }
        }

        protected virtual bool ShouldPreventUpdate()
        {
            return GameManager.PlayerEntity.IsDead || _weapon == null;
        }
        
        protected virtual bool CanShoot()
        {
            return Input.GetMouseButton(0) && _shotTimer > _weapon.Stats.FireRate;
        }

        protected virtual void Shoot()
        {
        }
    }
}
