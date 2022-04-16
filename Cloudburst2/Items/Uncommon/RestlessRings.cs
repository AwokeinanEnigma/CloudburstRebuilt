using BepInEx.Configuration;
using Cloudburst.Builders;
using Cloudburst.Cores;
using R2API;
using RoR2;
using RoR2.Projectile;
using System;
using UnityEngine;
using UnityEngine.Networking;

namespace Cloudburst.Items.Uncommon
{

    public class RestlessRings : ItemBuilder
    {
        public static RestlessRings instance;

        public static Material heal;
        public static Material armor;
        public static Material attack;
        public static Material basic;

        public override string ItemName => "Restless Rings";

        public override string ItemLangTokenName => "RESTLESSRINGS";

        public override string ItemPickupDesc => "Every 15 seconds, gain a buff that can grant you armor, or regeneration, or attack speed.";// Also extend the duration of positive buffs.";

        public override string ItemFullDescription => @"Every 15 seconds, gain a buff which can grant you one of the following:" + Environment.NewLine
+ "<style=cIsUtility>+8 <style=cStack>(+8 per stack)</style> armor" + Environment.NewLine
 + "<style=cIsHealing>20% <style=cStack>(+20% per stack)</style> regeneration</style>" + Environment.NewLine
+ "<style=cIsDamage>10% <style=cStack>(+10% per stack)</style> attack speed</style>";
        //

        public override string ItemLore => @"A pantheon of siblings. 
A celestial mystery. 
A throne awaiting a heir. 

The one with the most brilliant mind among them had found the thread that connects them. Born of grand divinity and the arcane, it was of his nature to peel back the 
skin of existence and seek the shimmering, guiding light underneath. 
So close he came to touching the source of said light, but in his hubris he had wronged so many. The scythe of death had fell upon him before, and had condemned him until the earth had revolved about the sun one hundred times. In his freedom, derrangement and fervor proliferated within him, and soon enough he would make a fatal mistake. 
He had tried to sacrifice the blood of his nemesis' next of kin. With this folly, he would die thrice more. Each at the hand of malice itself.
And far...













far...














Far did he fall.
";

        public override ItemTier Tier => ItemTier.Tier2;

        public override string ItemModelPath => "Assets/Cloudburst/Items/RestlessRings/IMDLRestlessRings.prefab";


        public override string ItemIconPath => "Assets/Cloudburst/Items/RestlessRings/icon.png";

        public BuffDef magicArmor;
        public BuffDef magicRegen;
        public BuffDef magicAttackSpeed;

        public static ConfigEntry<float> TimeInbetweenBuffs;

        public override void CreateConfig(ConfigFile config)
        {
            TimeInbetweenBuffs = config.Bind<float>(ConfigName, "Buff Duration", 10, "How long a buff lasts.");

        }
        /*public override ItemDisplayRuleDict CreateItemDisplayRules()
        {
            return new ItemDisplayRuleDict();
        }*/

        protected override void Initialization()
        {
            instance = this;

            BuildBuffs();

            heal = AssetLoader.mainAssetBundle.LoadAsset<Material>("matRestlessRingHeal");
            armor = AssetLoader.mainAssetBundle.LoadAsset<Material>("matRestlessRingArmor");
            basic = AssetLoader.mainAssetBundle.LoadAsset<Material>("matRestfulRing");
            attack = AssetLoader.mainAssetBundle.LoadAsset<Material>("matRestlessRingAttack");

        }

        private void CreateFollowerPrefab() {
        }

        private void BuildBuffs()
        {
            this.magicRegen = new QuickBuffBuilder()
            {
                canStack = false,
                isDebuff = false,
                iconSprite = AssetLoader.mainAssetBundle.LoadAsset<Sprite>("RestlessCircle"),
               // buffColor = CloudUtils.HexToColor("#3CB043"),
            }.BuildBuff();

            this.magicAttackSpeed = new QuickBuffBuilder()
            {
                canStack = false,
                isDebuff = false,
                iconSprite = AssetLoader.mainAssetBundle.LoadAsset<Sprite>("RestlessTriangle"),
               // buffColor = CloudUtils.HexToColor("#FFA500"),
            }.BuildBuff();

            this.magicArmor = new QuickBuffBuilder()
            {
                canStack = false,
                isDebuff = false,
                iconSprite = AssetLoader.mainAssetBundle.LoadAsset<Sprite>("RestlessSquare"),
              //  buffColor = CloudUtils.HexToColor("#4D516D"),
            }.BuildBuff();
        }
        public override void Hooks()
        {
            GlobalHooks.onInventoryChanged += GlobalHooks_onInventoryChanged;
            RecalculateStatsAPI.GetStatCoefficients += RecalculateStatsAPI_GetStatCoefficients;
        }

        private void RecalculateStatsAPI_GetStatCoefficients(CharacterBody body, RecalculateStatsAPI.StatHookEventArgs args)
        {
            int magicCount = GetCount(body);
            if (body.HasBuff(magicArmor))
            {
                args.armorAdd += (8f * magicCount);
            }
            if (body.HasBuff(magicAttackSpeed))
            {
                args.attackSpeedMultAdd += (0.1f * magicCount);
            }
            if (body.HasBuff(magicRegen))
            {
                args.baseRegenAdd += (0.2f * magicCount);
            }
        }

        private void GlobalHooks_onInventoryChanged(CharacterBody body)
        {
            if (NetworkServer.active)
            {
                body.AddItemBehavior<RestlessRingsBehavior>(GetCount(body));
            }
        }


        public override ItemDisplayRuleDict CreateItemDisplayRules()
        {
            var ItemBodyModelPrefab = AssetLoader.mainAssetBundle.LoadAsset<GameObject>("RestlessFollowerDummy.prefab");
            var ItemFollowerPrefab = AssetLoader.mainAssetBundle.LoadAsset<GameObject>("RestlessFollower.prefab"); ;
            var ItemFollower = ItemBodyModelPrefab.AddComponent<ItemFollowerSmooth>();

            ItemFollower.itemDisplay = ItemBodyModelPrefab.AddComponent<ItemDisplay>();
            ItemFollower.itemDisplay.rendererInfos = CCUtilities.GatherRenderInfos(ItemBodyModelPrefab);
            ItemFollower.followerPrefab = ItemFollowerPrefab;
            ItemFollower.targetObject = ItemBodyModelPrefab;
            ItemFollower.distanceDampTime = 0.3f;
            ItemFollower.distanceMaxSpeed = 50;
            ItemFollower.SmoothingNumber = 0.1f;
            ItemFollower.AddComponent<RestlessRingsGlow>();

            
            ItemDisplayRuleDict rules = new ItemDisplayRuleDict();

            rules.Add("mdlCommandoDualies", new RoR2.ItemDisplayRule[]
            {
                new RoR2.ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = ItemBodyModelPrefab,
  childName = "Base",
localPos = new Vector3(-1.66776F, -0.21747F, -0.52862F),
localAngles = new Vector3(270F, 0F, 0F),
localScale = new Vector3(0.15F, 0.15F, 0.15F)
                }
     });
            rules.Add("mdlHuntress", new RoR2.ItemDisplayRule[]
            {
                new RoR2.ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = ItemBodyModelPrefab,
childName = "Base",
localPos = new Vector3(-1.32395F, 0.51393F, -0.71607F),
localAngles = new Vector3(270F, 0F, 0F),
localScale = new Vector3(0.15F, 0.15F, 0.15F)
                }
            });
            //ruleLookup.Add("mdlHuntress", 0.1f);
            rules.Add("mdlToolbot", new RoR2.ItemDisplayRule[]
            {
                new RoR2.ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = ItemBodyModelPrefab,
childName = "Base",
localPos = new Vector3(8.79843F, 2.4368F, 5.00038F),
localAngles = new Vector3(270F, 180F, 0F),
localScale = new Vector3(0.15F, 0.15F, 0.15F)
                }
            });
            rules.Add("mdlEngi", new RoR2.ItemDisplayRule[]
            {
                new RoR2.ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = ItemBodyModelPrefab,
childName = "Base",
localPos = new Vector3(-2.26939F, -1.21903F, -0.00543F),
localAngles = new Vector3(270F, 0F, 0F),
localScale = new Vector3(0.15F, 0.15F, 0.15F)
                }
            });
            rules.Add("mdlMage", new RoR2.ItemDisplayRule[]
            {
                new RoR2.ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = ItemBodyModelPrefab,
childName = "Base",
localPos = new Vector3(-0.94248F, -0.05662F, -0.01945F),
localAngles = new Vector3(270F, 0F, 0F),
localScale = new Vector3(0.15F, 0.15F, 0.15F)
                }
            });
            rules.Add("mdlMerc", new RoR2.ItemDisplayRule[]
            {
                new RoR2.ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = ItemBodyModelPrefab,
childName = "Base",
localPos = new Vector3(-0.91384F, -0.37104F, 0.1309F),
localAngles = new Vector3(270F, 0F, 0F),
localScale = new Vector3(0.15F, 0.15F, 0.15F)
                }
            });
            rules.Add("mdlTreebot", new RoR2.ItemDisplayRule[]
            {
                new RoR2.ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = ItemBodyModelPrefab,
  childName = "Base",
localPos = new Vector3(-2.34945F, 0.69277F, -1.27591F),
localAngles = new Vector3(270F, 0F, 0F),
localScale = new Vector3(0.15F, 0.15F, 0.15F)
                }
            });
            rules.Add("mdlLoader", new RoR2.ItemDisplayRule[]
            {
                new RoR2.ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = ItemBodyModelPrefab,
            childName = "Base",
localPos = new Vector3(-1.26621F, -0.11008F, 0.05336F),
localAngles = new Vector3(270F, 0F, 0F),
localScale = new Vector3(0.15F, 0.15F, 0.15F)
                }
            });
            rules.Add("mdlCroco", new RoR2.ItemDisplayRule[]
            {
                new RoR2.ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = ItemBodyModelPrefab,
  childName = "Base",
localPos = new Vector3(-7.26764F, -5.35658F, 4.52467F),
localAngles = new Vector3(90F, 0F, 0F),
localScale = new Vector3(0.2F, 0.2F, 0.2F)
                }
            });
            rules.Add("mdlCaptain", new RoR2.ItemDisplayRule[]
            {
                new RoR2.ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = ItemBodyModelPrefab,
           childName = "Base",
localPos = new Vector3(-1.0712F, -0.0798F, -0.59302F),
localAngles = new Vector3(270F, 0F, 0F),
localScale = new Vector3(0.15F, 0.15F, 0.15F)
                }
            });
            rules.Add("mdlBandit2", new RoR2.ItemDisplayRule[]
            {
                new RoR2.ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = ItemBodyModelPrefab,
childName = "Base",
localPos = new Vector3(-0.45782F, -0.51298F, -0.39241F),
localAngles = new Vector3(347.6326F, 269.9879F, 89.96059F),
localScale = new Vector3(0.1F, 0.1F, 0.1F)
                }
            });
            rules.Add("mdlScav", new RoR2.ItemDisplayRule[]
{
                new RoR2.ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,  
                    followerPrefab = ItemBodyModelPrefab,
childName = "Base",
localPos = new Vector3(-13.53619F, -4.10502F, 14.88354F),
localAngles = new Vector3(282.4766F, 84.02295F, 277.8279F),
localScale = new Vector3(0.5F, 0.5F, 0.5F)
                }
});
            return rules;
        }
    }

    public class RestlessRingsGlow : NetworkBehaviour
    {
        [SyncVar]
        public int currentRing;

        private CharacterBody body;
        private RestlessRingsBehavior restlessRings;

        private Renderer renderer;

        private ParticleSystem sys;
        private ItemDisplay display;


        public void Start() {
            SetFields();
        }
        void SetFields()
        {

            display = GetComponentInParent<ItemDisplay>();
            var follower = GetComponentInParent<ItemFollowerSmooth>();
            if (display)
            {
                var mdl = display.GetComponentInParent<RoR2.CharacterModel>();
                if (mdl)
                {
                    body = mdl.body;
                    restlessRings = body.GetComponent<RestlessRingsBehavior>();
                    restlessRings.OnBuffChanged += RestlessRings_OnBuffChanged;
                }
            }
            else { Destroy(this); }
            if (follower)
            {
                var instance = follower.followerInstance;
                if (instance)
                {
                    renderer = instance.transform.Find("RINGSNEW/Cube").GetComponent<Renderer>();
                    sys = instance.transform.Find("RINGSNEW/Cube/HealthEmitter").GetComponent<ParticleSystem>();
                }
            }
            else { Destroy(this); }

            if (!body.healthComponent.alive)
            {
                Destroy(this);
            }

        }

        public void FixedUpdate() {
            if (!body || !renderer || !restlessRings || !sys)
            {
                SetFields();
            }
        }

        private void RestlessRings_OnBuffChanged(object sender, int e)
        {
            currentRing = e;

            if (!(display.visibilityLevel == VisibilityLevel.Invisible))
            {
                switch (e)
                {
                    case 0:
                        if (renderer)
                        {
                            renderer.materials = new Material[2] { RestlessRings.armor, RestlessRings.basic };
                        }
                        break;
                    case 1:
                        if (renderer)
                        {
                            renderer.materials = new Material[2] { RestlessRings.armor, RestlessRings.basic };
                        }
                        break;
                    case 2:
                        if (renderer)
                        {
                            renderer.materials = new Material[2] { RestlessRings.attack, RestlessRings.basic };
                        }
                        break;
                    case 3:
                        if (renderer)
                        {
                            renderer.materials = new Material[2] { RestlessRings.heal, RestlessRings.basic };
                            sys.Emit(5);
                        }
                        break;
                    case 4:
                        if (renderer)
                        {
                            sys.Emit(5);
                            renderer.materials = new Material[2] { RestlessRings.heal, RestlessRings.basic };
                        }
                        break;
                    default:
                        throw new NullReferenceException("THIS CAN'T FUCKING HAPPEN");
                }
            }
            //mat.Add(RestlessRings.basic);
            //LogCore.LogI("hi");

            // this is utterly fucking retarded
            // but i HAVE to do this or the thing fucking breaks  
            //var there = mat.ToArray();

        }
    }

    public class RestlessRingsBehavior : CharacterBody.ItemBehavior
    { 
        private float timer = 0;
        private int oldCount;
        private Xoroshiro128Plus rng;


        public event EventHandler<int> OnBuffChanged;

        public void Start()
        {
            rng = new Xoroshiro128Plus((ulong)DateTime.UtcNow.Ticks);
            AddBuff(rng.RangeInt(1, 4));
        }
        void AddBuff(int count)
        {            
            //the count is networked, as on all clients it SHOULD be the same
            //only the buff spawning part should be on the host
            //which means that the material switching should be networked
            
            if (NetworkServer.active)
            {
                switch (count)
                {
                    case 1:
                        body.AddBuff(RestlessRings.instance.magicArmor);
                        break;
                    case 2:
                        Util.PlaySound("Play_item_proc_crit_attack_speed1", base.gameObject);
                        body.AddBuff(RestlessRings.instance.magicAttackSpeed);
                        break;
                    case 3:
                        Util.PlaySound("char_healing_drone_heal_02", base.gameObject);
                        body.AddBuff(RestlessRings.instance.magicRegen);
                        break;
                    case 4:
                        Util.PlaySound("char_healing_drone_heal_02", base.gameObject);
                        body.AddBuff(RestlessRings.instance.magicRegen);
                        break;
                }
            }
            OnBuffChanged?.Invoke(this, count);
        }
        public void FixedUpdate()
        {
            if (body)
            {
                timer += Time.deltaTime;
                if (timer >= RestlessRings.TimeInbetweenBuffs.Value)
                {
                    CCUtilities.SafeRemoveBuff(RestlessRings.instance.magicArmor, body);
                    CCUtilities.SafeRemoveBuff(RestlessRings.instance.magicAttackSpeed, body);
                    CCUtilities.SafeRemoveBuff(RestlessRings.instance.magicRegen, body);

                    int count = rng.RangeInt(1, 4);
                    //roll me a new number you Worthless Fucking Number Generator 128+
                    if (count == oldCount)
                    {
                        count = rng.RangeInt(1, 4);
                    }
                    oldCount = count;
                    AddBuff(count);
                    timer = 0;
                }
            }
        }
    }
}
