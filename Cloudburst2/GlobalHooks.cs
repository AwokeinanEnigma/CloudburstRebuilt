using Cloudburst.Custodian.Components;
using RoR2;
using UnityEngine;

namespace Cloudburst
{
    /// <summary>
    /// This class contains all the global hooks you will need when coding items. You should use this class's events for item effects when possible.
    /// </summary>
    public class GlobalHooks
    {
        /// <summary>
        /// Struct for making dealing with the onHitEnemy event less of a hassle and variable clusterfuck
        /// </summary>
        public struct OnHitEnemy
        {
            public Inventory attackerInventory;
            public CharacterMaster attackerMaster;
            public CharacterBody attackerBody;
            public CharacterBody victimBody;
            public OnHitEnemy(GameObject victim, GameObject attacker)
            {
                victimBody = victim ? victim.GetComponent<CharacterBody>() : null;
                attackerBody = attacker ? attacker.GetComponent<CharacterBody>() : null;
                attackerMaster = attackerBody ? attackerBody.master : null;
                attackerInventory = attackerMaster ? attackerMaster.inventory : null;
            }
        }

        #region Ignore this shit. Trust me, it really doesn't matter.
        public delegate void DamageInfoCG(ref DamageInfo info, GameObject victim, OnHitEnemy onHitInfo);
        public delegate void CharacterBodyCG(CharacterBody body);
        public delegate void CharacterBodyAddTimedBuffCG(CharacterBody body, ref BuffDef type, ref float duration);
        public delegate void CritCG(CharacterBody attackerBody, CharacterMaster attackerMaster, float procCoeff, ProcChainMask procMask);
        public delegate void FinalBuffStackLostCG(CharacterBody body, BuffDef def);
        #endregion

        /// <summary>
        /// This is invoked when an entity (player or enemy or whatever) is hit. You can modify the damage info. Typically, on-hit effects are applied here.
        /// </summary>
        public static event DamageInfoCG onHitEnemy;
        /// <summary>
        /// This is invoked whenever something manages to score a critical hit. Usually, enemies cannot crit on players so you won't need a player check. 
        /// However if you're making an item with a powerful crit effect, it might be worth it to blacklist the item and add a player check.
        /// </summary>
        public static event CritCG onCrit;
        /// <summary>
        /// Like onHitEnemy, this is invoked whenever something takes damage. Item effects that trigger when an entity takes damage (i.e old war stealth kit) should subscribe to this event.
        /// </summary>
        public static event DamageInfoCG takeDamage;
        /// <summary>
        /// This is invoked whenever an entity loses an item or gains an item. Remember: Equipment counts too!
        /// </summary>
        public static event CharacterBodyCG onInventoryChanged;
        /// <summary>
        /// This is invoked when a timed buff is applied to an entity.
        /// </summary>
        public static event CharacterBodyAddTimedBuffCG onAddTimedBuff;
        /// <summary>
        /// Whenever something loses a buff, this will be invoked. 
        /// </summary>
        public static event FinalBuffStackLostCG onFinalBuffStackLost;

        public static void Init()
        {

            On.RoR2.HealthComponent.TakeDamage += HealthComponent_TakeDamage;
            On.RoR2.GlobalEventManager.OnHitEnemy += GlobalEventManager_OnHitEnemy;
            On.RoR2.CharacterBody.OnInventoryChanged += CharacterBody_OnInventoryChanged;
            On.RoR2.GlobalEventManager.OnCrit += GlobalEventManager_OnCrit1; ;
            On.RoR2.CharacterBody.AddTimedBuff_BuffDef_float += CharacterBody_AddTimedBuff_BuffDef_float;//AddTimedBuff += CharacterBody_AddTimedBuff;
            On.RoR2.CharacterBody.OnBuffFinalStackLost += CharacterBody_OnBuffFinalStackLost;
            On.RoR2.GlobalEventManager.OnCharacterHitGroundServer += GlobalEventManager_OnCharacterHitGroundServer;
        }

        #region Hooks
        private static void GlobalEventManager_OnCrit1(On.RoR2.GlobalEventManager.orig_OnCrit orig, GlobalEventManager self, CharacterBody body, DamageInfo damageInfo, CharacterMaster master, float procCoefficient, ProcChainMask procChainMask)
        {
            if (body && procCoefficient > 0f && body && master && master.inventory)
            {
                onCrit?.Invoke(body, master, procCoefficient, procChainMask);
            }
            orig(self, body, damageInfo, master, procCoefficient, procChainMask);
        }

        private static void GlobalEventManager_OnCharacterHitGroundServer(On.RoR2.GlobalEventManager.orig_OnCharacterHitGroundServer orig, GlobalEventManager self, CharacterBody characterBody, Vector3 impactVelocity)
        {
            if (!characterBody.HasComponent<SpikingComponent>())
            {
                orig(self, characterBody, impactVelocity);
            }
        }

        private static void CharacterBody_OnBuffFinalStackLost(On.RoR2.CharacterBody.orig_OnBuffFinalStackLost orig, CharacterBody self, BuffDef buffDef)
        {
            onFinalBuffStackLost?.Invoke(self, buffDef);
            orig(self, buffDef);

        }

        private static void CharacterBody_AddTimedBuff_BuffDef_float(On.RoR2.CharacterBody.orig_AddTimedBuff_BuffDef_float orig, CharacterBody self, BuffDef buffDef, float duration)
        {
            onAddTimedBuff?.Invoke(self, ref buffDef, ref duration);
            orig(self, buffDef, duration);
        }

        private static void CharacterBody_OnInventoryChanged(On.RoR2.CharacterBody.orig_OnInventoryChanged orig, CharacterBody self)
        {
            if (self)
            {
                onInventoryChanged?.Invoke(self);
            }
            orig(self);
        }

        private static void GlobalEventManager_OnHitEnemy(On.RoR2.GlobalEventManager.orig_OnHitEnemy orig, GlobalEventManager self, DamageInfo damageInfo, GameObject victim)
        {
            onHitEnemy?.Invoke(ref damageInfo, victim, new OnHitEnemy(victim, damageInfo.attacker));
            orig(self, damageInfo, victim);
        }

        public static void HealthComponent_TakeDamage(On.RoR2.HealthComponent.orig_TakeDamage orig, HealthComponent self, DamageInfo damageInfo)
        {
            takeDamage?.Invoke(ref damageInfo, self.gameObject, new OnHitEnemy(self.gameObject, damageInfo.attacker));
            orig(self, damageInfo);

        }
        #endregion
    }
}
