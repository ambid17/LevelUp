using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Minigames.Fight;
using UnityEditor;
using UnityEngine;
using Utils;

namespace Minigames.Fight
{
    public class Entity : MonoBehaviour
    {
        public bool Stunned { get; set; }
        public bool IsControlled;
        public VisualController VisualController;
        public AnimationManager AnimationController;
        public WeaponController WeaponController;
        public EntityStats Stats;
        public Rigidbody2D Rigidbody2D;

        // Used to load entity stats from appData file
        public string statsFileName; // i.e. "Player", "Bee", etc


        public bool IsDead => Stats.combatStats.currentHp <= 0;

        protected EventService eventService;

        [SerializeField]
        private bool tickDamageDownPerSecond;

        protected virtual void Awake()
        {
            eventService = Platform.EventService;
            VisualController = GetComponent<VisualController>();
            AnimationController = GetComponent<AnimationManager>();
            Rigidbody2D = GetComponent<Rigidbody2D>();
            LoadStats();
            Setup();

            Platform.EventService.Add<EntityStatsFileRemappedEvent>(OnStatsRemapped);
        }

        private void OnStatsRemapped(EntityStatsFileRemappedEvent e)
        {
            if (e.StatsFileName == statsFileName)
            {
                LoadStats();
            }
        }

        public void LoadStats()
        {
            var savedStats = FightDataLoader.EntityStatsMap[statsFileName];
            Stats.Load(savedStats);
            Stats.Init();
        }

        protected virtual void Setup()
        {

        }

        protected virtual void Update()
        {
            if(IsDead) return;
            Stats.TickStatuses();
        }

        public virtual void TakeHit(float damage, Entity hitter)
        {
            if (IsDead)
            {
                return;
            }
            Stats.combatStats.TakeDamage(damage);

            if (IsDead)
            {
                Die(hitter);
                return;
            }

            VisualController.StartDamageFx(damage);
        }

        public virtual void TakeHeal(float amount, Entity source)
        {
            if (IsDead)
            {
                return;
            }
            Stats.combatStats.AddHp(amount);
        }

        public void DealDamage(Entity target, WeaponStats weaponStats)
        {
            // Execute all of the onHit effects to populate the onHitDamage
            foreach (var effect in weaponStats.OnHitEffects)
            {
                effect.Execute(this, target);
            }

            float damage = WeaponController.CurrentWeapon.baseDamage.Calculated
                + WeaponController.CurrentWeapon.onHitDamage.Calculated;
            target.TakeHit(damage, this);

            // Clear the onHitDamage because it is only used once per hit as many of the effects
            // are dependent on target stats
            WeaponController.CurrentWeapon.onHitDamage.Clear();
        }

        protected virtual void OnKill(Entity killedEntity)
        {
            foreach (var effect in WeaponController.CurrentWeapon.OnKillEffects)
            {
                effect.Execute(this, killedEntity);
            }
        }

        protected virtual void Die(Entity killer)
        {
            killer.OnKill(this);
        }

#if UNITY_EDITOR
        [ContextMenu("Setup")]
        public void SetupInspector()
        {
            VisualController = GetComponent<VisualController>();
            AnimationController = GetComponent<AnimationManager>();
            WeaponController = GetComponent<WeaponController>();
            Rigidbody2D = GetComponent<Rigidbody2D>();

            statsFileName = gameObject.name;

            EditorUtility.SetDirty(this);
        }
        [ContextMenu("Generate Collider Points")]
        public void GenerateColliderPoints()
        {
            Stats.GenerateColliderPoints();
        }
    }
#endif
}