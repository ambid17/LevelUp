using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Minigames.Fight
{
    public class ProgressModel
    {
        public List<WorldData> WorldData;
        public float Currency;
        public TutorialState TutorialState;
    }

    public class WorldData
    {
        public string WorldName;
        public bool IsCompleted;
        public bool IsUnlocked;
    }
}