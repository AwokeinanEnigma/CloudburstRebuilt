using BepInEx.Configuration;
using Cloudburst.Builders;
using R2API;
using RoR2;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Cloudburst.Items.Uncommon
{
    public class BismuthEarrings : ItemBuilder
    {
        public override ItemTag[] ItemTags => new ItemTag[2] {
            ItemTag.Healing,
            ItemTag.AIBlacklist
        };

        public override string ItemName => "Bismuth Earrings";

        public override string ItemLangTokenName => "BARRIERONCRIT";

        public override string ItemPickupDesc => "Gain barrier on inflicting bleed";

        public override string ItemFullDescription => "Gain a <style=cIsHealing>temporary barrier</style> on inflicting bleed for <style=cIsHealing>" + BaseBarrier.Value + " health</style> <style=cStack>(+" + StackingBarrier.Value + " per stack)</style>. Also gain <style=cIsDamage>5% bleed chance</style>.";

        public override string ItemLore => "";

        public override ItemTierDef Tier => Addressables.LoadAssetAsync<ItemTierDef>("RoR2/Base/Common/Tier2Def.asset").WaitForCompletion();

        public override string ItemModelPath => "Assets/Cloudburst/Items/TopazLense/IMDLBismuthRings.prefab";

        public override string ItemIconPath => "Assets/Cloudburst/Items/TopazLense/icon.png";

        public ConfigEntry<float> BaseBarrier;
        public ConfigEntry<float> StackingBarrier;

        public override void CreateConfig(ConfigFile config)
        {
            BaseBarrier = config.Bind<float>(ConfigName, "Base Barrier", 5f, "How much barrier a single stack of this item gives you.");
            StackingBarrier = config.Bind<float>(ConfigName, "Stacking Barrier", 3f, "How much extra barrier stacks of this item gives you.");

        }

        public override ItemDisplayRuleDict CreateItemDisplayRules()
        {
            return new ItemDisplayRuleDict();
        }

        public override void Hooks()
        {
            On.RoR2.CharacterBody.RecalculateStats += CharacterBody_RecalculateStats;
            //GlobalHooks.takeDamage += GlobalHooks_takeDamage;
            On.RoR2.DotController.AddDot += DotController_AddDot;
        }

        private void DotController_AddDot(On.RoR2.DotController.orig_AddDot orig, DotController self, GameObject attackerObject, float duration, DotController.DotIndex dotIndex, float damageMultiplier, uint? maxStacksFromAttacker, float? totalDamage)
        {
            orig(self, attackerObject, duration, dotIndex, damageMultiplier, maxStacksFromAttacker, totalDamage);
            //get our body
            CharacterBody body = attackerObject.GetComponent<CharacterBody>();
            //get count, this method is safe even if the body doesn't exist, it'll just return 0
            int count = GetCount(body);
            //if the attacker has our item, and they have a health component (so we can apply barrier), and the dot theyre applying is bleed
            if (count > 0 && body.healthComponent && dotIndex == DotController.DotIndex.Bleed)
            {
                //base barrier + (count of items * stacking) - stacking
                //this is bad
                float barrierToApply = CCUtilities.GenericFlatStackingFloat(BaseBarrier.Value, count, StackingBarrier.Value);
                body.healthComponent.AddBarrier(barrierToApply);
            }
        }

        private void CharacterBody_RecalculateStats(On.RoR2.CharacterBody.orig_RecalculateStats orig, CharacterBody self)
        {
            orig(self);
            if (GetCount(self) > 0)
            {
                //add bleed chance
                self.bleedChance += 5;
            }
        }


        protected override void Initialization()
        {
        }
    }
}