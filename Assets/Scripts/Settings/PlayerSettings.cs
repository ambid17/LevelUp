using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Minigames.Fight
{

    [CreateAssetMenu(fileName = "PlayerSettings", menuName = "ScriptableObjects/Fight/PlayerSettings", order = 1)]
    [Serializable]
    public class PlayerSettings : ScriptableObject
    {
        public float baseMoveSpeed;
        public float moveSpeedScalar;
        public float baseAcceleration;
        public float accelerationScalar;
        public float baseMaxHp;
        public float hpScalar;
        public float lifeStealScalar;


        public float MoveSpeed { get; private set; }
        public float MoveSpeedScale { get; private set; }

        public void SetMoveSpeed(int upgradeLevel)
        {
            MoveSpeedScale = moveSpeedScalar * upgradeLevel;
            MoveSpeed = baseMoveSpeed * (1 + MoveSpeedScale);
        }
    
        public float Acceleration { get; private set; }
        public float AccelerationScale { get; private set; }
        public void SetAcceleration(int upgradeLevel)
        {
            AccelerationScale = accelerationScalar * upgradeLevel;
            Acceleration = baseAcceleration * (1 + AccelerationScale);
        }
    
        public float MaxHp { get; private set; }
        public float MaxHpScale { get; private set; }
        public void SetMaxHp(int upgradeLevel)
        {
            MaxHpScale = hpScalar * upgradeLevel;
            MaxHp = baseMaxHp * (1 + MaxHpScale);
        }
        
        public float LifeSteal { get; private set; }
        public float LifeStealScale { get; private set; }

        public void SetLifeSteal(int upgradeLevel)
        {
            LifeStealScale = lifeStealScalar * upgradeLevel;
            LifeSteal = LifeStealScale;
        }
        
        public void Init()
        {
            SetMoveSpeed(0);
            SetAcceleration(0);
            SetMaxHp(0);
            SetLifeSteal(0);
        }
    }
}