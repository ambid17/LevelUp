using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Minigames.Fight
{
    public class PheromoneTrail : MonoBehaviour
    {
        [SerializeField]
        private AntBehaviorData behaviorData;
        [SerializeField]
        private float dropFrequency;
        [SerializeField]
        private GameObject prefab;

        private float localDropTime;

        private void Update()
        {
            if (!behaviorData.IsAlerted)
            {
                return;
            }
            if (localDropTime > 0)
            {
                localDropTime -= Time.deltaTime;
                return;
            }
            Instantiate(prefab, transform.position, transform.rotation);
            localDropTime = dropFrequency;
        }
    }
}