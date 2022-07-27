using System;
using System.Collections.Generic;
using Cloudburst.Builders;
using Cloudburst.CEntityStates.Guardsman;
using Cloudburst.Cores;

using EntityStates;
using R2API;
using RoR2;
using RoR2.CharacterAI;
using RoR2.Navigation;
using RoR2.Skills;
using UnityEngine;

namespace Cloudburst.Enemies
{
    class Guardsman : EnemyCreator<Guardsman>
    {


        public override string EnemyLore => @"Just came.";

        public override string EnemyName => "Guardian";

        public override string EnemyInternalName => "Brusier";

        public override string BodyName => "TarGuardian";


        public override GameObject EnemyMdl => AssetLoader.mainAssetBundle.LoadAsset<GameObject>("mdlClayGuardsman");

        public override string EnemySubtitle => "YOU'RE FUCKIN DEAD BROOOOOOOOOOOOOO";

        public override int DirectorCost => 20;

        public override bool NoElites => false;

        public override bool ForbiddenAsBoss => false;

        public override HullClassification HullClassification => HullClassification.Golem;

        public override MapNodeGroup.GraphType GraphType => MapNodeGroup.GraphType.Ground;

        public override Material GetMasteryMat()
        {
            return RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/CharacterBodies/BrotherGlassBody").GetComponentInChildren<CharacterModel>().baseRendererInfos[0].defaultMaterial;
        }


        public override void AlterStatemachines(SetStateOnHurt hurt, NetworkStateMachine network)
        {
            base.AlterStatemachines(hurt, network);
        }

        public override void GenerateEquipmentDisplays(List<ItemDisplayRuleSet.KeyAssetRuleGroup> obj)
        {
            base.GenerateEquipmentDisplays(obj);

        }

        public override void CreateMaster()
        {
            base.CreateMaster();
            GameObject gameObject = RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/CharacterMasters/LemurianMaster").InstantiateClone(EnemyInternalName + "Master", true);
            gameObject.GetComponent<RoR2.CharacterMaster>().bodyPrefab = EnemyBody;
            foreach (AISkillDriver obj in gameObject.GetComponentsInChildren<AISkillDriver>())
            {
                UnityEngine.Object.DestroyImmediate(obj);
            }
            gameObject.GetComponent<BaseAI>().fullVision = false;
            gameObject.GetComponent<BaseAI>().aimVectorMaxSpeed = 360f;


            AISkillDriver aiskillDriver2 = gameObject.AddComponent<AISkillDriver>();
            aiskillDriver2.customName = "Axe";
            aiskillDriver2.movementType = AISkillDriver.MovementType.ChaseMoveTarget;
            aiskillDriver2.moveTargetType = AISkillDriver.TargetType.CurrentEnemy;
            aiskillDriver2.activationRequiresAimConfirmation = true;
            aiskillDriver2.activationRequiresTargetLoS = true;
            aiskillDriver2.selectionRequiresTargetLoS = true;
            aiskillDriver2.maxDistance = 17f;
            aiskillDriver2.minDistance = 10f;
            aiskillDriver2.requireSkillReady = false;
            aiskillDriver2.aimType = AISkillDriver.AimType.AtCurrentEnemy;
            aiskillDriver2.ignoreNodeGraph = true;
            aiskillDriver2.moveInputScale = .8f;
            aiskillDriver2.driverUpdateTimerOverride = 0.25f;
            aiskillDriver2.buttonPressType = AISkillDriver.ButtonPressType.Hold;
            aiskillDriver2.minTargetHealthFraction = float.NegativeInfinity;
            aiskillDriver2.maxTargetHealthFraction = float.PositiveInfinity;
            aiskillDriver2.minUserHealthFraction = float.NegativeInfinity;
            aiskillDriver2.maxUserHealthFraction = 1f;
            aiskillDriver2.skillSlot = SkillSlot.Primary;

            AISkillDriver aiskillDriver3 = gameObject.AddComponent<AISkillDriver>();
            aiskillDriver3.customName = "Stomp";
            aiskillDriver3.movementType = AISkillDriver.MovementType.ChaseMoveTarget;
            aiskillDriver3.moveTargetType = AISkillDriver.TargetType.CurrentEnemy;
            aiskillDriver3.activationRequiresAimConfirmation = true;
            aiskillDriver3.activationRequiresTargetLoS = false;
            aiskillDriver3.selectionRequiresTargetLoS = false;
            aiskillDriver3.maxDistance = 10f;
            aiskillDriver3.minDistance = 0f;
            aiskillDriver3.requireSkillReady = false;
            aiskillDriver3.aimType = AISkillDriver.AimType.AtCurrentEnemy;
            aiskillDriver3.ignoreNodeGraph = false;
            aiskillDriver3.moveInputScale = .5f;
            aiskillDriver3.driverUpdateTimerOverride = 0.1f;
            aiskillDriver3.buttonPressType = AISkillDriver.ButtonPressType.Hold;
            aiskillDriver3.minTargetHealthFraction = float.NegativeInfinity;
            aiskillDriver3.maxTargetHealthFraction = float.PositiveInfinity;
            aiskillDriver3.minUserHealthFraction = float.NegativeInfinity;
            aiskillDriver3.maxUserHealthFraction = float.PositiveInfinity;
            aiskillDriver3.skillSlot = SkillSlot.Secondary;

            AISkillDriver aiskillDriver1 = gameObject.AddComponent<AISkillDriver>();
            aiskillDriver1.customName = "Chase";
            aiskillDriver1.movementType = AISkillDriver.MovementType.ChaseMoveTarget;
            aiskillDriver1.moveTargetType = AISkillDriver.TargetType.CurrentEnemy;
            aiskillDriver1.activationRequiresAimConfirmation = false;
            aiskillDriver1.activationRequiresTargetLoS = false;
            aiskillDriver1.selectionRequiresTargetLoS = false;
            aiskillDriver1.maxDistance = float.PositiveInfinity;
            aiskillDriver1.minDistance = 0;
            aiskillDriver1.requireSkillReady = false;
            aiskillDriver1.aimType = AISkillDriver.AimType.AtMoveTarget;
            aiskillDriver1.ignoreNodeGraph = false;
            aiskillDriver1.moveInputScale = 1f;
            aiskillDriver1.driverUpdateTimerOverride = 0.5f;
            aiskillDriver1.buttonPressType = AISkillDriver.ButtonPressType.Hold;
            aiskillDriver1.minTargetHealthFraction = float.NegativeInfinity;
            aiskillDriver1.maxTargetHealthFraction = float.PositiveInfinity;
            aiskillDriver1.minUserHealthFraction = float.NegativeInfinity;
            aiskillDriver1.maxUserHealthFraction = float.PositiveInfinity;
            aiskillDriver1.skillSlot = SkillSlot.None;
            aiskillDriver1.shouldSprint = false;

            EnemyMaster = gameObject;

            Content.ContentHandler.Masters.RegisterMaster(EnemyMaster);
        }


        public override void GenerateRenderInfos(List<CharacterModel.RendererInfo> infos, Transform transform)
        {
            base.GenerateRenderInfos(infos, transform);
            /*var broom = arg2.Find("Wyatt.002");
            var mat = broom.GetComponentInChildren<SkinnedMeshRenderer>();
            arg1.Add(new CharacterModel.RendererInfo
            {
                defaultMaterial = mat.material,
                defaultShadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On,
                ignoreOverlays = false,
                renderer = mat,
            });
            var wyatt = arg2.Find("Cube");
            var wyattMat = wyatt.GetComponent<SkinnedMeshRenderer>();
            arg1.Add(new CharacterModel.RendererInfo
            {
                defaultMaterial = wyattMat.material,
                defaultShadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On,
                ignoreOverlays = false,
                renderer = wyattMat,
            }); */

            /*fuck you and your vars and args god
            SkinnedMeshRenderer broomMat = transform.Find("Brom.002").GetComponentInChildren<SkinnedMeshRenderer>();
            infos.Add(new CharacterModel.RendererInfo
            {
                defaultMaterial = broomMat.material,
                defaultShadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On,
                ignoreOverlays = false,
                renderer = broomMat,
            });
            SkinnedMeshRenderer wyattMat = transform.Find("Wyatt.002").GetComponent<SkinnedMeshRenderer>();
            infos.Add(new CharacterModel.RendererInfo
            {
                defaultMaterial = wyattMat.material,
                defaultShadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On,
                ignoreOverlays = false,
                renderer = wyattMat,
            });//*/
        }

        public override void SetupCharacterBody(CharacterBody characterBody)
        {
            base.SetupCharacterBody(characterBody);
            characterBody.baseAcceleration = 80f;
            characterBody.baseArmor = 20; //Base armor this character has, set to 20 if this character is melee 
            characterBody.baseAttackSpeed = 1; //Base attack speed, usually 1
            characterBody.baseCrit = 1;  //Base crit, usually one
            characterBody.baseDamage = 12; //Base damage
            characterBody.baseJumpCount = 1; //Base jump amount, set to 2 for a double jump. 
            characterBody.baseJumpPower = 16; //Base jump power
            characterBody.baseMaxHealth = 150; //Base health, basically the health you have when you start a new run
            characterBody.baseMaxShield = 0; //Base shield, basically the same as baseMaxHealth but with shields
            characterBody.baseMoveSpeed = 7; //Base move speed, this is usual 7
            characterBody.baseNameToken = EnemyInternalName + "_BODY_NAME"; //The base name token. 
            characterBody.subtitleNameToken = EnemyInternalName + "_BODY_SUBTITLE"; //Set this if its a boss
            characterBody.baseRegen = 1.5f; //Base health regen.
            characterBody.bodyFlags = (CharacterBody.BodyFlags.ImmuneToExecutes); ///Base body flags, should be self explanatory 
            characterBody._defaultCrosshairPrefab = characterBody._defaultCrosshairPrefab = RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/CharacterBodies/HuntressBody").GetComponent<CharacterBody>()._defaultCrosshairPrefab; //The crosshair prefab.
            characterBody.hideCrosshair = false; //Whether or not to hide the crosshair
            characterBody.hullClassification = HullClassification.Golem; //The hull classification, usually used for AI
            characterBody.isChampion = false; //Set this to true if its A. A boss or B. A miniboss
            characterBody.levelArmor = 0; //Armor gained when leveling up. 
            characterBody.levelAttackSpeed = 0; //Attackspeed gained when leveling up. 
            characterBody.levelCrit = 0; //Crit chance gained when leveling up. 
            characterBody.levelDamage = 2.4f; //Damage gained when leveling up. 
            characterBody.levelArmor = 0; //Armor gained when leveling up. 
            characterBody.levelJumpPower = 0; //Jump power gained when leveling up. 
            characterBody.levelMaxHealth = 42; //Health gained when leveling up. 
            characterBody.levelMaxShield = 0; //Shield gained when leveling up. 
            characterBody.levelMoveSpeed = 0; //Move speed gained when leveling up. 
            characterBody.levelRegen = 0.5f; //Regen gained when leveling up. 
            //credits to moffein for the icon
            //no 
            //credit
            //fuck you moffein
            characterBody.portraitIcon = AssetLoader.mainAssetBundle.LoadAsset<Texture>("WyattPortrait"); //The portrait icon, shows up in multiplayer and the death UI
            characterBody.preferredPodPrefab = RoR2.LegacyResourcesAPI.Load<GameObject>("prefabs/networkedobjects/EnemyPod");
        }

        public override void CreatePrimary(SkillLocator skillLocator, SkillFamily skillFamily)
        {
            Cloudburst.Content.ContentHandler.Loadouts.RegisterEntityState(typeof(Swing));

            SkillDef primarySkillDef = ScriptableObject.CreateInstance<SkillDef>();

            primarySkillDef.skillName = "RELIC_BODY_SECONDARY_BUFFDISC_NAME";
            primarySkillDef.skillNameToken = "RELIC_BODY_SECONDARY_BUFFDISC_NAME";
            //    skillDescriptionToken = "RELIC_BODY_SECONDARY_BUFFDISC_DESCRIPTION";
            //        skillIcon = Modules.Assets.mainAssetBundle.LoadAsset<Sprite>("texLunarEncore1Icon");
            primarySkillDef.activationState = new EntityStates.SerializableEntityStateType(typeof(Swing));
            primarySkillDef.activationStateMachineName = "Body";
            primarySkillDef.baseMaxStock = 1;
            primarySkillDef.baseRechargeInterval = 5f;
            primarySkillDef.beginSkillCooldownOnSkillEnd = false;
            primarySkillDef.canceledFromSprinting = false;
            primarySkillDef.forceSprintDuringState = false;
            primarySkillDef.fullRestockOnAssign = true;
            primarySkillDef.interruptPriority = EntityStates.InterruptPriority.Skill;
            primarySkillDef.resetCooldownTimerOnUse = false;
            primarySkillDef.isCombatSkill = true;
            primarySkillDef.mustKeyPress = false;
            primarySkillDef.cancelSprintingOnActivation = false;
            primarySkillDef.rechargeStock = 1;
            primarySkillDef.requiredStock = 1;
            primarySkillDef.stockToConsume = 1;

            //R2API.LanguageAPI.Add(primarySkillDef.skillNameToken, "G22 Grav-Broom");
            //R2API.LanguageAPI.Add(primarySkillDef.skillDescriptionToken, "<style=cIsUtility>Agile</style>. Swing in front for X% damage. [NOT IMPLEMENTED] Every 4th hit <style=cIsDamage>Spikes</style>.");
            //R2API.LanguageAPI.Add(primarySkillDef.keywordTokens[1], "<style=cKeywordName>Weightless</style><style=cSub>Slows and removes gravity from target.</style>");
            // R2API.LanguageAPI.Add(primarySkillDef.keywordTokens[2], "<style=cKeywordName>Spiking</style><style=cSub>Forces an enemy to travel downwards, causing a shockwave if they impact terrain.</style>");

            Cloudburst.Content.ContentHandler.Loadouts.RegisterSkillDef(primarySkillDef);
            SkillFamily primarySkillFamily = skillLocator.primary.skillFamily;

            primarySkillFamily.variants[0] = new SkillFamily.Variant
            {
                skillDef = primarySkillDef,
                viewableNode = new ViewablesCatalog.Node(primarySkillDef.skillNameToken, false, null)

            };
        }
        public override void CreateSecondary(SkillLocator skillLocator, SkillFamily skillFamily)
        {

            Cloudburst.Content.ContentHandler.Loadouts.RegisterEntityState(typeof(Stomp));
            //Cloudburst.Content.ContentHandler.Loadouts.RegisterEntityState(typeof(TrashOut3));

            SkillDef secondarySkillDef = ScriptableObject.CreateInstance<SkillDef>();

            secondarySkillDef.skillName = "RELIC_BODY_SECONDARY_BUFFDISC_NAME";
            secondarySkillDef.skillNameToken = "RELIC_BODY_SECONDARY_BUFFDISC_NAME";
            secondarySkillDef.skillDescriptionToken = "RELIC_BODY_SECONDARY_BUFFDISC_DESCRIPTION";
            //secondarySkillDef.icon = Modules.Assets.mainAssetBundle.LoadAsset<Sprite>("texLunarEncore1Icon");
            secondarySkillDef.activationState = new EntityStates.SerializableEntityStateType(typeof(Stomp));
            secondarySkillDef.activationStateMachineName = "Body";
            secondarySkillDef.baseMaxStock = 1;
            secondarySkillDef.baseRechargeInterval = 5f;
            secondarySkillDef.beginSkillCooldownOnSkillEnd = false;
            secondarySkillDef.canceledFromSprinting = false;
            secondarySkillDef.forceSprintDuringState = false;
            secondarySkillDef.fullRestockOnAssign = true;
            secondarySkillDef.interruptPriority = EntityStates.InterruptPriority.Skill;
            secondarySkillDef.resetCooldownTimerOnUse = false;
            secondarySkillDef.isCombatSkill = true;
            secondarySkillDef.mustKeyPress = false;
            secondarySkillDef.cancelSprintingOnActivation = false;
            secondarySkillDef.rechargeStock = 1;
            secondarySkillDef.requiredStock = 1;
            secondarySkillDef.stockToConsume = 1;

            R2API.LanguageAPI.Add(secondarySkillDef.skillNameToken, "Trash Out");
            R2API.LanguageAPI.Add(secondarySkillDef.skillDescriptionToken, "Deploy a winch that reels you towards an enemy, and <style=cIsDamage>Spike</style> for <style=cIsDamage>X%</style>.");

            Cloudburst.Content.ContentHandler.Loadouts.RegisterSkillDef(secondarySkillDef);
            SkillFamily secondarySkillFamily = skillLocator.secondary.skillFamily;

            secondarySkillFamily.variants[0] = new SkillFamily.Variant
            {
                skillDef = secondarySkillDef,
                viewableNode = new ViewablesCatalog.Node(secondarySkillDef.skillNameToken, false, null)

            };
        }
        public override void CreateUtility(SkillLocator skillLocator, SkillFamily skillFamily)
        {
            SkillDef utilitySkillDef = ScriptableObject.CreateInstance<SkillDef>();
            //utilitySkillDef.activationState = new SerializableEntityStateType(typeof(YeahDudeIBetterBeOrYouCanFuckinKissMyAssHumanCentipede));
            utilitySkillDef.activationStateMachineName = "SuperMarioJump";
            utilitySkillDef.baseMaxStock = 1;
            utilitySkillDef.baseRechargeInterval = 2f;
            utilitySkillDef.beginSkillCooldownOnSkillEnd = true;
            utilitySkillDef.canceledFromSprinting = false;
            utilitySkillDef.fullRestockOnAssign = false;
            utilitySkillDef.interruptPriority = InterruptPriority.Skill;
            utilitySkillDef.isCombatSkill = true;
            utilitySkillDef.mustKeyPress = false;
            utilitySkillDef.cancelSprintingOnActivation = false;
            utilitySkillDef.rechargeStock = 1;
            utilitySkillDef.requiredStock = 1;
            utilitySkillDef.stockToConsume = 1;
            utilitySkillDef.skillDescriptionToken = "WYATT_UTILITY_DESCRIPTION";
            utilitySkillDef.skillName = "aaa";
            utilitySkillDef.skillNameToken = "WYATT_UTILITY_NAME";
            //utilitySkillDef.icon = AssetsCore.wyattUtility;
            utilitySkillDef.keywordTokens = new string[] {
                 "KEYWORD_RUPTURE",
             };

            SkillDef utilitySkillDef2 = ScriptableObject.CreateInstance<SkillDef>();
            //  utilitySkillDef2.activationState = new SerializableEntityStateType(typeof(FireWinch));
            utilitySkillDef2.activationStateMachineName = "Weapon";
            utilitySkillDef2.baseMaxStock = 1;
            utilitySkillDef2.baseRechargeInterval = 4f;
            utilitySkillDef2.beginSkillCooldownOnSkillEnd = true;
            utilitySkillDef2.canceledFromSprinting = false;
            utilitySkillDef2.fullRestockOnAssign = false;
            utilitySkillDef2.interruptPriority = InterruptPriority.Skill;
            utilitySkillDef2.isCombatSkill = true;
            utilitySkillDef2.mustKeyPress = false;
            utilitySkillDef2.cancelSprintingOnActivation = false;
            utilitySkillDef2.rechargeStock = 1;
            utilitySkillDef2.requiredStock = 1;
            utilitySkillDef2.stockToConsume = 1;
            utilitySkillDef2.skillDescriptionToken = "WYATT_UTILITY2_DESCRIPTION";
            utilitySkillDef2.skillName = "aaa";
            utilitySkillDef2.skillNameToken = "WYATT_UTILITY2_NAME";
            //  utilitySkillDef2.icon = AssetsCore.wyattUtilityAlt;
            utilitySkillDef2.keywordTokens = Array.Empty<string>();

            R2API.LanguageAPI.Add(utilitySkillDef.skillNameToken, "Flow");
            R2API.LanguageAPI.Add(utilitySkillDef.skillDescriptionToken, "Idk if this even works rn tbh.\nActivate Flow for 4 seconds (0.4s for each stack of Groove, max 8 seconds). During flow, you are unable to lose or gain Groove. After Flow ends, lose all stacks groove.");
            R2API.LanguageAPI.Add("KEYWORD_RUPTURE", "<style=cKeywordName>Flow</style><style=cSub> Gives you a double jump. +30% cooldown reduction.</style>");


            R2API.LanguageAPI.Add(utilitySkillDef2.skillNameToken, "G22 WINCH");
            R2API.LanguageAPI.Add(utilitySkillDef2.skillDescriptionToken, "Fire a winch that deals <style=cIsDamage>500%</style> damage and <style=cIsUtility>pulls you</style> towards the target.");

            Cloudburst.Content.ContentHandler.Loadouts.RegisterSkillDef(utilitySkillDef);
            SkillFamily utilitySkillFamily = skillLocator.utility.skillFamily;

            utilitySkillFamily.variants[0] = new SkillFamily.Variant
            {
                skillDef = utilitySkillDef,
                viewableNode = new ViewablesCatalog.Node(utilitySkillDef.skillNameToken, false, null)
            };
        }
        public override void CreateSpecial(SkillLocator skillLocator, SkillFamily skillFamily)
        {
           // Cloudburst.Content.ContentHandler.Loadouts.RegisterEntityState(typeof(Stomp));
            //Cloudburst.Content.ContentHandler.Loadouts.RegisterEntityState(typeof(TrashOut3));

            SkillDef specialSkillDef = ScriptableObject.CreateInstance<SkillDef>();

            specialSkillDef.skillName = "RELIC_BODY_special_BUFFDISC_NAME";
            specialSkillDef.skillNameToken = "RELIC_BODY_special_BUFFDISC_NAME";
            specialSkillDef.skillDescriptionToken = "RELIC_BODY_special_BUFFDISC_DESCRIPTION";
            //specialSkillDef.icon = Modules.Assets.mainAssetBundle.LoadAsset<Sprite>("texLunarEncore1Icon");
            specialSkillDef.activationState = new EntityStates.SerializableEntityStateType(typeof(Stomp));
            specialSkillDef.activationStateMachineName = "Body";
            specialSkillDef.baseMaxStock = 1;
            specialSkillDef.baseRechargeInterval = 5f;
            specialSkillDef.beginSkillCooldownOnSkillEnd = false;
            specialSkillDef.canceledFromSprinting = false;
            specialSkillDef.forceSprintDuringState = false;
            specialSkillDef.fullRestockOnAssign = true;
            specialSkillDef.interruptPriority = EntityStates.InterruptPriority.Skill;
            specialSkillDef.resetCooldownTimerOnUse = false;
            specialSkillDef.isCombatSkill = true;
            specialSkillDef.mustKeyPress = false;
            specialSkillDef.cancelSprintingOnActivation = false;
            specialSkillDef.rechargeStock = 1;
            specialSkillDef.requiredStock = 1;
            specialSkillDef.stockToConsume = 1;

            R2API.LanguageAPI.Add(specialSkillDef.skillNameToken, "Trash Out");
            R2API.LanguageAPI.Add(specialSkillDef.skillDescriptionToken, "Deploy a winch that reels you towards an enemy, and <style=cIsDamage>Spike</style> for <style=cIsDamage>X%</style>.");

            Cloudburst.Content.ContentHandler.Loadouts.RegisterSkillDef(specialSkillDef);
            SkillFamily specialSkillFamily = skillLocator.special.skillFamily;

            specialSkillFamily.variants[0] = new SkillFamily.Variant
            {
                skillDef = specialSkillDef,
                viewableNode = new ViewablesCatalog.Node(specialSkillDef.skillNameToken, false, null)

            };
        }

        protected override void Initialization()
        {
        }
    }
}