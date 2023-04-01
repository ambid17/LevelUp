using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;

namespace Minigames.Fight
{
    public class DamageTextController : MonoBehaviour
    {
        [SerializeField] private TMP_Text text;

        public void Setup(string damage, Vector3 position)
        {
            transform.position = position;
            text.text = damage;
            StartCoroutine(Jump());
        }

        private IEnumerator Jump()
        {
            Sequence sequence = transform.DOJump(transform.position, 0.5f, 1, 1);
            yield return sequence.WaitForCompletion();
            GameManager.DamageTextPool.Release(this);
        }
    }
}