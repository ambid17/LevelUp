using UnityEngine;

namespace Minigames.Fight
{
    public class WeaponController : MonoBehaviour
    {
        protected Weapon _weapon;
        protected float _shotTimer = 0;
        protected Camera _camera;

        void Awake()
        {
            _camera = Camera.main;
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
    }
}
