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
            return GameManager.PlayerStatusController.IsDead || _weapon == null;
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

        protected virtual void OnHit(OnHitEvent eventType)
        {
            DamageWorksheet damageWorksheet = new DamageWorksheet(myEntity, eventType.Target);
            damageWorksheet.BaseDamage = CalculateDamage();
            
            foreach (var effect in GameManager.SettingsManager.effectSettings.UnlockedEffects
                         .OfType<IExecuteEffect>()
                         .Where(e => e.TriggerType == EffectTriggerType.OnHit))
            {
                effect.Execute(damageWorksheet);
            }
            
            damageWorksheet.ApplyDamage();
        }
    }

    public class DamageWorksheet
    {
        public float BaseDamage;
        public List<float> DamageMultipliers;

        public float EffectDamage;
        public List<float> EffectMultipliers;

        public float TargetResistance;
        public float SourcePenetration;

        public Entity Source;
        public Entity Target;

        public DamageWorksheet(Entity source, Entity target)
        {
            DamageMultipliers = new();
            EffectMultipliers = new();
        }
        
        // Base damage * [weaponMult] + [effectDamage * effectMult]... - (armor * penetration)
        public void ApplyDamage()
        {
            float baseDmg = BaseDamage;

            foreach (var mult in DamageMultipliers)
            {
                baseDmg *= mult;
            }

            float effectDmg = EffectDamage;
            foreach (var mult in EffectMultipliers)
            {
                effectDmg *= mult;
            }

            float damageToDeal = baseDmg + effectDmg;

            float damageResist = TargetResistance - SourcePenetration;

            float finalDamage = damageToDeal - damageResist;
            Target.TakeDamage(finalDamage);
        }
    }
}
