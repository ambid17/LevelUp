using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Minigames.Fight
{
    public class HiveMindManager : MonoBehaviour
    {
        public List<HiveMindBehaviorData> MyHiveMinds;

        [SerializeField]
        private float tick;

        private float _tickTimer;

        public HiveMindManager(List<HiveMindBehaviorData> hiveMinds)
        {
            MyHiveMinds = hiveMinds;
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

        }
    }
}