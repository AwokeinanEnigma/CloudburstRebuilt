using BepInEx.Configuration;
using Cloudburst.Builders;
using Cloudburst.Content;
using R2API;
using RoR2;
using System.Linq;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Cloudburst.Items.Common
{
    public class GlassHarvester : ItemBuilder
    {
        public override ItemTag[] ItemTags => new ItemTag[2]
{
        ItemTag.Utility,
        ItemTag.AIBlacklist
};
        public override string ItemName => "Glass Harvester";

        public override string ItemLangTokenName => "EXPERIENCEONHIT";

        public override string ItemPickupDesc => "Gain experience on hit";

        public override string ItemFullDescription => "Gain 3 <style=cStack>(+2 per stack)</style> points of <style=cIsUtility>experience</style> on hit.";

        public override string ItemLore => "";

        public override ItemTierDef Tier => Addressables.LoadAssetAsync<ItemTierDef>("RoR2/Base/Common/Tier1Def.asset").WaitForCompletion(); //Addressables.LoadAssetAsync<ItemTierDef>("RoR2/Base/Common/Tier1Def.asset").WaitForCompletion();

        public override string ItemModelPath => "Assets/Cloudburst/Items/Harvester/IMDLHarvester.prefab";

        public override string ItemIconPath => "Assets/Cloudburst/Items/Harvester/icon.png";

        public ConfigEntry<float> BaseExp;
        public ConfigEntry<float> StackingExp;

        public override void CreateConfig(ConfigFile config)
        {
            BaseExp = config.Bind<float>(ConfigName, "Base Experience", 3, "How much experience you get from a single stack of this item.");
            StackingExp = config.Bind<float>(ConfigName, "Stacking Experience", 2, "How much extra experience you get from extra stacks of this item.");
        }

        public override ItemDisplayRuleDict CreateItemDisplayRules()
        {
            var MDL = Load();
            ItemDisplayRuleDict rules = new ItemDisplayRuleDict();
            rules.Add("mdlCommandoDualies", new ItemDisplayRule[] {

    new ItemDisplayRule
    {
        ruleType = ItemDisplayRuleType.ParentedPrefab,
        followerPrefab = MDL,
childName = "Chest",
localPos = new Vector3(-0.00633F, 0.22777F, -0.27159F),
localAngles = new Vector3(354.5038F, 177.6712F, 20.34656F),
localScale = new Vector3(0.2F, 0.2F, 0.2F)

}
            });
            rules.Add("mdlHuntress", new ItemDisplayRule[]
            {
    new ItemDisplayRule
    {
        ruleType = ItemDisplayRuleType.ParentedPrefab,
        followerPrefab = MDL,
childName = "Chest",
localPos = new Vector3(0.13271F, 0.09739F, -0.13374F),
localAngles = new Vector3(19.56017F, 104.8181F, 11.13838F),
localScale = new Vector3(0.2F, 0.2F, 0.2F)
}
            });
            rules.Add("mdlToolbot", new ItemDisplayRule[]
            {
    new ItemDisplayRule
    {
        ruleType = ItemDisplayRuleType.ParentedPrefab,
        followerPrefab = MDL,
childName = "Head",
localPos = new Vector3(0.40617F, -3.44712F, 0.05289F),
localAngles = new Vector3(38.16683F, 41.05458F, 32.07256F),
localScale = new Vector3(2F, 2F, 2F)
    }
            });
            rules.Add("mdlEngi", new ItemDisplayRule[]
            {
    new ItemDisplayRule
    {
        ruleType = ItemDisplayRuleType.ParentedPrefab,
        followerPrefab = MDL,
childName = "Chest",
localPos = new Vector3(0.02773F, 0.00265F, -0.34495F),
localAngles = new Vector3(355.9258F, 184.9293F, 347.1985F),
localScale = new Vector3(0.2F, 0.2F, 0.2F)    }
            });
            rules.Add("mdlMage", new ItemDisplayRule[]
            {
    new ItemDisplayRule
    {
        ruleType = ItemDisplayRuleType.ParentedPrefab,
        followerPrefab = MDL,
childName = "Chest",
localPos = new Vector3(0.01584F, 0.11965F, -0.301F),
localAngles = new Vector3(1.20841F, 186.0194F, 357.1201F),
localScale = new Vector3(0.2F, 0.2F, 0.2F)         }
            });
            rules.Add("mdlMerc", new ItemDisplayRule[]
            {
    new ItemDisplayRule
    {
        ruleType = ItemDisplayRuleType.ParentedPrefab,
        followerPrefab = MDL,
childName = "HandR",
localPos = new Vector3(0.21108F, 0.20503F, 0.07222F),
localAngles = new Vector3(359.67F, 344.1947F, 271.8053F),
localScale = new Vector3(0.2F, 0.2F, 0.2F)
    }
            });
            rules.Add("mdlLoader", new ItemDisplayRule[]
            {
    new ItemDisplayRule
    {
        ruleType = ItemDisplayRuleType.ParentedPrefab,
        followerPrefab = MDL,
childName = "Chest",
localPos = new Vector3(0.01954F, 0.00341F, -0.40092F),
localAngles = new Vector3(1.68627F, 176.3239F, 0.92637F),
localScale = new Vector3(0.3F, 0.3F, 0.3F)
    }
            });
            rules.Add("mdlCroco", new ItemDisplayRule[]
            {
    new ItemDisplayRule
    {
        ruleType = ItemDisplayRuleType.ParentedPrefab,
        followerPrefab = MDL,
childName = "Chest",
localPos = new Vector3(-0.18765F, -0.06835F, 4.49741F),
localAngles = new Vector3(30.0772F, 354.7448F, 359.865F),
localScale = new Vector3(1.5F, 1.5F, 1.5F)
    }
            });
            rules.Add("mdlCaptain", new ItemDisplayRule[]
            {
    new ItemDisplayRule
        {
            ruleType = ItemDisplayRuleType.ParentedPrefab,
        followerPrefab = MDL  ,
childName = "Chest",
localPos = new Vector3(-0.00375F, 0.22505F, -0.24956F),
localAngles = new Vector3(353.9485F, 181.0955F, 353.3455F),
localScale = new Vector3(0.2F, 0.2F, 0.2F)
    }
            }); rules.Add("mdlTreebot", new ItemDisplayRule[]
 {
    new ItemDisplayRule
        {
            ruleType = ItemDisplayRuleType.ParentedPrefab,
        followerPrefab = MDL  ,
childName = "PlatformBase",
localPos = new Vector3(-0.49321F, 1.66212F, 0.44496F),
localAngles = new Vector3(344.3408F, 208.0533F, 104.2866F),
localScale = new Vector3(0.3F, 0.3F, 0.3F)
    }
 });
            rules.Add("mdlBandit2", new ItemDisplayRule[]
  {
    new ItemDisplayRule
        {
            ruleType = ItemDisplayRuleType.ParentedPrefab,
        followerPrefab = MDL  ,
childName = "Chest",
localPos = new Vector3(0.01257F, 0.05106F, -0.20291F),
localAngles = new Vector3(5.3335F, 155.0186F, 346.1779F),
localScale = new Vector3(0.2F, 0.2F, 0.2F)

    }
  });
            rules.Add("mdlVoidSurvivor", new ItemDisplayRule[]
{
    new ItemDisplayRule
        {
            ruleType = ItemDisplayRuleType.ParentedPrefab,
        followerPrefab = MDL  ,
childName = "Hand",
localPos = new Vector3(0.02317F, 0.12434F, 0.26187F),
localAngles = new Vector3(23.85456F, 286.3382F, 260.7058F),
localScale = new Vector3(0.2F, 0.2F, 0.2F)
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
        }

        public override void Hooks()
        {
            GlobalHooks.onHitEnemy += GlobalHooks_onHitEnemy;
        }

        private void GlobalHooks_onHitEnemy(ref DamageInfo info, UnityEngine.GameObject victim, GlobalHooks.OnHitEnemy onHitInfo)
        {
            int count = GetCount(onHitInfo.attackerBody);
            if (count > 0)
            {
                float exp = CCUtilities.GenericFlatStackingFloat(BaseExp.Value, count, StackingExp.Value);

                TeamManager.instance.GiveTeamExperience(TeamComponent.GetObjectTeam(onHitInfo.attackerBody.gameObject), (uint)(exp * Run.instance.difficultyCoefficient));
            }
        }
    }
}