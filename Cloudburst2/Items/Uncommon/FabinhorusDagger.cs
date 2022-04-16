using BepInEx.Configuration;
using Cloudburst.Builders;
using Cloudburst.Cores;
using R2API;
using RoR2;
using UnityEngine;

namespace Cloudburst.Items.Uncommon
{

    public class FabinhorusDagger : ItemBuilder
    {
        public override ItemTag[] ItemTags => new ItemTag[1]
        {
          ItemTag.Damage
        };

        public BuffDef fabinhoru;

        public override string ItemName => "Fabinhorus Dagger";

        public override string ItemLangTokenName => "FABINDAGGER";

        public override string ItemPickupDesc => "Striking bleeding enemies reduces their armor. Also gain chance to bleed.";

        public override string ItemFullDescription => $"Striking enemies while they are bleeding <style=cIsDamage>reduces their armor by {BaseArmorReduction.Value} <style=cStack>(+{StackArmorReduction.Value} per stack)</style>. Also gain 5% chance to <style=cIsDamage>bleed</style> an enemy on hit   .";

        public override string ItemLore => "";

        public override ItemTier Tier => ItemTier.Tier2;

        public override string ItemModelPath => "Assets/Cloudburst/Items/FabinhoruDagger/IMDLDagger.prefab";

        public override string ItemIconPath => "Assets/Cloudburst/Items/FabinhoruDagger/icon.png";

        public static ConfigEntry<float> BaseArmorReduction;
        public static ConfigEntry<float> StackArmorReduction;

        public override void CreateConfig(ConfigFile config)
        {
            BaseArmorReduction = config.Bind<float>(ConfigName, "Base Armor Reduction", 30, "How much armor is reduced on the first stack");
            StackArmorReduction = config.Bind<float>(ConfigName, "Stacking Armor Reduction", 15, "How much extra armor to reduce per stack");
        }

        public override ItemDisplayRuleDict CreateItemDisplayRules()
        {
            var MDL = Load();
            //LogCore.LogI(fabDagMDL);
            //fabDagMDL..SetTexture("_NormalTex", texturehere);
            //material.SetFloat("_NormalStrength", normalstrengthhere);
            ItemDisplayRuleDict rules = new ItemDisplayRuleDict();//new ItemDisplayRule[]
            rules.Add("mdlCommandoDualies", new ItemDisplayRule[] {

    new ItemDisplayRule
    {
        ruleType = ItemDisplayRuleType.ParentedPrefab,
        followerPrefab = MDL,
childName = "Chest",
localPos = new Vector3(0.04441F, 0.16476F, 0.33355F),
localAngles = new Vector3(358.6258F, 184.9319F, 213.4237F),
localScale = new Vector3(1F, 1F, 1F)

}
            });
            rules.Add("mdlHuntress", new ItemDisplayRule[]
            {
    new ItemDisplayRule
    {
        ruleType = ItemDisplayRuleType.ParentedPrefab,
        followerPrefab = MDL,
childName = "Chest",
localPos = new Vector3(0F, 0.2F, 0.3F),
localAngles = new Vector3(14.83532F, 172.032F, 194.775F),
localScale = new Vector3(1F, 1F, 1F)
}
            }); 
            rules.Add("mdlToolbot", new ItemDisplayRule[]
            {
    new ItemDisplayRule
    {
        ruleType = ItemDisplayRuleType.ParentedPrefab,
        followerPrefab = MDL,
childName = "Head",
localPos = new Vector3(0.42043F, 4.11931F, -1.94511F),
localAngles = new Vector3(48.77712F, 16.26471F, 13.63895F),
localScale = new Vector3(10F, 10F, 10F)
    }
            });
            rules.Add("mdlEngi", new ItemDisplayRule[]
            {
    new ItemDisplayRule
    {
        ruleType = ItemDisplayRuleType.ParentedPrefab,
        followerPrefab = MDL,
childName = "Chest",
localPos = new Vector3(0.23695F, 0.38017F, 0.14407F),
localAngles = new Vector3(26.38515F, 179.2051F, 179.8132F),
localScale = new Vector3(1F, 1F, 1F)          }
            });
            rules.Add("mdlMage", new ItemDisplayRule[]
            {
    new ItemDisplayRule
    {
        ruleType = ItemDisplayRuleType.ParentedPrefab,
        followerPrefab = MDL,
childName = "Chest",
localPos = new Vector3(-0.11532F, 0.21924F, 0.2006F),
localAngles = new Vector3(2.75933F, 149.7382F, 185.504F),
localScale = new Vector3(1F, 1F, 1F)             }
            });
            rules.Add("mdlMerc", new ItemDisplayRule[]
            {
    new ItemDisplayRule
    {
        ruleType = ItemDisplayRuleType.ParentedPrefab,
        followerPrefab = MDL,
childName = "Chest",
localPos = new Vector3(-0.21217F, 0.196F, 0.17159F),
localAngles = new Vector3(359.0273F, 121.4077F, 178.183F),
localScale = new Vector3(1F, 1F, 1F)
    }
            });
            rules.Add("mdlLoader", new ItemDisplayRule[]
            {
    new ItemDisplayRule
    {
        ruleType = ItemDisplayRuleType.ParentedPrefab,
        followerPrefab = MDL,
childName = "Chest",
localPos = new Vector3(-0.01377F, 0.17163F, 0.25114F),
localAngles = new Vector3(359.5154F, 177.2092F, 153.7225F),
localScale = new Vector3(1F, 1F, 1F)
    }
            });
            rules.Add("mdlCroco", new ItemDisplayRule[]
            {
    new ItemDisplayRule
    {
        ruleType = ItemDisplayRuleType.ParentedPrefab,
        followerPrefab = MDL,
childName = "Head",
localPos = new Vector3(-0.61853F, 3.66017F, -0.13073F),
localAngles = new Vector3(345.0883F, 96.6255F, 105.5187F),
localScale = new Vector3(10F, 10F, 10F)
    }
            });
            rules.Add("mdlCaptain", new ItemDisplayRule[]
            {
    new ItemDisplayRule
        {
            ruleType = ItemDisplayRuleType.ParentedPrefab,
        followerPrefab = MDL  ,
childName = "Chest",
localPos = new Vector3(0.00241F, 0.20137F, 0.25038F),
localAngles = new Vector3(343.6854F, 182.3367F, 92.10435F),
localScale = new Vector3(1F, 1F, 1F)
    }
            });
            rules.Add("mdlTreebot", new ItemDisplayRule[]
 {
    new ItemDisplayRule
        {
            ruleType = ItemDisplayRuleType.ParentedPrefab,
        followerPrefab = MDL  ,
childName = "Chest",
localPos = new Vector3(0.00241F, 0.20137F, 0.25038F),
localAngles = new Vector3(343.6854F, 182.3367F, 92.10435F),
localScale = new Vector3(1F, 1F, 1F)
    }
 });
            rules.Add("mdlBandit2", new ItemDisplayRule[]
  {
    new ItemDisplayRule
        {
            ruleType = ItemDisplayRuleType.ParentedPrefab,
        followerPrefab = MDL  ,
childName = "MainWeapon",
localPos = new Vector3(-0.01747F, 1.11235F, -0.01368F),
localAngles = new Vector3(279.0793F, 356.7184F, 284.2909F),
localScale = new Vector3(1F, 1F, 1F)
    }
  });
            rules.Add("mdlVoidSurvivor", new ItemDisplayRule[]
  {
    new ItemDisplayRule
        {
            ruleType = ItemDisplayRuleType.ParentedPrefab,
        followerPrefab = MDL  ,
childName = "Chest",
localPos = new Vector3(0.08702F, 0.09177F, -0.26754F),
localAngles = new Vector3(335.3578F, 91.90347F, 181.3902F),
localScale = new Vector3(0.50328F, 0.50328F, 0.50328F)
    }
  });
            rules.Add("mdlRailGunner", new ItemDisplayRule[]
  {
    new ItemDisplayRule
        {
            ruleType = ItemDisplayRuleType.ParentedPrefab,
        followerPrefab = MDL  ,
childName = "Chest",
localPos = new Vector3(-0.00209F, 0.13727F, 0.39517F),
localAngles = new Vector3(276.5795F, 202.7492F, 245.6863F),
localScale = new Vector3(0.15697F, 0.13228F, 0.2F)
    }
  });
            return rules;
        }

        protected override void Initialization()
        {
            AddBuffs();
        }

        public void AddBuffs()
        {
            this.fabinhoru = new QuickBuffBuilder()
            {
                canStack = false,
                isDebuff = true,
                iconSprite = AssetLoader.mainAssetBundle.LoadAsset<Sprite>("RoR2BuffTemplateUpdateDark"),
                // buffColor = CloudUtils.HexToColor("#590005"),
            }.BuildBuff();
        }

        public override void Hooks()
        {
            R2API.RecalculateStatsAPI.GetStatCoefficients += RecalculateStatsAPI_GetStatCoefficients;
            GlobalHooks.onHitEnemy += GlobalHooks_onHitEnemy;
        }

        private void RecalculateStatsAPI_GetStatCoefficients(CharacterBody sender, RecalculateStatsAPI.StatHookEventArgs args)
        {
            if (sender.HasBuff(fabinhoru))
            {
                int count = Util.GetItemCountForTeam(TeamIndex.Player, Index.itemIndex, false);

                args.armorAdd -= BaseArmorReduction.Value + (count * StackArmorReduction.Value);

            }
        }

        private void GlobalHooks_onHitEnemy(ref DamageInfo damageInfo, GameObject victim, GlobalHooks.OnHitEnemy onHitInfo)
        {
            if (onHitInfo.attackerInventory && onHitInfo.attackerBody)
            {
                int itemCount = GetCount(onHitInfo.attackerBody);
             
                if (!damageInfo.procChainMask.HasProc(ProcType.BleedOnHit))
                {
                    bool alreadyBleed = (damageInfo.damageType & DamageType.BleedOnHit) > DamageType.Generic;
                    if ((itemCount > 0 || alreadyBleed) && (alreadyBleed || Util.CheckRoll(5 * damageInfo.procCoefficient, onHitInfo.attackerMaster)))
                    {
                        ProcChainMask procChainMask2 = damageInfo.procChainMask;
                        procChainMask2.AddProc(ProcType.BleedOnHit);
                        DotController.InflictDot(victim, damageInfo.attacker, DotController.DotIndex.Bleed, 3f * damageInfo.procCoefficient, 1f);
                    }
                }

                DotController dControl = DotController.FindDotController(victim);
                if (dControl && dControl.HasDotActive(DotController.DotIndex.Bleed) && itemCount > 0)
                {
                    onHitInfo.victimBody.AddTimedBuff(fabinhoru, 3);
                }

            }

        }
    }
}