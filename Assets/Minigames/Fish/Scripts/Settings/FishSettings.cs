using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Minigames.Fish
{
    [CreateAssetMenu(fileName = "FishSettings", menuName = "ScriptableObjects/Fish/FishSettings", order = 1)]
    [Serializable]
    public class FishSettings : ScriptableObject
    {
        public List<Fish> Fish;
    }

    [Serializable]
    public class Fish
    {

    }
}