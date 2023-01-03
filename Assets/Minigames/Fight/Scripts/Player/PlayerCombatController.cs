using UnityEngine;

namespace Minigames.Fight
{
    public class PlayerCombatController : MonoBehaviour
    {
        [SerializeField] private GameObject projectilePrefab;
        private float _projectileSpread = 0.15f;
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

            if (Input.GetMouseButton(0) && _shotTimer > GameManager.SettingsManager.weaponSettings.FireRate)
            {
                _shotTimer = 0;
                Shoot();
            }
        }
    
        private void Shoot()
        {
            int projectileCount = GameManager.SettingsManager.weaponSettings.ProjectileCount;
            for (int i = 0; i < projectileCount; i++)
            {
                GameObject projectileGO = Instantiate(projectilePrefab);
                Projectile projectile = projectileGO.GetComponent<Projectile>();
            
            
                Vector2 direction = _camera.ScreenToWorldPoint(Input.mousePosition) - transform.position;

                // Map the indices to start from the leftmost projectile and spawn them to the right using the offset
                float indexOffset = (float)i - i/2;
                Vector2 offset = Vector2.Perpendicular(direction).normalized * indexOffset * _projectileSpread;
            
                projectileGO.transform.position = transform.position.AsVector2() + offset;

                float damage = CalculateDamage();
            
                projectile.SetupForPlayer(damage, direction);
            }
        
        }

        private float CalculateDamage()
        {
            float damage = GameManager.SettingsManager.weaponSettings.Damage;

            if (GameManager.SettingsManager.weaponSettings.CritChance > 0)
            {
                float randomValue = Random.Range(0f, 1f);
                bool shouldCrit = randomValue < GameManager.SettingsManager.weaponSettings.CritChance;

                if (shouldCrit)
                {
                    damage *= GameManager.SettingsManager.weaponSettings.CritDamage;
                }
            }

            return damage;
        }
    }
}
