using BepInEx.Configuration;
using Cloudburst.Builders;
using Cloudburst.Content;
using R2API;
using RoR2;
using RoR2.Projectile;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Cloudburst.Items.Common
{
    public class BlastBoot : ItemBuilder
    {
        public override string ItemName => "Blast Boot";

        public override string ItemLangTokenName => "BLASTBOOT";

        public override string ItemPickupDesc => "Firework-powered double jump upon Secondary Skill activation.";

        public override string ItemFullDescription => "Activating your Secondary skill also blasts you through the air with a flurry of 4 <style=cStack>(+1)</style> <style=cIsDamage>fireworks that deal 100% <style=cStack>(+50%)</style> damage</style>. This effect has a cooldown of 3 seconds.";

        public override string ItemLore => "";

        public override ItemTag[] ItemTags => new ItemTag[3]
{
    ItemTag.AIBlacklist,
    ItemTag.Utility,
    ItemTag.Damage
    };

        //public override ItemTierDef Tier => Addressables.LoadAssetAsync<ItemTierDef>("RoR2/Base/Common/Tier1Def.asset").WaitForCompletion();

        public override ItemTierDef Tier => Addressables.LoadAssetAsync<ItemTierDef>("RoR2/Base/Common/Tier1Def.asset").WaitForCompletion();

        public override string ItemModelPath => "Assets/Cloudburst/Items/CarePackageRequester/IMDLBlastBoot.prefab";

        public override string ItemIconPath => "Assets/Cloudburst/Items/CarePackageRequester/icon.png";

        public static GameObject fireworkPrefab;

        public override void CreateConfig(ConfigFile config)
        {

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
childName = "CalfL",
localPos = new Vector3(0.02731F, 0.28558F, -0.01864F),
localAngles = new Vector3(7.92373F, 187.8987F, 185.4126F),
localScale = new Vector3(0.3F, 0.3F, 0.3F)

}
            });
            rules.Add("mdlHuntress", new ItemDisplayRule[]
            {
    new ItemDisplayRule
    {
        ruleType = ItemDisplayRuleType.ParentedPrefab,
        followerPrefab = MDL,
childName = "FootL",
localPos = new Vector3(0.01535F, 0.00952F, -0.11311F),
localAngles = new Vector3(290.3859F, 87.12296F, 267.2033F),
localScale = new Vector3(0.2F, 0.2F, 0.2F)
}
            });
            rules.Add("mdlToolbot", new ItemDisplayRule[]
            {
    new ItemDisplayRule
    {
        ruleType = ItemDisplayRuleType.ParentedPrefab,
        followerPrefab = MDL,
childName = "Base",
localPos = new Vector3(-0.00199F, 3.38665F, -2.82651F),
localAngles = new Vector3(350.9689F, 354.3693F, 177.4269F),
localScale = new Vector3(1.4F, 1.4F, 1.4F)
    }
            });

            rules.Add("mdlEngi", new ItemDisplayRule[]
            {
    new ItemDisplayRule
    {
        ruleType = ItemDisplayRuleType.ParentedPrefab,
        followerPrefab = MDL,
childName = "CalfL",
localPos = new Vector3(0.07311F, 0.09037F, -0.00888F),
localAngles = new Vector3(12.57096F, 162.2495F, 178.7365F),
localScale = new Vector3(0.47F, 0.47F, 0.6F) }
            });
            rules.Add("mdlMage", new ItemDisplayRule[]
            {
    new ItemDisplayRule
    {
        ruleType = ItemDisplayRuleType.ParentedPrefab,
        followerPrefab = MDL,
childName = "FootL",
localPos = new Vector3(-0.00998F, 0.01119F, -0.06469F),
localAngles = new Vector3(315.5827F, 187.5823F, 189.1592F),
localScale = new Vector3(0.2F, 0.2F, 0.2F)        }
            });
            rules.Add("mdlMerc", new ItemDisplayRule[]
            {
    new ItemDisplayRule
    {
        ruleType = ItemDisplayRuleType.ParentedPrefab,
        followerPrefab = MDL,
childName = "FootL",
localPos = new Vector3(0.03039F, 0.01253F, -0.11448F),
localAngles = new Vector3(304.5077F, 186.6602F, 173.0096F),
localScale = new Vector3(0.25F, 0.25F, 0.25F)
    }
            });
            rules.Add("mdlLoader", new ItemDisplayRule[]
            {
    new ItemDisplayRule
    {
        ruleType = ItemDisplayRuleType.ParentedPrefab,
        followerPrefab = MDL,
childName = "FootL",
localPos = new Vector3(0.006F, -0.10728F, -0.20047F),
localAngles = new Vector3(318.6605F, 150.0478F, 194.3605F),
localScale = new Vector3(0.4F, 0.4F, 0.4F)
    }
            });
            rules.Add("mdlCroco", new ItemDisplayRule[]
            {
    new ItemDisplayRule
    {
        ruleType = ItemDisplayRuleType.ParentedPrefab,
        followerPrefab = MDL,
childName = "CalfL",
localPos = new Vector3(0.17916F, 2.85797F, -0.95134F),
localAngles = new Vector3(10.90281F, 181.5084F, 186.1528F),
localScale = new Vector3(3F, 3F, 3F)
    }
            });
            rules.Add("mdlCaptain", new ItemDisplayRule[]
            {
    new ItemDisplayRule
        {
            ruleType = ItemDisplayRuleType.ParentedPrefab,
        followerPrefab = MDL  ,
childName = "CalfL",
localPos = new Vector3(0.06974F, 0.19848F, -0.15316F),
localAngles = new Vector3(18.68582F, 179.9198F, 188.5853F),
localScale = new Vector3(0.4F, 0.4F, 0.4F)
    }
            });
            rules.Add("mdlTreebot", new ItemDisplayRule[]
 {
    new ItemDisplayRule
        {
            ruleType = ItemDisplayRuleType.ParentedPrefab,
        followerPrefab = MDL  ,
childName = "FootFrontLEnd",
localPos = new Vector3(-0.02173F, 0.0097F, -0.02386F),
localAngles = new Vector3(0.31136F, 150.9228F, 175.0508F),
localScale = new Vector3(0.2F, 0.2F, 0.2F)
    }
 });
            rules.Add("mdlBandit2", new ItemDisplayRule[]
  {
    new ItemDisplayRule
        {
            ruleType = ItemDisplayRuleType.ParentedPrefab,
        followerPrefab = MDL  ,
childName = "CalfL",
localPos = new Vector3(0.05839F, 0.35845F, -0.08628F),
localAngles = new Vector3(9.67126F, 197.2447F, 199.8692F),
localScale = new Vector3(0.32F, 0.32F, 0.32F)
    }
  });
            rules.Add("mdlVoidSurvivor", new ItemDisplayRule[]
  {
    new ItemDisplayRule
        {
            ruleType = ItemDisplayRuleType.ParentedPrefab,
        followerPrefab = MDL  ,
childName = "CalfL",
localPos = new Vector3(0.0255F, 0.36213F, 0.02881F),
localAngles = new Vector3(349.1798F, 86.03563F, 189.2694F),
localScale = new Vector3(0.30619F, 0.30619F, 0.30619F)
    }
  });
            rules.Add("mdlRailGunner", new ItemDisplayRule[]
  {
    new ItemDisplayRule
        {
            ruleType = ItemDisplayRuleType.ParentedPrefab,
        followerPrefab = MDL  ,
childName = "CalfL",
localPos = new Vector3(0.01771F, 0.35205F, 0.0558F),
localAngles = new Vector3(355.6657F, 83.68776F, 196.9867F),
localScale = new Vector3(0.39992F, 0.39992F, 0.39992F)
    }
  });


            return rules;
        }

        protected override void Initialization()
        {
            GameObject firework = Addressables.LoadAssetAsync<GameObject>(key: "5babd0aad4d1df745842603d90dd2036").WaitForCompletion().InstantiateClone("EasyFirework");
            GameObject orig = Addressables.LoadAssetAsync<GameObject>(key: "041fe0a68f990b843acd9652941f8f87").WaitForCompletion();

            ProjectileController controler = firework.GetComponent<ProjectileController>();
            controler.ghostPrefab = orig.GetComponent<ProjectileController>().ghostPrefab;

            foreach (var soundEvents in firework.GetComponents<AkEvent>())
            {
                CloudburstPlugin.Destroy(soundEvents);
            }

            foreach (var originalSound in orig.GetComponents<AkEvent>())
            {
                CCUtilities.CopyComponent<AkEvent>(originalSound, firework);
            }

            firework.GetComponent<ProjectileImpactExplosion>().impactEffect = orig.GetComponent<ProjectileImpactExplosion>().impactEffect;
            firework.GetComponent<ProjectileDamage>().damageType = DamageType.Stun1s;

            fireworkPrefab = firework;
            ContentHandler.Projectiles.RegisterProjectile(fireworkPrefab);
        }

        public override void Hooks()
        {
            GlobalHooks.onInventoryChanged += GlobalHooks_onInventoryChanged;
        }

        private void GlobalHooks_onInventoryChanged(CharacterBody body)
        {
            body.AddItemBehavior<BlastBootBehavior>(GetCount(body));
        }
    }
    public class BlastBootBehavior : CharacterBody.ItemBehavior
    {
        //Setting this to three will ensure that when the player picks up the item or starts a new stage with the item, they'll be able to use the item right away.
        public float timer = 3;

        public SkillLocator skillLocator;
            

        public void Awake()
        {
            base.enabled = false;
        }

        public void OnEnable()
        {
            if (body)
            {
                skillLocator = body.skillLocator;
                body.onSkillActivatedServer += Body_onSkillActivatedServer;
            }
        }

        public void OnDisable() {
            skillLocator = null;
            if (body)
            {
                body.onSkillActivatedServer -= Body_onSkillActivatedServer;
            }
        }
        private void Body_onSkillActivatedServer(GenericSkill skill)
        {
            if (((skillLocator != null) ? skillLocator.secondary : null) == skill && body.characterMotor && !body.characterMotor.isGrounded && timer >= 3)
            {
                Vector3 aimer = Vector3.down;
                for (int j = 0; j < 3 + (stack * 1); j++)
                {
                    body.characterMotor.velocity.y += body.jumpPower * .3f;
                    body.characterMotor.Motor.ForceUnground();

                    float theta = Random.Range(0.0f, 6.28f);
                    float x = Mathf.Cos(theta);
                    float z = Mathf.Sin(theta);
                    float c = j * 0.3777f;
                    c *= (1f / 12f);
                    aimer.x += c * x;
                    aimer.z += c * z;
                    float damage = CCUtilities.GenericFlatStackingFloat(1f, stack, 0.5f);
                    ProjectileManager.instance.FireProjectile(BlastBoot.fireworkPrefab,
                        base.transform.position,
                        Util.QuaternionSafeLookRotation(aimer),
                        body.gameObject,
                        damage * body.damage,
                        500f,
                        body.RollCrit(),
                        DamageColorIndex.Item,
                        null,
                        -1
                        );
                    aimer = Vector3.down;
                    timer = 0;
                }
            }
        }

        public void FixedUpdate()
        {
            timer += Time.fixedDeltaTime;
        }
    }

}