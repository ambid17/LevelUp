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
        public HitData Hit => hit;
        protected float ShotTimer;

        protected EventService EventService;
        protected Entity MyEntity;
        protected HitData hit;

        void Awake()
        {
            MyEntity = GetComponent<Entity>();

            EventService = Platform.EventService;
        }

        protected virtual void Start()
        {
            CalculateHitData();
        }

        public virtual void CalculateHitData()
        {
            hit = new HitData(MyEntity, weapon.damage);
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
