using System;
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
            MyEntity = GetComponent<Entity>();

            EventService = Platform.EventService;
        }

        protected virtual void Start()
        {
        }

        public virtual void Setup(Weapon weapon)
        {
            this.weapon = weapon;
        }

        protected virtual void Update()
        {
            if (ShouldPreventUpdate())
            {
                return;
            }

            ShotTimer += Time.deltaTime;
        }

        protected virtual bool ShouldPreventUpdate()
        {
            return GameManager.PlayerEntity.IsDead || weapon == null;
        }
        
        protected virtual bool CanShoot()
        {
            return Input.GetMouseButton(0) && ShotTimer > weapon.fireRate;
        }

        public virtual void Shoot()
        {
        }
    }
}
