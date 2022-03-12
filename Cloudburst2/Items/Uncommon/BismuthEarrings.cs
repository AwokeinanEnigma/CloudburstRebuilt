using BepInEx.Configuration;
using Cloudburst.Builders;
using R2API;
using RoR2;
using UnityEngine;

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

        public override string ItemFullDescription => "Gain a <style=cIsHealing>temporary barrier</style> on inflicting bleed for <style=cIsHealing>" + BaseBarrier.Value + " health</style> <style=cStack>(+" + StackingBarrier.Value + " per stack)</style>. Also gain <style=cIsDamage>5% critical hit chance</style>.";

        public override string ItemLore => "";

        public override ItemTier Tier => ItemTier.Tier2;

        public override string ItemModelPath => "Assets/Cloudburst/Items/TopazLense/IMDLBismuthRings.prefab";

        public override string ItemIconPath => "Assets/Cloudburst/Items/TopazLense/icon.png";

        public ConfigEntry<float> BaseBarrier;
        public ConfigEntry<float> StackingBarrier;

        public override void CreateConfig(ConfigFile config)
        {
            BaseBarrier = config.Bind<float>(ConfigName, "Base Barrier", 0.1f, "How much barrier a single stack of this item gives you.");
            StackingBarrier = config.Bind<float>(ConfigName, "Stacking Barrier", 0.1f, "How much extra barrier stacks of this item gives you.");

        }

        public override ItemDisplayRuleDict CreateItemDisplayRules()
        {
            return new ItemDisplayRuleDict();
        }

        public override void Hooks()
        {
            On.RoR2.CharacterBody.RecalculateStats += CharacterBody_RecalculateStats;
            GlobalHooks.takeDamage += GlobalHooks_takeDamage;
        }

        private void GlobalHooks_takeDamage(ref DamageInfo info, GameObject victim, GlobalHooks.OnHitEnemy onHitInfo)
        {
            int count = GetCount(onHitInfo.attackerBody);
            if (count > 0 && onHitInfo.attackerBody.healthComponent && info.dotIndex == DotController.DotIndex.Bleed) {
                onHitInfo.attackerBody.healthComponent.AddBarrier(BaseBarrier.Value + (count * StackingBarrier.Value));
            }
        }

        private void CharacterBody_RecalculateStats(On.RoR2.CharacterBody.orig_RecalculateStats orig, CharacterBody self)
        {
            orig(self);
            if (GetCount(self) > 0)
            {
                self.bleedChance += 7;
            }
            }


        protected override void Initialization()
        {
        }
    }
}