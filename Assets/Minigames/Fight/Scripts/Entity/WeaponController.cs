using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Utils;

namespace Minigames.Fight
{
    public class WeaponController : MonoBehaviour
    {
        [SerializeField] protected Weapon weapon;
        public Weapon Weapon => weapon;
        protected float ShotTimer;
        protected EventService EventService;
        protected Entity MyEntity;

        void Awake()
        {
            MyEntity = GetComponentInParent<Entity>();

            EventService = GameManager.EventService;
        }

        public void Setup(Weapon weapon)
        {
            this.weapon = weapon;
        }

        void Update()
        {
            if (ShouldPreventUpdate())
            {
                return;
            }

            ShotTimer += Time.deltaTime;

            if (CanShoot())
            {
                ShotTimer = 0;
                Shoot();
            }
        }

        protected virtual bool ShouldPreventUpdate()
        {
            return GameManager.PlayerEntity.IsDead || weapon == null;
        }
        
        protected virtual bool CanShoot()
        {
            return Input.GetMouseButton(0) && ShotTimer > weapon.Stats.FireRate;
        }

        protected virtual void Shoot()
        {
        }
    }
}
