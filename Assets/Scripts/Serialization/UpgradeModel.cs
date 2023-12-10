using System;
using System.Collections;
using System.Collections.Generic;

namespace Minigames.Fight
{
    [Serializable]
    public class UpgradeModel
    {
        public Type Type;
        public string Name;
        public int AmountOwned;
        public bool IsUnlocked;
        public bool IsCrafted;
    }
}