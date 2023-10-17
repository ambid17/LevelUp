using System;
using System.Collections;
using System.Collections.Generic;
using Minigames.Fight;
using UnityEngine;

namespace Minigames.Fight
{
    public enum IncomeUpgradeType
    {
        IdleGoldPerMin,
        IdleTime,
        IdleGoldPercent,
        KillsPerKill,
        SaveHighestGold,
        GoldPerKill,
        DeathTimer
    }

    [CreateAssetMenu(fileName = "IncomeSettings", menuName = "ScriptableObjects/Fight/IncomeSettings", order = 1)]
    [Serializable]
    public class IncomeSettings : ScriptableObject
    {
        public float baseGoldPerMin;
        public float goldPerMinScalar;

        public float baseIdleTimeInMinutes;
        public float idleTimeScalar;

        public float idleGoldPercentScalar;

        public float killsPerKillScalar;

        public float goldPerKillScalar;

        public float baseDeathTimer;
        public float deathTimerScalar;


        public float GoldPerMinute { get; private set; }

        private void SetGoldPerMinute(int upgradeLevel)
        {
            GoldPerMinute = baseGoldPerMin * Mathf.Pow(1 + goldPerMinScalar, upgradeLevel);
        }

        public float IdleTime { get; private set; }
        public float IdleTimeScale { get; private set; }

        private void SetIdleTime(int upgradeLevel)
        {
            IdleTimeScale = idleTimeScalar * upgradeLevel;
            IdleTime = baseIdleTimeInMinutes + IdleTimeScale;
        }

        public float IdleGoldRatio { get; private set; }

        private void SetIdleGoldPercent(int upgradeLevel)
        {
            IdleGoldRatio = idleGoldPercentScalar * upgradeLevel;
        }

        public float KillsPerKill { get; private set; }

        private void SetKillsPerKill(int upgradeLevel)
        {
            KillsPerKill = 1 + (killsPerKillScalar * upgradeLevel);
        }

        public bool SaveHighestGold { get; private set; }

        private void SetSaveHighestGold(int upgradeLevel)
        {
            SaveHighestGold = upgradeLevel > 0;
        }

        public float GoldPerKill { get; private set; }

        private void SetGoldPerKill(int upgradeLevel)
        {
            GoldPerKill = 1 + (goldPerKillScalar * upgradeLevel);
        }

        public float DeathTimer { get; private set; }

        private void SetDeathTimer(int upgradeLevel)
        {
            DeathTimer = baseDeathTimer * (1 - (deathTimerScalar * upgradeLevel));
        }

        public void Init()
        {
            SetGoldPerMinute(0);
            SetIdleTime(0);
            SetIdleGoldPercent(0);
            SetKillsPerKill(0);
            SetSaveHighestGold(0);
            SetGoldPerKill(0);
            SetDeathTimer(0);
        }
    }
}