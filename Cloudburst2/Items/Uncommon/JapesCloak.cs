
using Cloudburst.Builders;
using Cloudburst.Cores;
using R2API;
using RoR2;
using RoR2.Projectile;
using UnityEngine;

namespace Cloudburst.Items.Uncommon
{
    public class JapesCloak : ItemBuilder
    {
        public override ItemTag[] ItemTags => new ItemTag[] { ItemTag.AIBlacklist, ItemTag.Utility };

        public override string ItemName =>
            "Japes Cloak";

        public override string ItemLangTokenName =>
            "PRANKSANDJAPES";

        public override string ItemPickupDesc =>
            "Gain a buff that grants armor and healing on item pickup.";

        public override string ItemFullDescription => "Gain a buff that grants you <style=cIsUtility>+5 armor</style> and <style=cIsHealing>30% healing</style> when picking up an item. Maximum cap of 3 buffs <style=cStack>(+2 per stack)</style>.";

        public override string ItemLore => @"""Quartermaster’s log. 17 days after the crash.

It has been more than half a month since we’ve nearly burnt ourselves into smoldering black paste during our unwilling introduction to the atmosphere of this murder-planet. Due to the nature of our arrival, that being escape from a collapsing cargo ship that had been ripped out of warp travel, our supply of necessities has been dwindling from an already dangerously low base count. We have been forced to ration what little food and water we have. To keep in check of this, I have been elected as Quartermaster of our outpost. 
Our supplies are as follows:

-Enough food and water for around 1 week and a half.
-3 and a half cardboard boxes full of salvaged metal and circuitry from destroyed drones and sentries.
-4 bags of medical equipment 
-2 keychains, each with 13 rusted keys? (This must be an error, Juarez says it was brought in alongside the rest of the haul that those three blokes brought in 2 days ago, are they printing these or something?)
-6 boxes full of ammunition (potentially dwindling at an exponential rate)

Something isn’t lining up. The bulletin board we put up to track how much supplies everyone’s been taking isn’t covering all of it. While this may be just simple miscommunication on everyone’s part, the chance of somebody taking more than what they need is steadily increasing with each day. I don’t have any evidence to point towards this exactly, but it is a (very frightening) possibility. 


If it’s just one person taking it all, then at the rates that have been shown... 
They just might be more prepared than the rest of us.

End log.


Addendum: WHO. IN GOD ABOVE’S HOLY NAME. TOOK. MY. CIGARETTES??”""";

        public override ItemTier Tier => ItemTier.Tier2;

        public BuffDef japesCloak;

        public override string ItemModelPath => "Assets/Cloudburst/Items/Cloak/IMDLCloak.prefab";

        public override string ItemIconPath => "Assets/Cloudburst/Items/Cloak/JapeIcon.png";

        //public override string UnlockString => AchievementCore.GetUnlockableString("GrabOrDie");

        public override ItemDisplayRuleDict CreateItemDisplayRules()
        {
            return new ItemDisplayRuleDict();
        }

        protected override void Initialization()
        {

            this.japesCloak = new QuickBuffBuilder()
            {
                canStack = true,
                isDebuff = false,
                iconSprite = AssetLoader.mainAssetBundle.LoadAsset<Sprite>("JapesCloakBuff"),
                buffColor = new Color(1f, 0.7882353f, 0.05490196f)
            }.BuildBuff();

        }

        public override void Hooks()
        {
            RecalculateStatsAPI.GetStatCoefficients += RecalculateStatsAPI_GetStatCoefficients;
            On.RoR2.GenericPickupController.AttemptGrant += GenericPickupController_AttemptGrant; //GrantItem += GrantItem;
        }

        private void GenericPickupController_AttemptGrant(On.RoR2.GenericPickupController.orig_AttemptGrant orig, GenericPickupController self, CharacterBody body)
        {
            orig(self, body);
            int cloakOnInteractionCount = GetCount(body);
            if (self && body && cloakOnInteractionCount > 0)
            {
                if (body && body.GetBuffCount(japesCloak) <= 2 + cloakOnInteractionCount)
                {
                    body.AddBuff(japesCloak);
                }
                //body.AddTimedBuff(BuffCore.instance.japesCloak, 30);
            }
        }

        private void RecalculateStatsAPI_GetStatCoefficients(CharacterBody sender, RecalculateStatsAPI.StatHookEventArgs args)
        {

            if (sender && sender.HasBuff(japesCloak))
            {
                var buffCount = sender.GetBuffCount(japesCloak);

                args.armorAdd += (5 * buffCount);
                args.baseRegenAdd += (0.1f * buffCount);

            }
        }
    }
}