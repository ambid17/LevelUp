using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Utils;

namespace Minigames.Fight
{
    public class WeaponController : MonoBehaviour
    {
        [SerializeField] protected Weapon _weapon;
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
            _eventService.Add<OnHitEvent>(OnHit);
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

        protected virtual float CalculateDamage()
        {
            float damage = _weapon.Stats.Damage;

            if (_weapon.Stats.CritChance > 0)
            {
                float randomValue = Random.Range(0f, 1f);
                bool shouldCrit = randomValue < _weapon.Stats.CritChance;

                if (shouldCrit)
                {
                    damage *= _weapon.Stats.CritDamage;
                }
            }

            return damage;
        }

        // called when this weapon hits an enemy
        protected virtual void OnHit(OnHitEvent eventType)
        {
            Entity enemyEntity = eventType.Target;
            HitData hitData = new HitData(myEntity, enemyEntity);

            foreach (var effect in GameManager.SettingsManager.effectSettings.UnlockedEffects
                         .OfType<IExecuteEffect>()
                         .Where(e => e.TriggerType == EffectTriggerType.OnHit))
            {
                hitData.Effects.Add(effect);
            }
            
            enemyEntity.TakeHit(hitData);

        }
    }
}
