using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Minigames.Fight
{
    public class AntPheramoneTriggerBehavior : MonoBehaviour
    {
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.layer == PhysicsUtils.EnemyLayer)
            {
                AntBehaviorData ant = collision.GetComponent<AntBehaviorData>();
                if (ant != null)
                {
                    ant.IsAlerted = true;
                }
            }
        }
    }
}


