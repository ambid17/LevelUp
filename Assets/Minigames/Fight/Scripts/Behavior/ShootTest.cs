using UnityEngine;

public class ShootTest : MonoBehaviour
{
    
    public BulletTest bullet;

    private float shootTimer = 0;

    private void Update()
    {
        if (shootTimer > 0)
        {
            shootTimer -= Time.deltaTime;
        }
    }
    public void SpawnBullet(Quaternion rot)
    {
        Instantiate(bullet, transform.position, rot);
        shootTimer = 1;
    }
    public bool canShoot()
    {
        return shootTimer <= 0;
    }
}
