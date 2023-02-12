using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Utils;

namespace Minigames.Fight
{
    public class WeaponController : MonoBehaviour
    {
        protected Weapon _weapon;
        protected float _shotTimer = 0;
        protected Camera _camera;
        protected EventService _eventService;
        public Entity player;

        void Awake()
        {
            _camera = Camera.main;
            _eventService = GameManager.EventService;
            _eventService.Add<OnHitEvent>(OnHit);
        }

        public void Setup(Weapon weapon)
        {
            _weapon = weapon;
        }

        protected float CalculateDamage()
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

        private void OnHit(OnHitEvent eventType)
        {
            DamageWorksheet damageWorksheet = new DamageWorksheet();
            damageWorksheet.source = player;
            damageWorksheet.target = eventType.Target;
            
            foreach (var effect in GameManager.SettingsManager.effectSettings.UnlockedEffects
                         .OfType<IExecuteEffect>()
                         .Where(e => e.TriggerType == EffectTriggerType.OnHit))
            {
                effect.Execute(damageWorksheet);
            }
        }
    }

    public class DamageWorksheet
    {
        public float baseDamage;
        public List<float> weaponMultipliers;

        public float effectDamage;
        public List<float> effectMultipliers;

        public int resistance;
        public float penetration;

        public Entity source;
        public Entity target;
        
        // Base damage * [weaponMult] + [effectDamage * effectMult]... - (armor * penetration)
        public void ApplyDamage(Entity target)
        {
            float baseDmg = baseDamage;

            foreach (var mult in weaponMultipliers)
            {
                baseDmg *= mult;
            }

            float effectDmg = effectDamage;

            float finalDamage = baseDmg + effectDmg;

            //target.TakeDamage(finalDamage);
        }
    }
}
