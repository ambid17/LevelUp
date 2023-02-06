using UnityEngine;

namespace Minigames.Fight
{
    public class WeaponController : MonoBehaviour
    {
        [SerializeField] private ProjectileWeapon _weapon;
        private float _shotTimer = 0;
        private Camera _camera;

        void Awake()
        {
            _camera = Camera.main;
        }

        void Update()
        {
            if (GameManager.GameStateManager.IsDead)
            {
                return;
            }
        
            _shotTimer += Time.deltaTime;

            if (Input.GetMouseButton(0) && _shotTimer > _weapon.Stats.FireRate)
            {
                _shotTimer = 0;
                Shoot();
            }
        }
    
        private void Shoot()
        {
            int projectileCount = _weapon.Stats.ProjectileCount;
            for (int i = 0; i < projectileCount; i++)
            {
                PlayerProjectile projectile = Instantiate(_weapon.ProjectilePrefab);
            
                Vector2 direction = _camera.ScreenToWorldPoint(Input.mousePosition) - transform.position;

                // Map the indices to start from the leftmost projectile and spawn them to the right using the offset
                float indexOffset = (float)i - i/2;
                Vector2 offset = Vector2.Perpendicular(direction).normalized * indexOffset * _weapon.ProjectileSpread;
            
                projectile.transform.position = transform.position.AsVector2() + offset;

                float damage = CalculateDamage();
            
                projectile.Setup(damage, direction, _weapon.Stats.ProjectilePenetration);
            }
        
        }

        private float CalculateDamage()
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
