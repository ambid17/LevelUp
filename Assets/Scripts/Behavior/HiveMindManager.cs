using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Minigames.Fight
{
    public class HiveMindManager : MonoBehaviour
    {
        public List<HiveMindBehaviorData> MyHiveMinds;
        public bool CanSeeTarget;

        public bool IsAlerted => CurrentHiveHP < _totalHiveMaxHealth;

        public float CurrentHiveHP => MyHiveMinds.Sum(h => h.MyEntity.Stats.combatStats.currentHp);
        public float AttackPriority => _totalHiveMaxHealth - CurrentHiveHP;
        public Vector2 PlayerLastKnown => _playerLastKnown;
        private float tick;
        [SerializeField]
        private float tickRandomizer;

        private float _tickTimer;
        private float _totalHiveMaxHealth;
        private Vector2 _playerLastKnown;

        public void Initialize(List<HiveMindBehaviorData> hiveMinds)
        {
            MyHiveMinds = hiveMinds;
            foreach (HiveMindBehaviorData hiveMind in MyHiveMinds)
            {
                hiveMind.MyManager = this;
                _totalHiveMaxHealth += hiveMind.MyEntity.Stats.combatStats.maxHp.Calculated;
            }
            tick += Random.Range(-tickRandomizer, tickRandomizer);
        }

        private void Update()
        {
            if (_tickTimer < tick)
            {
                _tickTimer += Time.deltaTime;
                return;
            }
            OnTick();
            _tickTimer = 0;
        }

        private void OnTick()
        {
            StartCoroutine(CheckLoS());
        }

        IEnumerator CheckLoS()
        {
            if (!IsAlerted)
            {
                yield break;
            }
            var hiveMindsByDistanceFromTarget = MyHiveMinds.OrderBy(h => (h.transform.position.AsVector2() - h.PlayerPosition).magnitude);
            foreach (HiveMindBehaviorData hiveMind in hiveMindsByDistanceFromTarget)
            {
                GameObject player = PhysicsUtils.HasLineOfSight(hiveMind.transform, GameManager.PlayerEntity.transform, hiveMind.MyEntity.enemyStats.DetectRange, 360, hiveMind.ObstacleLayerMask);
                if (player != null)
                {
                    _playerLastKnown = GameManager.PlayerEntity.transform.position;
                    CanSeeTarget = true;
                    yield break;
                }

                yield return new WaitForSeconds(0);
            }
            CanSeeTarget = false;
        }
    }
}