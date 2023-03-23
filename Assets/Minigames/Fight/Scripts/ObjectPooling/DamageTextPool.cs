using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Pool;

namespace Minigames.Fight
{
    public class DamageTextPool : ObjectPoolBase<DamageTextController>
    {
        [SerializeField] private DamageTextController DamageTextPrefab;
        [SerializeField] private int defaultSize = 20;
        [SerializeField] private int maxSize = 50;

        private void Start()
        {
            InitPool(DamageTextPrefab, defaultSize, maxSize);
        }
    }
}