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
        protected float WeaponAbilityTimer;
        protected EventService EventService;
        protected Entity MyEntity;

        void Awake()
        {
            MyEntity = GetComponent<Entity>();

            EventService = GameManager.EventService;
        }

        public void Setup(Weapon weapon)
        {
            this.weapon = weapon;
            WeaponAbilityTimer = weapon.abilityCooldown;
        }

        protected virtual void Update()
        {
            if (ShouldPreventUpdate())
            {
                return;
            }

            ShotTimer += Time.deltaTime;
            WeaponAbilityTimer += Time.deltaTime;

            if(CanShoot())
            {
                ShotTimer = 0;
                Shoot();
            }

            if (CanUseWeaponAbility())
            {
                WeaponAbilityTimer = 0;
                EventService.Dispatch<PlayerUsedAbilityEvent>();
                UseWeaponAbility();
            }
        }

        protected virtual bool ShouldPreventUpdate()
        {
            return GameManager.PlayerEntity.IsDead || weapon == null;
        }
        
        protected virtual bool CanShoot()
        {
            return Input.GetMouseButton(0) && ShotTimer > weapon.fireRate;
        }

        protected virtual void Shoot()
        {
        }
        
        protected virtual bool CanUseWeaponAbility()
        {
            return Input.GetMouseButton(1) && WeaponAbilityTimer > weapon.abilityCooldown;
        }

        protected virtual void UseWeaponAbility()
        {
        }
    }
}
