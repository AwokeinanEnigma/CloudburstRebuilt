using BepInEx.Configuration;
using Cloudburst.Builders;
using Cloudburst.Cores;
using R2API;
using RoR2;
using UnityEngine;

namespace Cloudburst.Items.Equipment
{
    public class Lumpkin : EquipmentBuilder
    {
        public override string EquipmentName => "The Lumpkin";

        public override string EquipmentLangTokenName => "LUMPKIN";

        public override string EquipmentPickupDesc => "And his screams were Brazilian...";

        public override string EquipmentFullDescription => "Release a Brazilian scream that does <style=cIsDamage>500% damage, and twice your maximum health for damage</style>.";

        public override string EquipmentLore => "\"Lumpkin, one of the many rebel commanders of WW19, possessed a scream that could deafen his oppenents. In many battles and skrimishes, he would often use this scream to ambush squads and slaughter them. He continued this practice until the final battle of WW19, when he had his left lung sforcibly ripped from his chest and eaten. \r\n\r\nHis lungs, pictured above, allowed him to scream loudly without injuring himself.\"\r\n\r\n-Exhibit at The National WW19 Museum";

        public override string EquipmentModelPath => "Assets/Cloudburst/Equipment/Lumpkin/IMDLLumpkin.prefab";

        public override string EquipmentIconPath => "Assets/Cloudburst/Equipment/Lumpkin/icon.png";

        public override bool IsLunar => false;

        protected override void CreateConfig(ConfigFile config)
        {

        }

        public override ItemDisplayRuleDict CreateItemDisplayRules()
        {
            var mdl = AssetLoader.mainAssetBundle.LoadAsset<GameObject>(EquipmentModelPath);
            ItemDisplayRuleDict rules = new ItemDisplayRuleDict();

            rules.Add("mdlCommandoDualies", new ItemDisplayRule[]
            {
    new ItemDisplayRule
    {
        ruleType = ItemDisplayRuleType.ParentedPrefab,
        followerPrefab = mdl,
childName = "Chest",
localPos = new Vector3(-0.1546F, 0.2269F, 0.3945F),
localAngles = new Vector3(2.1125F, 264.9913F, 92.1811F),
localScale = new Vector3(1F, 1F, 1F)
}
            });
            rules.Add("mdlHuntress", new ItemDisplayRule[]
            {
    new ItemDisplayRule
    {
        ruleType = ItemDisplayRuleType.ParentedPrefab,
        followerPrefab = mdl,
childName = "Chest",
localPos = new Vector3(-0.06059F, 0.24714F, 0.32719F),
localAngles = new Vector3(355.0741F, 264.7103F, 99.27936F),
localScale = new Vector3(1F, 1F, 1F)
}
            });
            rules.Add("mdlToolbot", new ItemDisplayRule[]
            {
    new ItemDisplayRule
    {
        ruleType = ItemDisplayRuleType.ParentedPrefab,
        followerPrefab = mdl,
childName = "Chest",
localPos = new Vector3(-0.4381F, 1.8153F, 4.3646F),
localAngles = new Vector3(3.4262F, 271.8089F, 104.9852F),
localScale = new Vector3(8F, 8F, 8F)
    }
            });

            rules.Add("mdlEngi", new ItemDisplayRule[]
            {
    new ItemDisplayRule
    {
        ruleType = ItemDisplayRuleType.ParentedPrefab,
        followerPrefab = mdl,
childName = "Chest",
localPos = new Vector3(0.03356F, 0.36964F, 0.37491F),
localAngles = new Vector3(323.1482F, 238.5304F, 117.516F),
localScale = new Vector3(1F, 1F, 1F)            }
            });
            rules.Add("mdlMage", new ItemDisplayRule[]
            {
    new ItemDisplayRule
    {
        ruleType = ItemDisplayRuleType.ParentedPrefab,
        followerPrefab = mdl,
childName = "Chest",
localPos = new Vector3(0.0531F, 0.0726F, 0.3056F),
localAngles = new Vector3(3.4891F, 290.111F, 61.6155F),
localScale = new Vector3(1F, 1F, 1F)           }
            });
            rules.Add("mdlMerc", new ItemDisplayRule[]
            {
    new ItemDisplayRule
    {
        ruleType = ItemDisplayRuleType.ParentedPrefab,
        followerPrefab = mdl,
childName = "Chest",
localPos = new Vector3(0.0439F, 0.2073F, 0.3381F),
localAngles = new Vector3(359.9271F, 289.9793F, 85.8721F),
localScale = new Vector3(1F, 1F, 1F)
    }
            });
            rules.Add("mdlLoader", new ItemDisplayRule[]
            {
    new ItemDisplayRule
    {
        ruleType = ItemDisplayRuleType.ParentedPrefab,
        followerPrefab = mdl,
childName = "MechHandR",
localPos = new Vector3(-0.03233F, 0.27857F, -0.02345F),
localAngles = new Vector3(298.3183F, 204.5738F, 95.84402F),
localScale = new Vector3(1F, 1F, 1F)
    }
            });
            rules.Add("mdlCroco", new ItemDisplayRule[]
            {
    new ItemDisplayRule
    {
        ruleType = ItemDisplayRuleType.ParentedPrefab,
        followerPrefab = mdl,
childName = "Head",
localPos = new Vector3(2.37611F, 3.28387F, -0.66733F),
localAngles = new Vector3(345.0883F, 96.62551F, 105.5187F),
localScale = new Vector3(10F, 10F, 10F)
    }
            }); 
            rules.Add("mdlTreebot", new ItemDisplayRule[]
 {
    new ItemDisplayRule
    {
        ruleType = ItemDisplayRuleType.ParentedPrefab,
        followerPrefab = mdl,
childName = "MuzzleSyringe",
localPos = new Vector3(0.14432F, -0.09815F, -1.26949F),
localAngles = new Vector3(61.6874F, 83.68061F, 89.07842F),
localScale = new Vector3(2F, 2F, 2F)
    }
 });
            rules.Add("mdlCaptain", new ItemDisplayRule[]
            {
    new ItemDisplayRule
        {
            ruleType = ItemDisplayRuleType.ParentedPrefab,
        followerPrefab = mdl  ,
childName = "Head",
localPos = new Vector3(0.3209F, 0.2037F, -0.2083F),
localAngles = new Vector3(331.4015F, 25.9899F, 115.6633F),
localScale = new Vector3(1F, 1F, 1F)
    }
            });
            rules.Add("mdlBandit2", new RoR2.ItemDisplayRule[]
{
                new RoR2.ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = mdl,
childName = "Chest",
localPos = new Vector3(-0.10012F, 0.14507F, 0.31825F),
localAngles = new Vector3(15.53031F, 63.34069F, 268.07F),
localScale = new Vector3(1F, 1F, 1F)
                }
});
            rules.Add("mdlScav", new RoR2.ItemDisplayRule[]
{
                new RoR2.ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = mdl,
childName = "Chest",
localPos = new Vector3(-2.7836F, 9.86149F, -2.97035F),
localAngles = new Vector3(337.1804F, 221.6179F, 169.3359F),
localScale = new Vector3(20F, 20F, 20F)
                }
});
            rules.Add("mdlVoidSurvivor", new ItemDisplayRule[]
{
    new ItemDisplayRule
        {
            ruleType = ItemDisplayRuleType.ParentedPrefab,
        followerPrefab = mdl  ,
childName = "Chest",
localPos = new Vector3(0.07061F, 0.36416F, -0.41635F),
localAngles = new Vector3(291.8598F, 335.0049F, 208.2181F),
localScale = new Vector3(1F, 1F, 1F)
    }
});
            rules.Add("mdlRailGunner", new ItemDisplayRule[]
  {
    new ItemDisplayRule
        {
            ruleType = ItemDisplayRuleType.ParentedPrefab,
        followerPrefab = mdl  ,
childName = "Head",
localPos = new Vector3(0.00195F, 0.09624F, 0.20534F),
localAngles = new Vector3(293.2141F, 244.3938F, 102.493F),
localScale = new Vector3(0.6F, 0.6F, 0.6F)
    }
  });

            return rules;
        }

        protected override void Initialization()
        {

        }

        private bool Scream(EquipmentSlot slot)
        {
            if (slot.characterBody)
            {
                CharacterBody body = slot.characterBody;
                BlastAttack impactAttack = new BlastAttack
                {
                    attacker = slot.gameObject,
                    attackerFiltering = AttackerFiltering.Default,
                    baseDamage = (5f * body.damage) + (body.maxHealth * 2f),
                    baseForce = 5000,
                    bonusForce = new Vector3(0, 5000, 0),
                    crit = false,
                    damageColorIndex = DamageColorIndex.CritHeal,
                    damageType = DamageType.AOE,
                    falloffModel = BlastAttack.FalloffModel.None,
                    inflictor = body.gameObject,
                    losType = BlastAttack.LoSType.NearestHit,
                    position = body.transform.position,
                    procChainMask = default,
                    procCoefficient = 2f,
                    radius = 15,
                    teamIndex = body.teamComponent.teamIndex
                };
                impactAttack.Fire();
                EffectData effect = new EffectData()
                {
                    origin = body.footPosition,
                    scale = 15
                };
                EffectManager.SpawnEffect(Effects.lumpkinEffect, effect, true);
                return true;
            }
            return false;
        }

        protected override bool ActivateEquipment(EquipmentSlot slot)
        {
            return Scream(slot);
        }
    }
}