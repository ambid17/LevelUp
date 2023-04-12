using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks
{
    [TaskDescription("Determine location to shoot.")]
    public class Shoot : Action
    {
        [Tooltip("Target we want to shoot at.")]
        public SharedTransform target;
        [Tooltip("Speed of the Projectile")]
        public SharedFloat projectilSpeed;
        [Tooltip("The velocity the target is moving")]
        public SharedVector2 targetVelocity;
        [Tooltip("Value for random offset to prevent AI from being too accurate, scaled with distance")]
        public SharedFloat randomOffset;

        private ShootTest shootTest;

        private Vector2 prediction;
        public override void OnAwake()
        {
            base.OnAwake();
            shootTest = gameObject.GetComponent<ShootTest>();
            projectilSpeed = shootTest.bullet.bulletSpeed;

            if (shootTest.canShoot())
            {
                shootTest.SpawnBullet(Target());
            }
        }
        public override TaskStatus OnUpdate()
        {
            if (shootTest.canShoot())
            {
                shootTest.SpawnBullet(Target());
            }
            return TaskStatus.Running;
        }
        private Quaternion Target()
        {
            Vector2 relativePosition = transform.position - target.Value.position;
            float theta = Vector2.Angle(relativePosition, targetVelocity.Value);

            float a = (targetVelocity.Value.magnitude * targetVelocity.Value.magnitude) - (projectilSpeed.Value * projectilSpeed.Value);
            float b = -2 * Mathf.Cos(theta * Mathf.Deg2Rad) * relativePosition.magnitude * targetVelocity.Value.magnitude;
            float c = relativePosition.magnitude * relativePosition.magnitude;
            float delta = Mathf.Sqrt((b * b) - (4 * a * c));
            float t = -(b + delta) / (2 * a);

            prediction = (Vector2)target.Value.position + (targetVelocity.Value * t);
            Vector2 difference = RandomOffset() - (Vector2)transform.position;
            float angleToShoot = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
            Quaternion rot = Quaternion.AngleAxis(angleToShoot - 90, Vector3.forward);
            return rot;
        }
        private Vector2 RandomOffset()
        {
            float distance = (prediction - (Vector2)transform.position).magnitude;
            float randomFactor = distance * randomOffset.Value;

            float x = Random.Range(prediction.x - randomFactor, prediction.x + randomFactor);
            float y = Random.Range(prediction.y - randomFactor, prediction.y + randomFactor);

            return new Vector2(x, y);
        }

    }
}