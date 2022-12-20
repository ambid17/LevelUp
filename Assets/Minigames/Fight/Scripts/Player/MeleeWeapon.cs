using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeWeapon : Weapon
{
    [SerializeField] private LayerMask _layerMask;
    [SerializeField] private Vector2 _size;
    
    protected override void Shoot()
    {
        PlayAnimation();
        Collider2D[] overlaps = Physics2D.OverlapBoxAll(transform.position, _size, 0, _layerMask);

        foreach (Collider2D overlap in overlaps)
        {
            EnemyController enemy = overlap.gameObject.GetComponent<EnemyController>();

            if (enemy != null)
            {
                enemy.TakeDamage(setting.Damage);
            }
        }
    }

    private void PlayAnimation()
    {
        
    }
}
