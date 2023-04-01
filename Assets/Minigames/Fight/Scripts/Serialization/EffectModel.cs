using System;
using System.Collections;
using System.Collections.Generic;

namespace Minigames.Fight
{
    [Serializable]
    public class EffectModel
    {
        public Type Type;
        public string Name;
        public int AmountOwned;
        public bool IsUnlocked;
    }
}