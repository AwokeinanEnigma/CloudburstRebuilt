using System;
using Cloudburst.CEntityStates.EngineerStates;
using Cloudburst.CEntityStates.EngineerStates.Turret;
using Cloudburst.Cores;

using EntityStates;
using R2API;
using RoR2;
using RoR2.Navigation;
using RoR2.Projectile;
using RoR2.Skills;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Cloudburst.Engineer
{
    class Engineer
    {
        //she stares you down
        //she cares your top
        //you know she'll never love you

        public static Engineer instance;

        //TODO
        //Fix skins
        //Tweak primary
        protected GameObject flameTurret;
        public static GameObject flameTurretMaster;
        public static GameObject projectileGameObject;
        protected GameObject engineerObject;

        private FlameTurrets turrets;
        public class FlameTurrets : ModifiedEnemyBuilder
        {
            protected override string resourceMasterPath => "prefabs/charactermasters/EngiWalkerTurretMaster";

            protected override string resourceBodyPath => "prefabs/characterbodies/EngiWalkerTurretBody";

            protected override string bodyName => "EngiFlameTurret";

            protected override bool registerNetwork => true;

            public override int DirectorCost => 7595;

            public override bool NoElites => true;

            public override bool ForbiddenAsBoss => true;

            public override HullClassification HullClassification => HullClassification.Human;

            public override MapNodeGroup.GraphType GraphType => MapNodeGroup.GraphType.Ground;

            public override void OverrideCharacterbody(CharacterBody body)
            {
                base.OverrideCharacterbody(body);
                body.baseMaxHealth = 70;
                body.levelMaxHealth = 21;
                body.baseDamage = 8;
                body.levelDamage = 1.6f;

            }

            public override void OverrideSkills()
            {
                base.OverrideSkills();
                //KILL
                Cloudburst.Content.ContentHandler.Loadouts.RegisterEntityState(typeof(DeathState));

                var characterDeathBehavior = enemyBody.GetComponent<CharacterDeathBehavior>();
                characterDeathBehavior.deathState = new SerializableEntityStateType(typeof(DeathState));
            }

            public override void CreatePrimary(SkillLocator skillLocator, SkillFamily skillFamily)
            {
                base.CreatePrimary(skillLocator, skillFamily);

                SkillDef origDef = Resources.Load<SkillDef>("skilldefs/engiturretbody/EngiTurretFireBeam");
                SkillDef def = ScriptableObject.CreateInstance<SkillDef>();
                CCUtilities.CopySkillDefSettings(origDef, def);

                Cloudburst.Content.ContentHandler.Loadouts.RegisterEntityState(typeof(FireFlameThrower));

                def.activationState = new SerializableEntityStateType(typeof(FireFlameThrower));

                Cloudburst.Content.ContentHandler.Loadouts.RegisterSkillDef(def);

                skillFamily.variants[0] = new SkillFamily.Variant
                {
                    skillDef = def,
                    viewableNode = new ViewablesCatalog.Node(def.skillNameToken, false, null)

                };
            }

            public override void CreateSecondary(SkillLocator skillLocator, SkillFamily skillFamily)
            {
                base.CreateSecondary(skillLocator, skillFamily);

                SkillDef origDef = Resources.Load<SkillDef>("skilldefs/engiturretbody/EngiTurretFireBeam");
                SkillDef def = ScriptableObject.CreateInstance<SkillDef>();
                CCUtilities.CopySkillDefSettings(origDef, def);

                def.activationState = new SerializableEntityStateType(typeof(FireFlameThrower));

                Cloudburst.Content.ContentHandler.Loadouts.RegisterSkillDef(def);

                skillFamily.variants[0] = new SkillFamily.Variant
                {
                    skillDef = def,
                    viewableNode = new ViewablesCatalog.Node(def.skillNameToken, false, null)

                };
            }

            public override void CreateUtility(SkillLocator skillLocator, SkillFamily skillFamily)
            {
                base.CreateUtility(skillLocator, skillFamily);
                SkillDef origDef = Resources.Load<SkillDef>("skilldefs/engiturretbody/EngiTurretFireBeam");
                SkillDef def = ScriptableObject.CreateInstance<SkillDef>();
                CCUtilities.CopySkillDefSettings(origDef, def);

                def.activationState = new SerializableEntityStateType(typeof(FireFlameThrower));

                Cloudburst.Content.ContentHandler.Loadouts.RegisterSkillDef(def);

                skillFamily.variants[0] = new SkillFamily.Variant
                {
                    skillDef = def,
                    viewableNode = new ViewablesCatalog.Node(def.skillNameToken, false, null)

                };
            }

            public override void CreateSpecial(SkillLocator skillLocator, SkillFamily skillFamily)
            {
                base.CreateSpecial(skillLocator, skillFamily);
                SkillDef origDef = Resources.Load<SkillDef>("skilldefs/engiturretbody/EngiTurretFireBeam");
                SkillDef def = ScriptableObject.CreateInstance<SkillDef>();
                CCUtilities.CopySkillDefSettings(origDef, def);

                def.activationState = new SerializableEntityStateType(typeof(FireFlameThrower));

                Cloudburst.Content.ContentHandler.Loadouts.RegisterSkillDef(def);

                skillFamily.variants[0] = new SkillFamily.Variant
                {
                    skillDef = def,
                    viewableNode = new ViewablesCatalog.Node(def.skillNameToken, false, null)

                };
            }

        }

        public Engineer() => EngineerEdits();


        protected void EngineerEdits()
        {
            CCUtilities.LogI("Initializing Core: " + base.ToString());
            instance = this;

            engineerObject = Addressables.LoadAssetAsync<GameObject>(key: "dabe06ec87224e7468138a294762bc43").WaitForCompletion();
            RegisterNewTurret();
            AddNewSpecial();
            AddNewPrimary();
            CreateAltPrimaryProjectile();
            On.RoR2.CharacterMaster.GetDeployableSameSlotLimit += CharacterMaster_GetDeployableSameSlotLimit;
            //IL.RoR2.CharacterMaster.GetDeployableSameSlotLimit += CharacterMaster_GetDeployableSameSlotLimit;
        }

        private void CreateAltPrimaryProjectile()
        {
            projectileGameObject = RoR2.LegacyResourcesAPI.Load<GameObject>("prefabs/projectiles/EngiGrenadeProjectile").InstantiateClone("EngiGrenadeImpactProjectile", true);
            CCUtilities.RegisterNewProjectile(projectileGameObject);
            ProjectileImpactExplosion impact = projectileGameObject.GetComponent<ProjectileImpactExplosion>();
            impact.destroyOnWorld = true;
            UnityEngine.Object.Destroy(projectileGameObject.GetComponent<ProjectileStickOnImpact>());

            var mdlSkinController = engineerObject.GetComponentInChildren<ModelSkinController>();
            var skins = mdlSkinController.skins[1];

            SkinDef.ProjectileGhostReplacement replace = new SkinDef.ProjectileGhostReplacement()
            {
                projectilePrefab = projectileGameObject,
                projectileGhostReplacementPrefab = skins.projectileGhostReplacements[0].projectileGhostReplacementPrefab
            };

            int old = skins.projectileGhostReplacements.Length;
            Array.Resize<SkinDef.ProjectileGhostReplacement>(ref skins.projectileGhostReplacements, old + 1);
            skins.projectileGhostReplacements[old] = replace;

        }
        private int CharacterMaster_GetDeployableSameSlotLimit(On.RoR2.CharacterMaster.orig_GetDeployableSameSlotLimit orig, CharacterMaster self, DeployableSlot slot)
        {
            bool engi = slot == DeployableSlot.EngiTurret;
            if (!engi) return orig(self, slot);
            else
            {
                //Reflection will never fail me!
                GameObject fuckIL = self.bodyInstanceObject;
                if (fuckIL)
                {
                    if (fuckIL.GetComponent<SkillLocator>().special.skillDef.skillName == "FUCKIL")
                    {
                        return 4;
                    }
                    return 2;
                }
                return 2;
            }
        }


        protected void RegisterNewTurret()
        {
            turrets = new FlameTurrets();

            turrets.Create();

            flameTurret = turrets.enemyBody;
            flameTurretMaster = turrets.enemyMaster;
        }
        protected void AddNewSpecial()
        {
            var mdlSkinController = engineerObject.GetComponentInChildren<ModelSkinController>();
            var skins = mdlSkinController.skins;

            SkinDef.MinionSkinReplacement replace = new SkinDef.MinionSkinReplacement()
            {
                minionBodyPrefab = flameTurret,
                minionSkin = skins[1].minionSkinReplacements[1].minionSkin,

            };

            int old = mdlSkinController.skins[1].minionSkinReplacements.Length;
            Array.Resize<SkinDef.MinionSkinReplacement>(ref mdlSkinController.skins[1].minionSkinReplacements, old + 1);
            mdlSkinController.skins[1].minionSkinReplacements[old] = replace;

            //skins[1].minionSkinReplacements;

            SkillDef origDef = Resources.Load<SkillDef>("skilldefs/engibody/EngiBodyPlaceWalkerTurret");
            var newDef = ScriptableObject.CreateInstance<SkillDef>();

            CCUtilities.CopySkillDefSettings(origDef, newDef);

            Cloudburst.Content.ContentHandler.Loadouts.RegisterEntityState(typeof(PlaceFlameTurret));
            Cloudburst.Content.ContentHandler.Loadouts.RegisterSkillDef(newDef);

            R2API.LanguageAPI.Add("ENGI_SPECIAL_ALT2_NAME", "TR27 Immolator Turret");
            R2API.LanguageAPI.Add("ENGI_SPECIAL_ALT2_DESCRIPTION", "Place a <style=cIsUtility>mobile</style> turret that <style=cIsUtility>inherits all your items.</style> Fires a plume of flame for <style=cIsDamage>100% damage per second</style> that <style=cIsDamage>ignites enemies</style>. <style=cIsDamage>Explodes on death for 3x</style> maximum health. Can place up to 4.");

            newDef.baseRechargeInterval = 15;
            newDef.activationState = new SerializableEntityStateType(typeof(PlaceFlameTurret));
            newDef.baseMaxStock = 4;
            newDef.skillName = "FUCKIL";
            newDef.icon = AssetLoader.mainAssetBundle.LoadAsset<Sprite>("Assets/Cloudburst/Icons/Engineer/TR75_Immolator_Turret_2.png");
            newDef.skillNameToken = "ENGI_SPECIAL_ALT2_NAME";
            newDef.skillDescriptionToken = "ENGI_SPECIAL_ALT2_DESCRIPTION";
            newDef.icon = AssetLoader.mainAssetBundle.LoadAsset<Sprite>("Assets/Cloudburst/Icons/Engineer/TR75_Immolator_Turret_2.png");

            var locator = engineerObject.GetComponent<SkillLocator>();

            var family = locator.special.skillFamily;

            SkillFamily.Variant variant = new SkillFamily.Variant();

            variant.skillDef = newDef;

            int prevLength = family.variants.Length;
            Array.Resize<SkillFamily.Variant>(ref family.variants, prevLength + 1);
            family.variants[prevLength] = variant;
        }

        protected void AddNewPrimary()
        {
            SkillDef origDef = Resources.Load<SkillDef>("skilldefs/engibody/EngiBodyFireGrenade");
            var newDef = ScriptableObject.CreateInstance<SkillDef>();

            CCUtilities.CopySkillDefSettings(origDef, newDef);

            Cloudburst.Content.ContentHandler.Loadouts.RegisterEntityState(typeof(FireVolley));
            Cloudburst.Content.ContentHandler.Loadouts.RegisterSkillDef(newDef);

            R2API.LanguageAPI.Add("ENGINEER_PRIMARY_ALT_NAME", "Impact Grenades");
            R2API.LanguageAPI.Add("ENGINEER_PRIMARY_ALT_DESCRIPTION", "Shoot 3 impact grenades from each cannon that deal <style=cIsDamage>100%</style>");

            newDef.activationState = new SerializableEntityStateType(typeof(FireVolley));
            newDef.skillName = "GrenadeVolley";
            newDef.skillNameToken = "ENGINEER_PRIMARY_ALT_NAME";
            newDef.skillDescriptionToken = "ENGINEER_PRIMARY_ALT_DESCRIPTION";
            //newDef.beginSkillCooldownOnSkillEnd = true;
            //newDef.baseRechargeInterval = .1f;
            newDef.icon = AssetLoader.mainAssetBundle.LoadAsset<Sprite>("Assets/Cloudburst/Icons/Engineer/Impact_Grenades.png");
            newDef.canceledFromSprinting = false;
            //newDef.cancelSprintingOnActivation = false;


            var locator = engineerObject.GetComponent<SkillLocator>();

            var family = locator.primary.skillFamily;

            SkillFamily.Variant variant = new SkillFamily.Variant();

            variant.skillDef = newDef;

            int prevLength = family.variants.Length;
            Array.Resize<SkillFamily.Variant>(ref family.variants, prevLength + 1);
            family.variants[prevLength] = variant;
        }
    }
}