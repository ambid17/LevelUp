using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Minigames.Fight
{
    public class EntityBehaviorData : MonoBehaviour
    {
        [SerializeField]
        private EnemyEntity myEntity;
        [SerializeField]
        private float tick;
        [SerializeField]
        private int tickRandomizer;

        public Transform playerTransform => myEntity.Target;
        public Vector2 playerPosition => myEntity.Target.position;


        private float tickTimer;

        private void Start()
        {
            tick += Random.Range(-tickRandomizer, tickRandomizer);
        }

        private void Update()
        {
            if (tickTimer < tick)
            {
                tickTimer += Time.deltaTime;
                return;
            }
            OnTick();
            tickTimer = 0;
        }

        public virtual void OnTick()
        {

        }
    }
}