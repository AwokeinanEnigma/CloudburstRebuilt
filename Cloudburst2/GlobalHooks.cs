using Cloudburst.Custodian.Components;
using RoR2;
using UnityEngine;

namespace Cloudburst
{
    class GlobalHooks
    {
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

        public delegate void DamageInfoCloudGate(ref DamageInfo info, GameObject victim, OnHitEnemy onHitInfo);
        public delegate void CharacterBodyCloudGate(CharacterBody body);
        public delegate void CharacterBodyAddTimedBuffCloudGate(CharacterBody body, ref BuffDef type, ref float duration);
        public delegate void CritCloudGate(CharacterBody attackerBody, CharacterMaster attackerMaster, float procCoeff, ProcChainMask procMask);
        public delegate void FinalBuffStackLostCloudGate(CharacterBody body, BuffDef def);


        public static event DamageInfoCloudGate onHitEnemy;
        public static event CritCloudGate onCrit;
        public static event DamageInfoCloudGate takeDamage;
        public static event CharacterBodyCloudGate onInventoryChanged;
        public static event CharacterBodyAddTimedBuffCloudGate onAddTimedBuff;
        public static event FinalBuffStackLostCloudGate onFinalBuffStackLost;

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

        public static void CharacterSpawnCard_Awake(On.RoR2.CharacterSpawnCard.orig_Awake orig, CharacterSpawnCard self)
        {
            self.loadout = new SerializableLoadout();
            orig(self);
        }

        public static void HealthComponent_TakeDamage(On.RoR2.HealthComponent.orig_TakeDamage orig, HealthComponent self, DamageInfo damageInfo)
        {
            takeDamage?.Invoke(ref damageInfo, self.gameObject, new OnHitEnemy(self.gameObject, damageInfo.attacker));
            orig(self, damageInfo);

        }
    }
}
