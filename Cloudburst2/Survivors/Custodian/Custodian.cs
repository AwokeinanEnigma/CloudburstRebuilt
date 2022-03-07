using System;
using System.Collections.Generic;
using Cloudburst.Builders;
using RoR2;
using RoR2.Projectile;
using UnityEngine;
using UnityEngine.Networking;
using EntityStates;
using R2API;
using RoR2.Skills;
using UnityEngine.AddressableAssets;
using Cloudburst.GlobalComponents;
using Cloudburst.CEntityStates.Custodian;
using Cloudburst.Custodian.Components;
using Cloudburst.Cores;

public class BuffBuilder
{

    public Sprite iconSprite;

    public Color buffColor = Color.white;

    public bool canStack;

    public EliteDef eliteDef;

    public bool isDebuff;

    public NetworkSoundEventDef startSfx;
    public BuffDef BuildBuff()
    {
        //create buff
        var buff = ScriptableObject.CreateInstance<BuffDef>();
        buff.canStack = canStack;
        buff.isDebuff = isDebuff;
        buff.iconSprite = iconSprite; // AssetsCore.mainAssetBundle.LoadAsset<Sprite>("Charm");
        buff.buffColor = buffColor;
        if (startSfx)
        {
            buff.startSfx = startSfx;
        }
        if (eliteDef)
        {
            buff.eliteDef = eliteDef;
        }

        //add again
        Cloudburst.Content.ContentHandler.Buffs.RegisterBuff(buff);
        return buff;
    }
}

namespace Cloudburst.Custodian
{

    

    class Custodian : SurvivorBuilder<Custodian>
    {
        public override string SurvivorOutro => "...and so they left, a job well done";

        public static SerializableEntityStateType Retrieve = new SerializableEntityStateType(typeof(RetrieveMaid));
        public static SerializableEntityStateType Deploy = new SerializableEntityStateType(typeof(DeployMaid));

        public override string SurvivorLore => @"Can't stop now. Can't stop now. Every step I take is a step I can't take back. Come hell or high water I will find it.

It's all a rhythm, just a rhythm. Every time I step out of line is a punishment. I will obey the groove. Nothing can stop me now.

Every scar is worth it. I can feel it, I am coming closer to it. I will have it, and it will be mine.

No matter the blood, it's worth it. I will find what I want, and I will come home.

No matter how many I slaughter, it will be mine... It can't hide from me from me forever.

It's here, I can feel it. This security chest, it has it. I crack it open, and I find it.

It's... finally mine. I hold it in my bruised hands. Has it really been years? 

She'll love this, I know.

";

        public override string SurvivorName => "Custodian";

        public override string SurvivorDescription => "The Custodian is a master of janitorial warfare who uses his MAID to control the battle field<color=#CCD3E0>" + Environment.NewLine + Environment.NewLine
        + "< ! > Send enemies upwards with the MAID, and spike them downwads with Trash Out for major damage." + Environment.NewLine + Environment.NewLine
      + "< ! > The MAID slows projectiles within her radius, use this to your advantage in combat!" + Environment.NewLine + Environment.NewLine
+ "< ! > this skill no longer exists" + Environment.NewLine + Environment.NewLine
        + "< ! > The key to success is realizing that staying away from the ground helps you stay alive longer." + Environment.NewLine + Environment.NewLine;

        public override string SurvivorInternalName => "CUSTODIAN";

        public override string BodyName => "Custodian";

        public override string UnlockableString => "";

        #region SurvivorBuilder overrides
        public override UnlockableDef MasteryUnlockString => null;//AchievementCore.WyattMastery;

        public override GameObject survivorDisplay => AssetLoader.mainAssetBundle.LoadAsset<GameObject>("mdlWyattCSS");

        public override GameObject survivorMdl => AssetLoader.mainAssetBundle.LoadAsset<GameObject>("mdlWyattAIC");

        public override Color survivorDefColor => new Color(0.4862745098f, 0.94901960784f, 0.71764705882f);

        public override Sprite defaultSkinColor => LoadoutAPI.CreateSkinIcon(CCUtilities.HexToColor("00A86B"), CCUtilities.HexToColor("E56717"), CCUtilities.HexToColor("D9DDDC"), CCUtilities.HexToColor("43464B"));

        public override Sprite masterySkinColor => LoadoutAPI.CreateSkinIcon(CCUtilities.HexToColor("00A86B"), CCUtilities.HexToColor("E56717"), CCUtilities.HexToColor("D9DDDC"), CCUtilities.HexToColor("43464B"));

        public override string SurvivorSubtitle => "Lean, Mean, Cleaning Machines";

        public override UnlockableDef unlockableDef => null;

        public override float desiredSortPosition => 11.5f;

        #endregion
    
        public BuffDef wyattCombatDef;

        public BuffDef wyattFlowDef;

        public static SkillDef throwPrimary;

        public override void Hooks()
        {
            base.Hooks();

        }

        public override void GenerateUmbra()
        {
            base.GenerateUmbra();
            umbraMaster = BandaidConvert.Resources.Load<GameObject>("prefabs/charactermasters/LoaderMonsterMaster").InstantiateClone(BodyName + "MonsterMaster", true);
            CCUtilities.RegisterNewMaster(umbraMaster);

            umbraMaster.GetComponent<CharacterMaster>().bodyPrefab = survivorBody;
        }

        public override Material GetMasteryMat()
        {
            return BandaidConvert.Resources.Load<GameObject>("Prefabs/CharacterBodies/BrotherGlassBody").GetComponentInChildren<CharacterModel>().baseRendererInfos[0].defaultMaterial;
        }

        #region Item/Equipment Displays
        public override void GenerateEquipmentDisplays(List<ItemDisplayRuleSet.KeyAssetRuleGroup> obj)
        {
            base.GenerateEquipmentDisplays(obj);

        }

        public override void GenerateItemDisplays(List<ItemDisplayRuleSet.KeyAssetRuleGroup> obj)
        {
            base.GenerateItemDisplays(obj);

            /*childName = "UpperBody",
localPos = new Vector3(0.0019F, 0.0019F, -0.0046F),
localAngles = new Vector3(60.9874F, 341.0948F, 359.8671F),
localScale = new Vector3(0.008F, 0.008F, 0.005F)*/
            obj.Add(ItemDisplayLoader.CreateGenericEquipmentDisplayRule(RoR2Content.Equipment.Jetpack, "DisplayBugWings", "UpperBody", new Vector3(0.0019F, 0.0019F, -0.0046F), new Vector3(60.9874F, 341.0948F, 359.8671F), new Vector3(0.005f, 0.005f, 0.005f)));
            /*childName = "UpperBody",
localPos = new Vector3(-0.0144F, 0.0148F, -0.0039F),
localAngles = new Vector3(325.9973F, 81.0325F, 332.6911F),
localScale = new Vector3(0.005F, 0.005F, 0.005F)*/
            obj.Add(ItemDisplayLoader.CreateGenericEquipmentDisplayRule(RoR2Content.Equipment.GoldGat, "DisplayGoldGat", "UpperBody", new Vector3(-0.0144F, 0.0148F, -0.0039F), new Vector3(325.9973F, 81.0325F, 332.6911F), new Vector3(0.005F, 0.005F, 0.005F)));
            /*childName = "UpperBody",
localPos = new Vector3(-0.0096F, 0.007F, 0.0044F),
localAngles = new Vector3(358.4925F, 342.8776F, 30.949F),
localScale = new Vector3(0.01F, 0.01F, 0.01F)*/
            obj.Add(ItemDisplayLoader.CreateGenericEquipmentDisplayRule(RoR2Content.Equipment.BFG, "DisplayBFG", "UpperBody", new Vector3(-0.0096F, 0.007F, 0.0044F), new Vector3(358.4925F, 342.8776F, 30.949F), new Vector3(0.01f, 0.01f, 0.01f)));
            /*childName = "UpperBody",
localPos = new Vector3(0.006F, 0.0058F, -0.0014F),
localAngles = new Vector3(357.2348F, 86.3632F, 7.6717F),
localScale = new Vector3(0.01F, 0.01F, 0.01F)*/
            obj.Add(ItemDisplayLoader.CreateGenericEquipmentDisplayRule(RoR2Content.Equipment.QuestVolatileBattery, "DisplayBatteryArray", "UpperBody", new Vector3(0.006F, 0.0058F, -0.0014F), new Vector3(357.2348F, 86.3632F, 7.6717F), new Vector3(0.01f, 0.01f, 0.01f)));
            /*childName = "UpperBody",
localPos = new Vector3(0.0014F, 0.0092F, -0.0021F),
localAngles = new Vector3(55.7399F, 171.457F, 6.6232F),
localScale = new Vector3(0.02F, 0.02F, 0.02F)*/
            obj.Add(ItemDisplayLoader.CreateGenericEquipmentDisplayRule(RoR2Content.Equipment.CommandMissile, "DisplayMissileRack", "UpperBody", new Vector3(0.0014F, 0.0092F, -0.0021F), new Vector3(55.7399F, 171.457F, 6.6232F), new Vector3(0.02f, 0.02f, 0.02f)));
            /*childName = "LowerBody",
localPos = new Vector3(0.0082F, 0.0017F, -0.0058F),
localAngles = new Vector3(3.2258F, 25.4977F, 22.0889F),
localScale = new Vector3(0.01F, 0.01F, 0.01F)*/
            obj.Add(ItemDisplayLoader.CreateGenericEquipmentDisplayRule(RoR2Content.Equipment.Fruit, "DisplayFruit", "LowerBody", new Vector3(0.0082F, 0.0017F, -0.0058F), new Vector3(3.2258F, 25.4977F, 22.0889F), new Vector3(0.01f, 0.01f, 0.01f)));
            /*childName = "Head",
localPos = new Vector3(0.0098F, 0.0012F, -0.0002F),
localAngles = new Vector3(9.6844F, 91.5572F, 181.5758F),
localScale = new Vector3(0.001F, 0.001F, 0.001F)*/
            obj.Add(ItemDisplayLoader.CreateGenericEquipmentDisplayRule(RoR2Content.Equipment.AffixBlue, "DisplayEliteIceCrown", "Head", new Vector3(0.0098F, 0.0012F, -0.0002F), new Vector3(9.6844F, 91.5572F, 181.5758F), new Vector3(0.001f, 0.001f, 0.001f)));
            obj.Add(ItemDisplayLoader.CreateGenericEquipmentDisplayRule(RoR2Content.Equipment.AffixPoison, "DisplayEliteUrchinCrown", "Head", new Vector3(0.0098F, 0.0012F, -0.0002F), new Vector3(9.6844F, 91.5572F, 181.5758F), new Vector3(0.001f, 0.001f, 0.001f)));
            obj.Add(ItemDisplayLoader.CreateGenericEquipmentDisplayRule(RoR2Content.Equipment.AffixHaunted, "DisplayEliteStealthCrown", "Head", new Vector3(0.0098F, 0.0012F, -0.0002F), new Vector3(9.6844F, 91.5572F, 181.5758F), new Vector3(0.001f, 0.001f, 0.001f)));
            /*childName = "Head",
localPos = new Vector3(0.003F, 0.0122F, 0.0044F),
localAngles = new Vector3(74.8949F, 241.3595F, 156.7753F),
localScale = new Vector3(0.01F, 0.01F, 0.01F)*/
            obj.Add(ItemDisplayLoader.CreateGenericEquipmentDisplayRule(RoR2Content.Equipment.CritOnUse, "DisplayNeuralImplant", "Head", new Vector3(0.003F, 0.0122F, 0.0044F), new Vector3(74.8949F, 241.3595F, 156.7753F), new Vector3(0.01f, 0.01f, 0.01f)));
            obj.Add(ItemDisplayLoader.CreateGenericEquipmentDisplayRule(RoR2Content.Equipment.DroneBackup, "DisplayRadio", "Pelvis", new Vector3(0, 0, 0.008f), new Vector3(0, 0, 180), new Vector3(0.01f, 0.01f, 0.01f)));
            //obj.Add(ItemDisplayLoader.CreateGenericDisplayRule("Lightning", ItemDisplays.capacitorPrefab, "ClavicleL", new Vector3(0, 0, 0), new Vector3(0, 0, 0), new Vector3(0.04f, 0.04f, 0.04f)));
            /*childName = "Pelvis",
localPos = new Vector3(0F, -0.0004F, 0.003F),
localAngles = new Vector3(0F, 0F, 180F),
localScale = new Vector3(0.002F, 0.002F, 0.002F)*/
            obj.Add(ItemDisplayLoader.CreateGenericEquipmentDisplayRule(RoR2Content.Equipment.BurnNearby, "DisplayPotion", "Pelvis", new Vector3(0F, -0.0004F, 0.003F), new Vector3(0, 0, 180), new Vector3(0.002f, 0.002f, 0.002f)));
            obj.Add(ItemDisplayLoader.CreateGenericEquipmentDisplayRule(RoR2Content.Equipment.CrippleWard, "DisplayEffigy", "Pelvis", new Vector3(0, 0.008f, 0.009f), new Vector3(0, 180, 180), new Vector3(0.02f, 0.02f, 0.02f)));
            obj.Add(ItemDisplayLoader.CreateGenericEquipmentDisplayRule(RoR2Content.Equipment.GainArmor, "DisplayElephantFigure", "UpperLeg.R", new Vector3(0.004f, 0.012f, 0), new Vector3(90, 90, 0), new Vector3(0.02f, 0.02f, 0.02f)));
            /*childName = "UpperBody",
localPos = new Vector3(0.0085F, 0.0066F, 0.0046F),
localAngles = new Vector3(357.8228F, 278.9436F, 329.11F),
localScale = new Vector3(0.002F, 0.002F, 0.002F)*/
            obj.Add(ItemDisplayLoader.CreateGenericEquipmentDisplayRule(RoR2Content.Equipment.Recycle, "DisplayRecycler", "UpperBody", new Vector3(0.0085F, 0.0066F, 0.0046F), new Vector3(357.8228F, 278.9436F, 329.11F), new Vector3(0.005f, 0.005f, 0.005f)));
            obj.Add(ItemDisplayLoader.CreateGenericEquipmentDisplayRule(RoR2Content.Equipment.FireBallDash, "DisplayEgg", "Pelvis", new Vector3(0, 0, 0.008f), new Vector3(90, 0, 0), new Vector3(0.01f, 0.01f, 0.01f)));
            /*childName = "UpperBody",
localPos = new Vector3(-0.0017F, 0.0009F, -0.0041F),
localAngles = new Vector3(353.7988F, 169.6067F, 357.2388F),
localScale = new Vector3(0.003F, 0.003F, 0.003F)*/
            obj.Add(ItemDisplayLoader.CreateGenericEquipmentDisplayRule(RoR2Content.Equipment.Cleanse, "DisplayWaterPack", "UpperBody", new Vector3(-0.0017F, 0.0009F, -0.0041F), new Vector3(353.7988F, 169.6067F, 357.2388F), new Vector3(0.005f, 0.005f, 0.005f)));
            obj.Add(ItemDisplayLoader.CreateGenericEquipmentDisplayRule(RoR2Content.Equipment.Tonic, "DisplayTonic", "Pelvis", new Vector3(0, 0, 0.008f), new Vector3(0, 0, 180), new Vector3(0.01f, 0.01f, 0.01f)));
            obj.Add(ItemDisplayLoader.CreateGenericEquipmentDisplayRule(RoR2Content.Equipment.Gateway, "DisplayVase", "Pelvis", new Vector3(0, 0, 0.009f), new Vector3(0, 0, 180), new Vector3(0.01f, 0.01f, 0.01f)));
            /*childName = "UpperBody",
localPos = new Vector3(0.0054F, 0.0081F, 0.0012F),
localAngles = new Vector3(278.6485F, 141.6753F, 207.3689F),
localScale = new Vector3(0.005F, 0.005F, 0.005F)*/
            obj.Add(ItemDisplayLoader.CreateGenericEquipmentDisplayRule(RoR2Content.Equipment.Scanner, "DisplayScanner", "Pelvis", new Vector3(0.0054F, 0.0081F, 0.0012F), new Vector3(278.6485F, 141.6753F, 207.3689F), new Vector3(0.005f, 0.005f, 0.005f)));
            obj.Add(ItemDisplayLoader.CreateGenericEquipmentDisplayRule(RoR2Content.Equipment.DeathProjectile, "DisplayDeathProjectile", "Pelvis", new Vector3(-0.0012f, 0.005f, 0), new Vector3(0, 270, 180), new Vector3(0.005f, 0.005f, 0.005f)));
            /*childName = "Head",
localPos = new Vector3(0.0034F, 0.0079F, 0.002F),
localAngles = new Vector3(45F, 180F, 0F),
localScale = new Vector3(0.005F, 0.005F, 0.005F)*/
            obj.Add(ItemDisplayLoader.CreateGenericEquipmentDisplayRule(RoR2Content.Equipment.LifestealOnHit, "DisplayLifestealOnHit", "Head", new Vector3(0.0034F, 0.0079F, 0.002F), new Vector3(45F, 180F, 0F), new Vector3(0.005f, 0.005f, 0.005f)));
            obj.Add(ItemDisplayLoader.CreateGenericEquipmentDisplayRule(RoR2Content.Equipment.TeamWarCry, "DisplayTeamWarCry", "Pelvis", new Vector3(0.009672F, 0.213739F, -0.717588F), new Vector3(18.79459F, 176.7826F, 1.458205F), new Vector3(0.2F, 0.2F, 0.2F)));

            obj.Add(ItemDisplayLoader.CreateFollowerDisplayRule(RoR2Content.Equipment.Saw, "DisplaySawmerang", new Vector3(0.06f, 0.02f, 0), new Vector3(90, 0, 0), new Vector3(0.25f, 0.25f, 0.25f)));
            obj.Add(ItemDisplayLoader.CreateFollowerDisplayRule(RoR2Content.Equipment.Meteor, "DisplayMeteor", new Vector3(0.05f, 0.02f, 0), new Vector3(90, 0, 0), new Vector3(1, 1, 1)));
            obj.Add(ItemDisplayLoader.CreateFollowerDisplayRule(RoR2Content.Equipment.Blackhole, "DisplayGravCube", new Vector3(0.05f, 0.02f, 0), new Vector3(90, 0, 0), new Vector3(1, 1, 1)));


            /*childName = "Head",
localPos = new Vector3(0.0142F, 0.6222F, 0.5267F),
localAngles = new Vector3(273.3598F, 75.2117F, 107.0705F),
localScale = new Vector3(0.8F, 0.8F, 0.8F)*/

            /*done*/
            obj.Add(ItemDisplayLoader.CreateGenericItemDisplayRule(RoR2Content.Items.CritGlasses, "DisplayGlasses", "Head", new Vector3(0.0142F, 0.6222F, 0.5267F), new Vector3(273.3598F, 75.2117F, 107.0705F), new Vector3(0.8F, 0.8F, 0.8F)));
            /*childName = "LowerBody",
localPos = new Vector3(-0.0058F, 0.0037F, 0.0015F),
localAngles = new Vector3(69.0152F, 187.046F, 261.7581F)
localScale = new Vector3(0.005F, 0.005F, 0.005F)*/
            obj.Add(ItemDisplayLoader.CreateGenericItemDisplayRule(RoR2Content.Items.Syringe, "DisplaySyringeCluster", "Neck", new Vector3(-0.220076F, 0.290355F, 0.128224F), new Vector3(53.72817F, 232.3132F, 328.412F), new Vector3(0.5F, 0.5F, 0.5F)));
            /*childName = RoR2Content.Items.Broom3",
 localPos = new Vector3(0.0003F, -0.0182F, 0.0001F),
 localAngles = new Vector3(0F, 0F, 0F),
 localScale = new Vector3(0.005F, 0.005F, 0.005F)*/
            obj.Add(ItemDisplayLoader.CreateGenericItemDisplayRule(RoR2Content.Items.NearbyDamageBonus, "DisplayDiamond", "GyroBall", new Vector3(0.014831F, 0.006201F, 0.010713F), new Vector3(333.5044F, 123.4042F, 17.2751F), new Vector3(0.25F, 0.25F, 0.25F)));
            /*childName = RoR2Content.Items.Broom3",
localPos = new Vector3(0.0023F, 0.0571F, 0.0006F),
localAngles = new Vector3(274.0766F, 66.9896F, 295.4782F),
localScale = new Vector3(0.0175F, 0.0175F, 0.0175F)*/
            /*odd*/
            obj.Add(ItemDisplayLoader.CreateGenericItemDisplayRule(RoR2Content.Items.ArmorReductionOnHit, "DisplayWarhammer", "Broom3", new Vector3(0.009617F, 3.189145F, -0.043662F), new Vector3(271.2599F, 228.7159F, 130.642F), new Vector3(1F, 1F, 1F)));
            /*childName = RoR2Content.Items.Broom3",
localPos = new Vector3(0.0009F, 0F, 0.0012F),
localAngles = new Vector3(70F, 0F, 180F),
localScale = new Vector3(0.003F, 0.003F, 0.003F)*/
            /*weird*/
            obj.Add(ItemDisplayLoader.CreateGenericItemDisplayRule(RoR2Content.Items.SecondarySkillMagazine, "DisplayDoubleMag", "Broom3", new Vector3(0.0009F, 0F, 0.0012F), new Vector3(70, 0, 180), new Vector3(0.003F, 0.003F, 0.003F)));
            /*childName = "Zipper",
localPos = new Vector3(0.0001F, 0.0009F, -0.0018F),
localAngles = new Vector3(347.2675F, 171.081F, 164.3253F),
localScale = new Vector3(0.005F, 0.005F, 0.005F)*/

            obj.Add(ItemDisplayLoader.CreateGenericItemDisplayRule(RoR2Content.Items.Bear, "DisplayBear", "UpperBody", new Vector3(0.102427F, 1.343844F, 0.119726F), new Vector3(12.28595F, 174.9624F, 355.2533F), new Vector3(1.022359F, 1.121412F, 1.022359F)));
            /*childName = "Pelvis",
localPos = new Vector3(0.0002F, 0.0028F, 0.0036F),
localAngles = new Vector3(18.9127F, 281.0396F, 184.2924F),
localScale = new Vector3(0.0175F, 0.0175F, 0.0175F)*/
            obj.Add(ItemDisplayLoader.CreateGenericItemDisplayRule(RoR2Content.Items.SprintOutOfCombat, "DisplayWhip", "Pelvis", new Vector3(0.0002F, 0.0028F, 0.0036F), new Vector3(18.9127F, 281.0396F, 184.2924F), new Vector3(0.0175F, 0.0175F, 0.0175F)));
            /*childName = "Pelvis",
localPos = new Vector3(-0.009F, 0.0029F, -0.0014F),
localAngles = new Vector3(82.4872F, 303.8977F, 214.3752F),
localScale = new Vector3(0.005F, 0.005F, 0.005F)*/
            obj.Add(ItemDisplayLoader.CreateGenericItemDisplayRule(RoR2Content.Items.PersonalShield, "DisplayShieldGenerator", "Pelvis", new Vector3(-0.009F, 0.0029F, -0.0014F), new Vector3(82.4872F, 303.8977F, 214.3752F), new Vector3(0.005F, 0.005F, 0.005F)));
            /*childName = "UpperBody",
localPos = new Vector3(-0.01F, 0.007F, -0.0022F),
localAngles = new Vector3(315.0769F, 218.2874F, 137.894F),
localScale = new Vector3(0.0075F, 0.0075F, 0.0075F)*/
            obj.Add(ItemDisplayLoader.CreateGenericItemDisplayRule(RoR2Content.Items.FlatHealth, "DisplaySteakCurved", "UpperBody", new Vector3(-0.01F, 0.007F, -0.0022F), new Vector3(315.0769F, 218.2874F, 137.894F), new Vector3(0.0075F, 0.0075F, 0.0075F)));
            /*childName = "Pelvis",
localPos = new Vector3(-0.0029F, 0.0048F, 0.0042F),
localAngles = new Vector3(65.5398F, 187.609F, 6.2366F),
localScale = new Vector3(0.002F, 0.002F, 0.002F)*/
            obj.Add(ItemDisplayLoader.CreateGenericItemDisplayRule(RoR2Content.Items.FireballsOnHit, "DisplayFireballsOnHit", "Pelvis", new Vector3(-0.0029F, 0.0048F, 0.0042F), new Vector3(65.5398F, 187.609F, 6.2366F), new Vector3(0.002F, 0.002F, 0.002F)));
            /*childName = "LowerLeg.L",
localPos = new Vector3(-0.0034F, 0.0118F, 0.0006F),
localAngles = new Vector3(76.5328F, 91.3411F, 1.3874F),
localScale = new Vector3(0.006F, 0.005F, 0.005F)*/
            obj.Add(ItemDisplayLoader.CreateGenericItemDisplayRule(RoR2Content.Items.Hoof, "DisplayHoof", "LowerLeg.L", new Vector3(-0.0034F, 0.0118F, 0.0006F), new Vector3(76.5328F, 91.3411F, 1.3874F), new Vector3(0.006F, 0.005F, 0.005F)));
            /*childName = "UpperBody",
localPos = new Vector3(0.0008F, 0.0123F, -0.0035F),
localAngles = new Vector3(4.9346F, 351.0688F, 91.633F),
localScale = new Vector3(0.0175F, 0.0175F, 0.0175F)*/
            obj.Add(ItemDisplayLoader.CreateGenericItemDisplayRule(RoR2Content.Items.WardOnLevel, "DisplayWarbanner", "UpperBody", new Vector3(0.0008F, 0.0123F, -0.0035F), new Vector3(4.9346F, 351.0688F, 91.633F), new Vector3(0.0175f, 0.0175f, 0.0175f)));
            /*childName = "LowerArm.R",
localPos = new Vector3(-0.1096F, -0.0194F, -0.0507F),
localAngles = new Vector3(90F, 90F, 0F),
localScale = new Vector3(1F, 1F, 1F)*/
            obj.Add(ItemDisplayLoader.CreateGenericItemDisplayRule(RoR2Content.Items.BarrierOnOverHeal, "DisplayAegis", "LowerArm.R", new Vector3(-0.1096F, -0.0194F, -0.0507F), new Vector3(90, 90, 0), new Vector3(1F, 1F, 1F)));
            /*childName = "UpperBody",
localPos = new Vector3(0.0011F, 0.0031F, -0.0047F),
localAngles = new Vector3(285.629F, 202.3053F, 143.9489F),
localScale = new Vector3(0.025F, 0.025F, 0.025F)*/
            obj.Add(ItemDisplayLoader.CreateGenericItemDisplayRule(RoR2Content.Items.WarCryOnMultiKill, "DisplayPauldron", "Collar.R", new Vector3(-0.143662F, 0.519012F, -0.012151F), new Vector3(359.2599F, 308.4473F, 145.5516F), new Vector3(0.75F, 0.75F, 0.75F)));
            obj.Add(ItemDisplayLoader.CreateGenericItemDisplayRule(RoR2Content.Items.SprintArmor, "DisplayBuckler", "LowerArm.R", new Vector3(0.002f, 0.005f, 0), new Vector3(0, 90, 0), new Vector3(0.01f, 0.01f, 0.01f)));
            /*childName = "GyroRing",
localPos = new Vector3(0F, 0F, 0F),
localAngles = new Vector3(270F, 90F, 0F),
localScale = new Vector3(0.02F, 0.0175F, 0.0175F)*/
            obj.Add(ItemDisplayLoader.CreateGenericItemDisplayRule(RoR2Content.Items.IceRing, "DisplayIceRing", "GyroRing", new Vector3(0, 0, 0), new Vector3(270, 90, 0), new Vector3(0.02f, 0.0175f, 0.0175f)));
            /*childName = "GyroBall",
localPos = new Vector3(0F, 0F, 0F),
localAngles = new Vector3(270F, 90F, 0F),
localScale = new Vector3(0.02F, 0.0175F, 0.0175F)*/
            obj.Add(ItemDisplayLoader.CreateGenericItemDisplayRule(RoR2Content.Items.FireRing, "DisplayFireRing", "GyroBall", new Vector3(0, 0, 0), new Vector3(270, 90, 0), new Vector3(0.02f, 0.0175f, 0.0175f)));
            /*childName = "Broom3",
localPos = new Vector3(-0.0499F, -3.9765F, 0.0416F),
localAngles = new Vector3(0F, 0F, 0F),
localScale = new Vector3(0.2F, 0.2F, 0.2F)*/
            obj.Add(ItemDisplayLoader.CreateGenericItemDisplayRule(RoR2Content.Items.Behemoth, "DisplayBehemoth", "Broom3", new Vector3(-0.0499F, -3.9765F, 0.0416F), new Vector3(0, 0, 0), new Vector3(0.2F, 0.2F, 0.2F)));
            /*childName = "UpperBody",
localPos = new Vector3(-0.0016F, 0.0146F, 0.0144F),
localAngles = new Vector3(4.3116F, 270.9612F, 339.7851F),
localScale = new Vector3(0.002F, 0.002F, 0.002F)*/
            obj.Add(ItemDisplayLoader.CreateGenericItemDisplayRule(RoR2Content.Items.Missile, "DisplayMissileLauncher", "UpperBody", new Vector3(-0.0016F, 0.0146F, 0.0144F), new Vector3(4.3116F, 270.9612F, 339.7851F), new Vector3(0.002F, 0.002F, 0.002F)));
            /*childName = "UpperBody",
localPos = new Vector3(0.9364F, 0.4605F, -0.356F),
localAngles = new Vector3(298.7396F, 143.3624F, 142.3565F),
localScale = new Vector3(3F, 3F, 3F)*/
            obj.Add(ItemDisplayLoader.CreateGenericItemDisplayRule(RoR2Content.Items.Dagger, "DisplayDagger", "UpperBody", new Vector3(0.9364F, 0.4605F, -0.356F), new Vector3(298.7396F, 143.3624F, 142.3565F), new Vector3(3F, 3F, 3F)));
            /*childName = "Broom3",
localPos = new Vector3(0.0356F, 0.0923F, 0.2279F),
localAngles = new Vector3(0.5132F, 185.9516F, 183.0593F),
localScale = new Vector3(6F, 6F, 6F)*/
            obj.Add(ItemDisplayLoader.CreateGenericItemDisplayRule(RoR2Content.Items.ChainLightning, "DisplayUkulele", "Broom3", new Vector3(0.0356F, 0.0923F, 0.2279F), new Vector3(0.5132F, 185.9516F, 183.0593F), new Vector3(6F, 6F, 6F)));
            /*childName = "Head",
localPos = new Vector3(0.03F, 0.4189F, 0.5159F),
localAngles = new Vector3(275.36F, 198.4852F, 342.2571F),
localScale = new Vector3(1.7F, 1.7F, 1.7F)*/
            obj.Add(ItemDisplayLoader.CreateGenericItemDisplayRule(RoR2Content.Items.GhostOnKill, "DisplayMask", "Head", new Vector3(0.03F, 0.4189F, 0.5159F), new Vector3(275.36F, 198.4852F, 342.2571F), new Vector3(1.7F, 1.7F, 1.7F)));
            /*childName = "Collar.L",
localPos = new Vector3(0F, 0.0121F, -0.0034F),
localAngles = new Vector3(349.4365F, 123.2711F, 75.3896F),
localScale = new Vector3(0.0035F, 0.0035F, 0.0035F)*/
            obj.Add(ItemDisplayLoader.CreateGenericItemDisplayRule(RoR2Content.Items.Mushroom, "DisplayMushroom", "Collar.L", new Vector3(0F, 0.0121F, -0.0034F), new Vector3(349.4365F, 123.2711F, 75.3896F), new Vector3(0.0035f, 0.0035f, 0.0035f)));
            /*childName = "Head",
localPos = new Vector3(0.0002F, 0.0026F, 0.0105F),
localAngles = new Vector3(287.2566F, 341.5681F, 194.5766F),
localScale = new Vector3(0.015F, 0.015F, 0.015F)*/
            obj.Add(ItemDisplayLoader.CreateGenericItemDisplayRule(RoR2Content.Items.AttackSpeedOnCrit, "DisplayWolfPelt", "Collar.L", new Vector3(0.000794F, 0.753816F, 0.147341F), new Vector3(277.3531F, 237.849F, 304.8341F), new Vector3(1.279801F, 1.228338F, 1.310545F)));
            /*childName = "Broom3",
localPos = new Vector3(-0.1287F, 1.4563F, -0.4076F),
localAngles = new Vector3(296.7932F, 199.4901F, 194.4085F),
localScale = new Vector3(2F, 2F, 2F)*/
            obj.Add(ItemDisplayLoader.CreateGenericItemDisplayRule(RoR2Content.Items.BleedOnHit, "DisplayTriTip", "Broom3", new Vector3(-0.1287F, 1.4563F, -0.4076F), new Vector3(296.7932F, 199.4901F, 194.4085F), new Vector3(2F, 2F, 2F)));
            /*childName = "Hand.R",
localPos = new Vector3(0.0008F, 0.002F, -0.0023F),
localAngles = new Vector3(275.5979F, 11.7498F, 57.0921F),
localScale = new Vector3(0.01F, 0.01F, 0.01F)*/
            obj.Add(ItemDisplayLoader.CreateGenericItemDisplayRule(RoR2Content.Items.HealOnCrit, "DisplayScythe", "Hand.R", new Vector3(0.0008F, 0.002F, -0.0023F), new Vector3(275.5979F, 11.7498F, 57.0921F), new Vector3(0.01F, 0.01F, 0.01F)));
            /*childName = "UpperBody",
localPos = new Vector3(0.0107F, 0.0053F, 0.0016F),
localAngles = new Vector3(25.7662F, 357.8116F, 19.4318F),
localScale = new Vector3(0.0035F, 0.0035F, 0.0035F)*/
            obj.Add(ItemDisplayLoader.CreateGenericItemDisplayRule(RoR2Content.Items.HealWhileSafe, "DisplaySnail", "Head", new Vector3(-0.009932F, 0.156171F, 0.986855F), new Vector3(278.7867F, 299.9488F, 241.61F), new Vector3(0.25F, 0.25F, 0.25F)));
            /*childName = "UpperBody",
localPos = new Vector3(0.001F, 0.0083F, -0.0033F),
localAngles = new Vector3(284.1014F, 42.8073F, 280.8434F),
localScale = new Vector3(0.02F, 0.02F, 0.02F)*/
            obj.Add(ItemDisplayLoader.CreateGenericItemDisplayRule(RoR2Content.Items.Clover, "DisplayClover", "Head", new Vector3(-0.230584F, 0.513138F, 0.823247F), new Vector3(56.75823F, 333.8656F, 357.9174F), new Vector3(1F, 1F, 1F)));
            /*childName = "Head",
localPos = new Vector3(-0.0008F, 0.0013F, 0.0084F),
localAngles = new Vector3(294.6417F, 30.514F, 144.012F),
localScale = new Vector3(0.035F, 0.035F, 0.035F)*/
            obj.Add(ItemDisplayLoader.CreateGenericItemDisplayRule(RoR2Content.Items.GoldOnHit, "DisplayBoneCrown", "Head", new Vector3(-0.0008F, 0.0013F, 0.0084F), new Vector3(294.6417F, 30.514F, 144.012F), new Vector3(0.035f, 0.035f, 0.035f)));
            /*childName = "Head",
localPos = new Vector3(0.0037F, -0.0006F, -0.0046F),
localAngles = new Vector3(298.2337F, 52.7536F, 108.3291F),
localScale = new Vector3(0.025F, 0.025F, 0.025F)*/

            obj.Add(ItemDisplayLoader.CreateGenericItemDisplayRule(RoR2Content.Items.JumpBoost, "DisplayWaxBird", "Head", new Vector3(0.0037F, -0.0006F, -0.0046F), new Vector3(298.2337F, 52.7536F, 108.3291F), new Vector3(0.025f, 0.025f, 0.025f)));
            obj.Add(ItemDisplayLoader.CreateGenericItemDisplayRule(RoR2Content.Items.ArmorPlate, "DisplayRepulsionArmorPlate", "Pelvis", new Vector3(-0.9153F, 0.2416F, -0.1152F), new Vector3(82.5424F, 125.5413F, 50.2729F), new Vector3(0.8492F, 0.8492F, 0.8492F)));
            /*childName = "LowerArm.L",
localPos = new Vector3(0.0019F, 0.0034F, -0.0008F),
localAngles = new Vector3(0F, 0F, 270F),
localScale = new Vector3(0.0005F, 0.0005F, 0.0005F)*/
            obj.Add(ItemDisplayLoader.CreateGenericItemDisplayRule(RoR2Content.Items.Feather, "DisplayFeather", "LowerArm.L", new Vector3(0.0019F, 0.0034F, -0.0008F), new Vector3(0F, 0F, 270F), new Vector3(0.0005F, 0.0005F, 0.0005F)));
            /*childName = "UpperBody",
localPos = new Vector3(0.1194F, 0.1915F, -0.5017F),
localAngles = new Vector3(6.2101F, 269.2011F, 6.8961F),
localScale = new Vector3(1F, 1F, 1F)*/
            obj.Add(ItemDisplayLoader.CreateGenericItemDisplayRule(RoR2Content.Items.Crowbar, "DisplayCrowbar", "UpperBody", new Vector3(0.1194F, 0.1915F, -0.5017F), new Vector3(6.2101F, 269.2011F, 6.8961F), new Vector3(1F, 1F, 1F)));
            /*childName = "Head",
localPos = new Vector3(0.0015F, -0.001F, -0.0006F),
localAngles = new Vector3(81.1622F, 236.9245F, 62.1139F),
localScale = new Vector3(0.01F, 0.01F, 0.01F)*/
            obj.Add(ItemDisplayLoader.CreateGenericItemDisplayRule(RoR2Content.Items.ExecuteLowHealthElite, "DisplayGuillotine", "Head", new Vector3(0.0015F, -0.001F, -0.0006F), new Vector3(81.1622F, 236.9245F, 62.1139F), new Vector3(0.01F, 0.01F, 0.01F)));
            /*childName = "UpperBody",
localPos = new Vector3(0.0006F, -0.0028F, -0.0038F),
localAngles = new Vector3(358.8678F, 83.5013F, 309.2877F),
localScale = new Vector3(0.007F, 0.007F, 0.007F)*/
            obj.Add(ItemDisplayLoader.CreateGenericItemDisplayRule(RoR2Content.Items.EquipmentMagazine, "DisplayBattery", "Pelvis", new Vector3(-0.436813F, 0.472419F, 0.357303F), new Vector3(85.98849F, 320.2064F, 79.23889F), new Vector3(0.437541F, 0.42765F, 0.43462F)));
            /*childName = "Pelvis",
localPos = new Vector3(-0.0055F, 0.0025F, 0.0033F),
localAngles = new Vector3(0.2051F, 342.8358F, 1.3584F),
localScale = new Vector3(0.02F, 0.02F, 0.02F)*/
            obj.Add(ItemDisplayLoader.CreateGenericItemDisplayRule(RoR2Content.Items.Infusion, "DisplayInfusion", "Pelvis", new Vector3(-0.0055F, 0.0025F, 0.0033F), new Vector3(0.2051F, 342.8358F, 1.3584F), new Vector3(0.02f, 0.02f, 0.02f)));
            /*childName = "UpperBody",
localPos = new Vector3(0.0003F, 0.0074F, -0.0031F),
localAngles = new Vector3(279.5177F, 16.8457F, 149.1503F),
localScale = new Vector3(0.03F, 0.03F, 0.03F))*/
            obj.Add(ItemDisplayLoader.CreateGenericItemDisplayRule(RoR2Content.Items.Medkit, "DisplayMedkit", "UpperBody", new Vector3(0.0003F, 0.0074F, -0.0031F), new Vector3(279.5177F, 16.8457F, 149.1503F), new Vector3(0.03F, 0.03F, 0.03F)));
            /*childName = "UpperBody",
localPos = new Vector3(-0.0191F, 0.0391F, 0.0896F),
localAngles = new Vector3(292.1157F, 12.8333F, 165.8401F),
localScale = new Vector3(2F, 2F, 2F)*/
            obj.Add(ItemDisplayLoader.CreateGenericItemDisplayRule(RoR2Content.Items.Bandolier, "DisplayBandolier", "UpperBody", new Vector3(-0.0191F, 0.0391F, 0.0896F), new Vector3(292.1157F, 12.8333F, 165.8401F), new Vector3(2F, 2F, 2F)));
            /*childName = "Head",
localPos = new Vector3(-0.5151F, 0.1651F, -0.3034F),
localAngles = new Vector3(25.9458F, 219.2616F, 193.0679F),
localScale = new Vector3(1F, 1F, 1F)*/
            obj.Add(ItemDisplayLoader.CreateGenericItemDisplayRule(RoR2Content.Items.BounceNearby, "DisplayHook", "Head", new Vector3(0.0043F, 0.0008F, 0.0057F), new Vector3(299.2878F, 359.7645F, 186.9263F), new Vector3(0.02f, 0.02f, 0.02f)));
            obj.Add(ItemDisplayLoader.CreateGenericItemDisplayRule(RoR2Content.Items.StunChanceOnHit, "DisplayStunGrenade", "UpperLeg.R", new Vector3(-0.005f, 0.01f, 0), new Vector3(90, 270, 0), new Vector3(0.03f, 0.03f, 0.03f)));
            /*childName = "Pelvis",
localPos = new Vector3(0.0054F, 0.0055F, 0.0039F),
localAngles = new Vector3(87.3338F, 214.1456F, 111.7661F),
localScale = new Vector3(0.02F, 0.02F, 0.02F)*/
            obj.Add(ItemDisplayLoader.CreateGenericItemDisplayRule(RoR2Content.Items.IgniteOnKill, "DisplayGasoline", "Pelvis", new Vector3(0.0054F, 0.0055F, 0.0039F), new Vector3(87.3338F, 214.1456F, 111.7661F), new Vector3(0.02f, 0.02f, 0.02f)));
            /*childName = "Pelvis",
localPos = new Vector3(-0.006F, -0.0027F, 0.0034F),
localAngles = new Vector3(90F, 0F, 0F),
localScale = new Vector3(0.01F, 0.01F, 0.01F)*/
            obj.Add(ItemDisplayLoader.CreateGenericItemDisplayRule(RoR2Content.Items.Firework, "DisplayFirework", "Pelvis", new Vector3(-0.006F, -0.0027F, 0.0034F), new Vector3(90, 0, 0), new Vector3(0.01f, 0.01f, 0.01f)));
            obj.Add(ItemDisplayLoader.CreateGenericItemDisplayRule(RoR2Content.Items.LunarDagger, "DisplayLunarDagger", "UpperBody", new Vector3(0.01f, 0.002f, 0), new Vector3(290, 90, 0), new Vector3(0.03f, 0.03f, 0.03f)));
            /*childName = "UpperBody",
localPos = new Vector3(0.0018F, -0.0118F, -0.0011F),
localAngles = new Vector3(4.2873F, 315.2964F, 6.5038F),
localScale = new Vector3(0.0035F, 0.0035F, 0.0035F)*/
            obj.Add(ItemDisplayLoader.CreateGenericItemDisplayRule(RoR2Content.Items.Knurl, "DisplayKnurl", "UpperBody", new Vector3(0.0018F, -0.0118F, -0.0011F), new Vector3(4.2873F, 315.2964F, 6.5038F), new Vector3(0.0035f, 0.0035f, 0.0035f)));
            /*childName = "UpperBody",
localPos = new Vector3(1.2898F, 0.8175F, 0.2931F),
localAngles = new Vector3(277.3013F, 204.3983F, 22.0124F),
localScale = new Vector3(0.3F, 0.3F, 0.3F)*/
            obj.Add(ItemDisplayLoader.CreateGenericItemDisplayRule(RoR2Content.Items.BeetleGland, "DisplayBeetleGland", "UpperBody", new Vector3(1.2898F, 0.8175F, 0.2931F), new Vector3(277.3013F, 204.3983F, 22.0124F), new Vector3(0.3F, 0.3F, 0.3F)));
            obj.Add(ItemDisplayLoader.CreateGenericItemDisplayRule(RoR2Content.Items.SprintBonus, "DisplaySoda", "Pelvis", new Vector3(0.72736F, 0.258266F, -0.482206F), new Vector3(272.8019F, 53.99409F, 55.91787F), new Vector3(1F, 1F, 1F)));
            obj.Add(ItemDisplayLoader.CreateGenericItemDisplayRule(RoR2Content.Items.StickyBomb, "DisplayStickyBomb", "Pelvis", new Vector3(0.0025f, 0.002f, -0.008f), new Vector3(345, 15, 0), new Vector3(0.01f, 0.01f, 0.01f)));
            /*childName = "Pelvis",
localPos = new Vector3(0.005F, 0.0064F, -0.0058F),
localAngles = new Vector3(4.7991F, 60.5039F, 259.4F),
localScale = new Vector3(0.03F, 0.03F, 0.03F)*/
            obj.Add(ItemDisplayLoader.CreateGenericItemDisplayRule(RoR2Content.Items.TreasureCache, "DisplayKey", "Pelvis", new Vector3(0.622186F, 0.265435F, 0.222264F), new Vector3(3.57456F, 319.2377F, 266.4469F), new Vector3(3F, 3F, 3F)
));
            /*childName = "Pelvis",
localPos = new Vector3(0.0008F, 0.0014F, 0.0043F),
localAngles = new Vector3(90F, 0F, 0F),
localScale = new Vector3(0.02F, 0.02F, 0.02F)*/
            obj.Add(ItemDisplayLoader.CreateGenericItemDisplayRule(RoR2Content.Items.BossDamageBonus, "DisplayAPRound", "Pelvis", new Vector3(-0.42113F, 0.167473F, -0.632915F), new Vector3(87.39136F, 286.4641F, 253.5351F), new Vector3(1.33F, 1.5F, 1.66F)));
            /*childName = "UpperBody",
localPos = new Vector3(-0.0049F, 0.008F, 0.0064F),
localAngles = new Vector3(356.7661F, 338.3447F, 0.434F),
localScale = new Vector3(0.005F, 0.005F, 0.005F))*/
            obj.Add(ItemDisplayLoader.CreateGenericItemDisplayRule(RoR2Content.Items.ExtraLife, "DisplayHippo", "Zipper", new Vector3(-0.0049F, 0.008F, 0.0064F), new Vector3(356.7661F, 338.3447F, 0.434F), new Vector3(0.005F, 0.005F, 0.005F)));
            /*childName = "Head",
localPos = new Vector3(-0.0031F, 0F, 0.0066F),
localAngles = new Vector3(81.1442F, 227.1479F, 208.2072F),
localScale = new Vector3(0.009F, 0.009F, 0.009F)*/
            obj.Add(ItemDisplayLoader.CreateGenericItemDisplayRule(RoR2Content.Items.KillEliteFrenzy, "DisplayBrainstalk", "Head", new Vector3(-0.0031F, 0F, 0.0066F), new Vector3(81.1442F, 227.1479F, 208.2072F), new Vector3(0.006f, 0.006f, 0.006f)));
            /*childName = "UpperBody",
localPos = new Vector3(-0.0046F, 0.0089F, -0.0023F),
localAngles = new Vector3(340.1515F, 302.2997F, 98.361F),
localScale = new Vector3(0.01F, 0.01F, 0.01F)*/
            obj.Add(ItemDisplayLoader.CreateGenericItemDisplayRule(RoR2Content.Items.RepeatHeal, "DisplayCorpseFlower", "UpperBody", new Vector3(-0.0046F, 0.0089F, -0.0023F), new Vector3(340.1515F, 302.2997F, 98.361F), new Vector3(0.01f, 0.01f, 0.01f)));
            /*childName = "UpperBody",
localPos = new Vector3(-0.5791F, -1.0927F, 0.3493F),
localAngles = new Vector3(17.207F, 49.0485F, 13.9471F),
localScale = new Vector3(1F, 1F, 1F)*/
            obj.Add(ItemDisplayLoader.CreateGenericItemDisplayRule(RoR2Content.Items.AutoCastEquipment, "DisplayFossil", "UpperBody", new Vector3(-0.5791F, -1.0927F, 0.3493F), new Vector3(17.207F, 49.0485F, 13.9471F), new Vector3(1F, 1F, 1F)));
            /*childName = "UpperBody",
localPos = new Vector3(-0.0013F, 0.0027F, 0.0113F),
localAngles = new Vector3(351.5573F, 306.0773F, 13.2031F),
localScale = new Vector3(0.0085F, 0.0085F, 0.0085F)*/
            obj.Add(ItemDisplayLoader.CreateGenericItemDisplayRule(RoR2Content.Items.TitanGoldDuringTP, "DisplayGoldHeart", "UpperBody", new Vector3(-0.0013F, 0.0027F, 0.0113F), new Vector3(351.5573F, 306.0773F, 13.2031F), new Vector3(0.0085f, 0.0085f, 0.0085f)));
            obj.Add(ItemDisplayLoader.CreateGenericItemDisplayRule(RoR2Content.Items.SprintWisp, "DisplayBrokenMask", "ShoulderL", new Vector3(0.005f, 0.003f, 0), new Vector3(0, 90, 180), new Vector3(0.01f, 0.01f, 0.01f)));
            /*childName = "UpperBody",
localPos = new Vector3(0.0037F, 0.0047F, 0.0079F),
localAngles = new Vector3(74.094F, 148.379F, 143.1946F),
localScale = new Vector3(0.015F, 0.015F, 0.015F)*/
            obj.Add(ItemDisplayLoader.CreateGenericItemDisplayRule(RoR2Content.Items.BarrierOnKill, "DisplayBrooch", "Head", new Vector3(0.194496F, 0.572236F, 0.932347F), new Vector3(5.009361F, 162.6576F, 25.99005F), new Vector3(1.196984F, 1.196984F, 1.196984F)));
            obj.Add(ItemDisplayLoader.CreateGenericItemDisplayRule(RoR2Content.Items.TPHealingNova, "DisplayGlowFlower", "UpperBody", new Vector3(-0.0055f, 0.003f, -0.004f), new Vector3(0, 250, 0), new Vector3(0.01f, 0.01f, 0.01f)));
            //childName = "Pelvis",
            //            localPos = new Vector3(0.0071F, 0.0051F, -0.0029F),
            //localAngles = new Vector3(0F, 180F, 0F),
            //localScale = new Vector3(0.02F, 0.02F, 0.02F)
            /*childName = "Head",
 localPos = new Vector3(0.012F, 0.0056F, 0.0001F),
 localAngles = new Vector3(25.9458F, 158.9222F, 341.5057F),
 localScale = new Vector3(0.02F, 0.02F, 0.02F)*/
            obj.Add(ItemDisplayLoader.CreateGenericItemDisplayRule(RoR2Content.Items.LunarUtilityReplacement, "DisplayBirdFoot", "Head", new Vector3(0.012F, 0.0056F, 0.0001F), new Vector3(25.9458F, 158.9222F, 341.5057F), new Vector3(0.02F, 0.02F, 0.02F)));
            /*childName = "Broom3",
localPos = new Vector3(-0.0013F, 0.0045F, -0.0011F),
localAngles = new Vector3(272.9714F, 289.2923F, 68.729F),
localScale = new Vector3(0.02F, 0.02F, 0.025F)*/
            obj.Add(ItemDisplayLoader.CreateGenericItemDisplayRule(RoR2Content.Items.Thorns, "DisplayRazorwireLeft", "Broom3", new Vector3(-0.0013F, 0.0045F, -0.0011F), new Vector3(272.9714F, 289.2923F, 68.729F), new Vector3(0.02f, 0.02f, 0.025f)));
            obj.Add(ItemDisplayLoader.CreateGenericItemDisplayRule(RoR2Content.Items.LunarPrimaryReplacement, "DisplayBirdEye", "Head", new Vector3(-0.003f, 0.0025f, 0), new Vector3(0, 0, 270), new Vector3(0.01f, 0.01f, 0.01f)));
            /*childName = "UpperBody",
localPos = new Vector3(-0.0007F, 0.0078F, 0.0028F),
localAngles = new Vector3(317.0402F, 358.8969F, 8.588F),
localScale = new Vector3(0.005F, 0.005F, 0.005F)*/
            obj.Add(ItemDisplayLoader.CreateGenericItemDisplayRule(RoR2Content.Items.NovaOnLowHealth, "DisplayJellyGuts", "UpperBody", new Vector3(-0.0007F, 0.0078F, 0.0028F), new Vector3(317.0402F, 358.8969F, 8.588F), new Vector3(0.005f, 0.005f, 0.005f)));

            obj.Add(ItemDisplayLoader.CreateGenericItemDisplayRule(RoR2Content.Items.LunarTrinket, "DisplayBeads", "LowerArm.L", new Vector3(0.002f, 0.004f, 0.0015f), new Vector3(0, 90, 90), new Vector3(0.04f, 0.04f, 0.04f)));
            /*childName = "Head",
localPos = new Vector3(0.0006F, 0.0048F, 0.0018F),
localAngles = new Vector3(285.2283F, 47.5475F, 213.363F),
localScale = new Vector3(0.003F, 0.003F, 0.003F)*/
            obj.Add(ItemDisplayLoader.CreateGenericItemDisplayRule(RoR2Content.Items.Plant, "DisplayInterstellarDeskPlant", "Head", new Vector3(0.0006F, 0.0048F, 0.0018F), new Vector3(285.2283F, 47.5475F, 213.363F), new Vector3(0.004F, 0.004F, 0.004F)));
            /*childName = "Head",
localPos = new Vector3(-0.0148F, 0.4678F, 1.3055F),
localAngles = new Vector3(355.7947F, 359.3278F, 176.6593F),
localScale = new Vector3(0.2F, 0.2F, 0.2F)*/
            obj.Add(ItemDisplayLoader.CreateGenericItemDisplayRule(RoR2Content.Items.DeathMark, "DisplayDeathMark", "Head", new Vector3(-0.0148F, 0.4678F, 1.3055F), new Vector3(355.7947F, 359.3278F, 176.6593F), new Vector3(0.2F, 0.2F, 0.2F)));
            //     obj.Add(ItemDisplayLoader.CreateGenericItemDisplayRule(RoR2Content.Items.CooldownOnCrit, "DisplaySkull", "Hand.R", new Vector3(0, 0.005f, 0), new Vector3(270, 90, 0), new Vector3(0.0085f, 0.0085f, 0.01f)));
            /*childName = RoR2Content.Items.Broom3",
localPos = new Vector3(-0.0002F, -0.0465F, 0.0003F),
localAngles = new Vector3(0F, 0F, 0F),
localScale = new Vector3(0.035F, 0.035F, 0.035F)*/
            obj.Add(ItemDisplayLoader.CreateGenericItemDisplayRule(RoR2Content.Items.UtilitySkillMagazine, "DisplayAfterburnerShoulderRing", "Broom3", new Vector3(-0.0002F, -0.0465F, 0.0003F), new Vector3(0F, 0F, 0F), new Vector3(0.035f, 0.035f, 0.035f)));
            /*childName = "Pelvis",
localPos = new Vector3(0.0006F, 0.0027F, -0.0081F),
localAngles = new Vector3(7.0044F, 134.3362F, 177.5309F),
localScale = new Vector3(0.0021F, 0.0021F, 0.0021F)*/
            obj.Add(ItemDisplayLoader.CreateGenericItemDisplayRule(RoR2Content.Items.ExplodeOnDeath, "DisplayWilloWisp", "Pelvis", new Vector3(0.0006F, 0.0027F, -0.0081F), new Vector3(7.0044F, 134.3362F, 177.5309F), new Vector3(0.0021f, 0.0021f, 0.0021f)));
            obj.Add(ItemDisplayLoader.CreateGenericItemDisplayRule(RoR2Content.Items.Seed, "DisplaySeed", "LowerArm.R", new Vector3(0, 0.005f, 0), new Vector3(270, 0, 0), new Vector3(0.0025f, 0.0025f, 0.0025f)));
            obj.Add(ItemDisplayLoader.CreateGenericItemDisplayRule(RoR2Content.Items.Phasing, "DisplayStealthkit", "UpperLeg.L", new Vector3(-0.004f, 0.01f, 0), new Vector3(90, 90, 0), new Vector3(0.01f, 0.01f, 0.01f)));
            /*childName = "UpperBody",
localPos = new Vector3(0.0143F, 0.0075F, 0.0017F),
localAngles = new Vector3(14.8723F, 358.7363F, 327.3503F),
localScale = new Vector3(0.02F, 0.02F, 0.02F)*/
            obj.Add(ItemDisplayLoader.CreateGenericItemDisplayRule(RoR2Content.Items.ShockNearby, "DisplayTeslaCoil", "UpperBody", new Vector3(0.0143F, 0.0075F, 0.0017F), new Vector3(14.8723F, 358.7363F, 327.3503F), new Vector3(0.02F, 0.02F, 0.02F)));
            /*childName = "Hand.R",
localPos = new Vector3(-0.0028F, -0.0026F, 0.0008F),
localAngles = new Vector3(312.3487F, 109.3963F, 155.8183F),
localScale = new Vector3(0.05F, 0.05F, 0.05F)*/
            obj.Add(ItemDisplayLoader.CreateGenericItemDisplayRule(RoR2Content.Items.AlienHead, "DisplayAlienHead", "Hand.R", new Vector3(-0.0028F, -0.0026F, 0.0008F), new Vector3(312.3487F, 109.3963F, 155.8183F), new Vector3(2, 2, 2)));
            /*childName = "Head",
localPos = new Vector3(0.0007F, 0.0031F, -0.0019F),
localAngles = new Vector3(285.5032F, 34.1144F, 136.7536F),
localScale = new Vector3(0.015F, 0.005F, 0.005F)*/
            obj.Add(ItemDisplayLoader.CreateGenericItemDisplayRule(RoR2Content.Items.HeadHunter, "DisplaySkullCrown", "Head", new Vector3(0.002729F, 0.219027F, 0.831527F), new Vector3(288.2999F, 1.47202F, 178.4848F), new Vector3(1.431602F, 0.592387F, 0.069496F)));
            /*childName = "Pelvis",
localPos = new Vector3(-0.3487F, 0.3723F, 0.4773F),
localAngles = new Vector3(349.9248F, 48.4294F, 82.9856F),
localScale = new Vector3(1F, 1F, 1F)*/
            obj.Add(ItemDisplayLoader.CreateGenericItemDisplayRule(RoR2Content.Items.EnergizedOnEquipmentUse, "DisplayWarHorn", "Pelvis", new Vector3(-0.3487F, 0.3723F, 0.4773F), new Vector3(349.9248F, 48.4294F, 82.9856F), new Vector3(1F, 1F, 1F)));
            obj.Add(ItemDisplayLoader.CreateGenericItemDisplayRule(RoR2Content.Items.Tooth, "DisplayToothMeshLarge", "UpperBody", new Vector3(0, 0.012f, 0.005f), new Vector3(290, 0, 0), new Vector3(0.3f, 0.3f, 0.3f)));
            obj.Add(ItemDisplayLoader.CreateGenericItemDisplayRule(RoR2Content.Items.Pearl, "DisplayPearl", "Head", new Vector3(0.0082F, 0.0018F, 0F), new Vector3(346.5741F, 98.3489F, 347.4611F), new Vector3(0.005f, 0.005f, 0.005f)));
            /*childName = "Head",
localPos = new Vector3(0.0082F, 0.0018F, 0F),
localAngles = new Vector3(346.5741F, 98.3489F, 347.4611F),
localScale = new Vector3(0.005F, 0.005F, 0.005F)*/
            obj.Add(ItemDisplayLoader.CreateGenericItemDisplayRule(RoR2Content.Items.ShinyPearl, "DisplayShinyPearl", "Head", new Vector3(0.0082F, 0.0018F, 0F), new Vector3(346.5741F, 98.3489F, 347.4611F), new Vector3(0.005f, 0.005f, 0.005f)));
            /*childName = "Pelvis",
localPos = new Vector3(-0.0052F, 0.1523F, -0.7167F),
localAngles = new Vector3(7.4822F, 202.5525F, 9.9656F),
localScale = new Vector3(0.19F, 0.19F, 0.19F)*/
            obj.Add(ItemDisplayLoader.CreateGenericItemDisplayRule(RoR2Content.Items.BonusGoldPackOnKill, "DisplayTome", "Pelvis", new Vector3(-0.0051F, 0.0033F, -0.007F), new Vector3(7.9428F, 200.4164F, 14.3652F), new Vector3(0.0025f, 0.0025f, 0.0025f)));
            /*childName = "Head",
localPos = new Vector3(-0.0001F, -0.0008F, 0.0042F),
localAngles = new Vector3(353.4194F, 178.8901F, 185.1167F),
localScale = new Vector3(0.002F, 0.002F, 0.002F)*/
            obj.Add(ItemDisplayLoader.CreateGenericItemDisplayRule(RoR2Content.Items.Squid, "DisplaySquidTurret", "Head", new Vector3(-0.0001F, -0.0008F, 0.0042F), new Vector3(353.4194F, 178.8901F, 185.1167F), new Vector3(0.002F, 0.002F, 0.002F)));
            /*childName = RoR2Content.Items.Broom3",
localPos = new Vector3(0.0074F, -0.041F, 0.0349F),
localAngles = new Vector3(0F, 0F, 0F),
localScale = new Vector3(0.015F, 0.015F, 0.015F)*/
            obj.Add(ItemDisplayLoader.CreateGenericItemDisplayRule(RoR2Content.Items.LaserTurbine, "DisplayLaserTurbine", "Broom3", new Vector3(0.0074F, -0.041F, 0.0349F), new Vector3(0, 90, 0), new Vector3(0.015f, 0.015f, 0.015f)));
            //      obj.Add(ItemDisplayLoader.CreateGenericItemDisplayRule(RoR2Content.Items.Incubator, "DisplayAncestralIncubator", "BroomModel", new Vector3(0, 0.012f, 0), new Vector3(90, 0, 0), new Vector3(0.0035f, 0.0035f, 0.0035f)));
            obj.Add(ItemDisplayLoader.CreateGenericItemDisplayRule(RoR2Content.Items.SiphonOnLowHealth, "DisplaySiphonOnLowHealth", "Pelvis", new Vector3(-0.006f, 0.004f, 0.006f), new Vector3(0, 315, 180), new Vector3(0.0035f, 0.0035f, 0.0035f)));
            /*childName = "UpperLeg.R",
localPos = new Vector3(0.4207F, 0.5024F, 0.0078F),
localAngles = new Vector3(0F, 0F, 0F),
localScale = new Vector3(0.2F, 0.2F, 0.2F)*/
            obj.Add(ItemDisplayLoader.CreateGenericItemDisplayRule(RoR2Content.Items.BleedOnHitAndExplode, "DisplayBleedOnHitAndExplode", "UpperLeg.R", new Vector3(0.4207F, 0.5024F, 0.0078F), new Vector3(0, 0, 0), new Vector3(0.2F, 0.2F, 0.2F)));
            obj.Add(ItemDisplayLoader.CreateGenericItemDisplayRule(RoR2Content.Items.MonstersOnShrineUse, "DisplayMonstersOnShrineUse", "UpperLeg.L", new Vector3(-0.005f, 0.005f, 0.002f), new Vector3(90, 0, 0), new Vector3(0.002f, 0.002f, 0.002f)));
            obj.Add(ItemDisplayLoader.CreateGenericItemDisplayRule(RoR2Content.Items.RandomDamageZone, "DisplayRandomDamageZone", "Hand.L", new Vector3(0.002f, 0.005f, 0.001f), new Vector3(0, 270, 270), new Vector3(0.002f, 0.002f, 0.002f)));

            //weird rules here
            obj.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2Content.Items.IncreaseHealing,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplayLoader.LoadDisplay("DisplayAntler"),
childName = "Head",
localPos = new Vector3(-0.0035F, 0.002F, 0.0048F),
localAngles = new Vector3(343.7933F, 143.7717F, 326.4241F),
localScale = new Vector3(0.015F, 0.015F, -0.015F),
                            limbMask = LimbFlags.None
                        },
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplayLoader.LoadDisplay("DisplayAntler"),
childName = "Head",
localPos = new Vector3(0.0029F, 0.0013F, 0.0053F),
localAngles = new Vector3(3.6792F, 24.5194F, 306.1763F),
localScale = new Vector3(0.015F, 0.015F, 0.015F),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });
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

            //fuck you and your vars and args god
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
        #endregion

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
            characterBody.baseNameToken = SurvivorInternalName + "_BODY_NAME"; //The base name token. 
            characterBody.subtitleNameToken = SurvivorInternalName + "_BODY_SUBTITLE"; //Set this if its a boss
            characterBody.baseRegen = 1.5f; //Base health regen.
            characterBody.bodyFlags = (CharacterBody.BodyFlags.ImmuneToExecutes); ///Base body flags, should be self explanatory 
            characterBody._defaultCrosshairPrefab = characterBody._defaultCrosshairPrefab = Addressables.LoadAssetAsync<GameObject>(CCUtilities.ConvertResourceLoadToGUID("prefabs/characterbodies/huntressbody")).WaitForCompletion().GetComponent<CharacterBody>()._defaultCrosshairPrefab; //The crosshair prefab.
            characterBody.hideCrosshair = false; //Whether or not to hide the crosshair
            characterBody.hullClassification = HullClassification.Human; //The hull classification, usually used for AI
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
            characterBody.preferredPodPrefab = BandaidConvert.Resources.Load<GameObject>("prefabs/networkedobjects/SurvivorPod");
        }

        #region States
        public override void AlterStatemachines(SetStateOnHurt hurt, NetworkStateMachine network)
        {
            base.AlterStatemachines(hurt, network);
            var machine = survivorBody.AddComponent<EntityStateMachine>();
            machine.customName = "MAID";
            machine.initialStateType = new SerializableEntityStateType(typeof(EntityStates.Idle));
            machine.mainStateType = new SerializableEntityStateType(typeof(EntityStates.Idle));

            int l = hurt.idleStateMachine.Length;
            Array.Resize<EntityStateMachine>(ref hurt.idleStateMachine, l + 1);
            hurt.idleStateMachine[l] = machine;

            int l2 = network.stateMachines.Length;
            Array.Resize<EntityStateMachine>(ref network.stateMachines, l2 + 1);
            network.stateMachines[l2] = machine;


            var machine2 = survivorBody.AddComponent<EntityStateMachine>();
            machine2.customName = "SuperMarioJump";
            machine2.initialStateType = new SerializableEntityStateType(typeof(EntityStates.Idle));
            machine2.mainStateType = new SerializableEntityStateType(typeof(EntityStates.Idle));

            l = hurt.idleStateMachine.Length;
            Array.Resize<EntityStateMachine>(ref hurt.idleStateMachine, l + 1);
            hurt.idleStateMachine[l] = machine2;

            l2 = network.stateMachines.Length;
            Array.Resize<EntityStateMachine>(ref network.stateMachines, l2 + 1);
            network.stateMachines[l2] = machine2;
        }
        public override void CreateMainState(EntityStateMachine machine)
        {
            base.CreateMainState(machine);
        }
        public override void CreatePassive(SkillLocator skillLocator)
        {
            var passive = skillLocator.passiveSkill;

            passive.enabled = true;
            passive.skillNameToken = "WYATT_PASSIVE_NAME";
            passive.skillDescriptionToken = "WYATT_PASSIVE_DESCRIPTION";
            passive.keywordToken = "KEYWORD_VELOCITY";
            //passive.icon = AssetsCore.wyattPassive;

            R2API.LanguageAPI.Add(passive.keywordToken, "<style=cKeywordName>Groove</style><style=cSub>Increases movement speed by X%.</style>");
            R2API.LanguageAPI.Add(passive.skillNameToken, "Walkman");
            R2API.LanguageAPI.Add(passive.skillDescriptionToken, "On hit, gain a stack Groove. Lose 2 stacks of Groove every 0.5 seconds after being out of combat for 3 seconds. Groove grants 30% move speed and 25% damage.");

            skillLocator.passiveSkill = passive;

        }
        public override void CreatePrimary(SkillLocator skillLocator, SkillFamily skillFamily)
        {


            Cloudburst.Content.ContentHandler.Loadouts.RegisterEntityState(typeof(WyattBaseMeleeAttack));

            SteppedSkillDef primarySkillDef = ScriptableObject.CreateInstance<SteppedSkillDef>();

            primarySkillDef.activationState = new SerializableEntityStateType(typeof(WyattBaseMeleeAttack));
            primarySkillDef.stepCount = 3;
            primarySkillDef.activationStateMachineName = "Weapon";
            primarySkillDef.baseMaxStock = 1;
            primarySkillDef.baseRechargeInterval = 0f;
            primarySkillDef.beginSkillCooldownOnSkillEnd = false;
            primarySkillDef.canceledFromSprinting = false;
            //primarySkillDef.resetStepsOnIdle = true;
            primarySkillDef.fullRestockOnAssign = true;
            primarySkillDef.interruptPriority = InterruptPriority.Any;
            primarySkillDef.isCombatSkill = true;
            primarySkillDef.mustKeyPress = false;
            primarySkillDef.cancelSprintingOnActivation = false;
            //primarySkillDef.can = false;
            primarySkillDef.rechargeStock = 1;
            primarySkillDef.requiredStock = 1;
            primarySkillDef.stockToConsume = 0;
            primarySkillDef.skillDescriptionToken = "WYATT_PRIMARY_DESCRIPTION";
            primarySkillDef.skillName = "WYATT_PRIMARY_NAME";
            primarySkillDef.skillNameToken = "WYATT_PRIMARY_NAME";
            //primarySkillDef.icon = AssetsCore.wyattPrimary;
            primarySkillDef.keywordTokens = new string[] {
                 "KEYWORD_AGILE",
                 "KEYWORD_WEIGHTLESS",
                 "KEYWORD_SPIKED",
            };

            R2API.LanguageAPI.Add(primarySkillDef.skillNameToken, "G22 Grav-Broom");
            R2API.LanguageAPI.Add(primarySkillDef.skillDescriptionToken, "<style=cIsUtility>Agile</style>. Swing in front for X% damage. [NOT IMPLEMENTED] Every 4th hit <style=cIsDamage>Spikes</style>.");
            //R2API.LanguageAPI.Add(primarySkillDef.keywordTokens[1], "<style=cKeywordName>Weightless</style><style=cSub>Slows and removes gravity from target.</style>");
            R2API.LanguageAPI.Add(primarySkillDef.keywordTokens[2], "<style=cKeywordName>Spiking</style><style=cSub>Forces an enemy to travel downwards, causing a shockwave if they impact terrain.</style>");

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

            Cloudburst.Content.ContentHandler.Loadouts.RegisterEntityState(typeof(TrashOut));

            SkillDef secondarySkillDef = ScriptableObject.CreateInstance<GCETSkillDef>();
            secondarySkillDef.activationState = new SerializableEntityStateType(typeof(TrashOut));
            secondarySkillDef.activationStateMachineName = "Weapon";
            secondarySkillDef.baseMaxStock = 2;
            secondarySkillDef.baseRechargeInterval = 3f;
            secondarySkillDef.beginSkillCooldownOnSkillEnd = true;
            secondarySkillDef.canceledFromSprinting = false;
            secondarySkillDef.fullRestockOnAssign = false;
            secondarySkillDef.interruptPriority = InterruptPriority.Skill;
            secondarySkillDef.isCombatSkill = true;
            secondarySkillDef.mustKeyPress = true;
            secondarySkillDef.cancelSprintingOnActivation = false;
            secondarySkillDef.rechargeStock = 1;
            secondarySkillDef.requiredStock = 1;
            secondarySkillDef.dontAllowPastMaxStocks = true;
            secondarySkillDef.stockToConsume = 1;
            secondarySkillDef.skillDescriptionToken = "WYATT_SECONDARY_DESCRIPTION";
            secondarySkillDef.skillName = "aaa";
            secondarySkillDef.skillNameToken = "WYATT_SECONDARY_NAME";
            //secondarySkillDef.icon = AssetsCore.wyattSecondary;
            secondarySkillDef.keywordTokens = new string[] {
                // "KEYWORD_AGILE",
                // "KEYWORD_CHARGEABLE",
                "KEYWORD_SPIKED"
             };

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
            Cloudburst.Content.ContentHandler.Loadouts.RegisterEntityState(typeof(ActivateFlow));

            SkillDef utilitySkillDef = ScriptableObject.CreateInstance<SkillDef>();
            utilitySkillDef.activationState = new SerializableEntityStateType(typeof(ActivateFlow));
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
        //    utilitySkillDef.icon = AssetsCore.wyattUtility;
            utilitySkillDef.keywordTokens = new string[] {
                 "KEYWORD_RUPTURE",
             };

          

            R2API.LanguageAPI.Add(utilitySkillDef.skillNameToken, "Flow");
            R2API.LanguageAPI.Add(utilitySkillDef.skillDescriptionToken, "Idk if this even works rn tbh.\nActivate Flow for 4 seconds (0.4s for each stack of Groove, max 8 seconds). During flow, you are unable to lose or gain Groove. After Flow ends, lose all stacks groove.");
            R2API.LanguageAPI.Add("KEYWORD_RUPTURE", "<style=cKeywordName>Flow</style><style=cSub> Gives you a double jump. +30% cooldown reduction.</style>");


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
            //Cloudburst.Content.ContentHandler.Loadouts.RegisterEntityState(typeof(Drone));
            Cloudburst.Content.ContentHandler.Loadouts.RegisterEntityState(typeof(DeployMaid));
            Cloudburst.Content.ContentHandler.Loadouts.RegisterEntityState(typeof(RetrieveMaid));

            SkillDef specialSkillDef = ScriptableObject.CreateInstance<CustodianMAIDSkillDef>();
            CCUtilities.CopySkillDefSettings(BandaidConvert.Resources.Load<SkillDef>("skilldefs/loaderbody/FireYankHook"), specialSkillDef);

            //specialSkillDef.stockToConsume = 1;
            specialSkillDef.activationState = Deploy;
            specialSkillDef.activationStateMachineName = "MAID";
            specialSkillDef.baseRechargeInterval = 5;
            specialSkillDef.stockToConsume = 0;
            specialSkillDef.skillDescriptionToken = "WYATT_SPECIAL_DESCRIPTION";
            specialSkillDef.skillNameToken = "WYATT_SPECIAL_NAME";
            //specialSkillDef.icon = AssetsCore.wyattSpecial;
            specialSkillDef.keywordTokens = new string[] {
                 "KEYWORD_WEIGHTLESS"
            };

            //  ProjectileManager.instance.FireProjectile()

            throwPrimary = specialSkillDef;


            // /   retrievePrimary = specialSkillDef2;

            R2API.LanguageAPI.Add(specialSkillDef.skillNameToken, "M88 MAID");
            R2API.LanguageAPI.Add(specialSkillDef.skillDescriptionToken, "Send your MAID unit barreling through enemies for 500% damage before stopping briefly and returning to you, able to hit enemies on the way back. Using this skill again while MAID is deployed reels you to the MAID and rebounds you off of her, bashing into an enemy for X% damage.");
            //   R2API.LanguageAPI.Add(specialSkillDef2.skillNameToken, "Retrival");
            //     R2API.LanguageAPI.Add(specialSkillDef2.skillDescriptionToken, "Throw a winch towards the deployed MAID unit, bringing her back.");


            Cloudburst.Content.ContentHandler.Loadouts.RegisterSkillDef(specialSkillDef);
            //    Cloudburst.Content.ContentHandler.Loadouts.RegisterSkillDef(specialSkillDef2);
            SkillFamily specialSkillFamily = skillLocator.special.skillFamily;

            specialSkillFamily.variants[0] = new SkillFamily.Variant
            {
                skillDef = specialSkillDef,
                viewableNode = new ViewablesCatalog.Node(specialSkillDef.skillNameToken, false, null)
            };
        }
        #endregion

        protected override void Initialization()
        {
            this.wyattCombatDef = new BuffBuilder()
            {
                canStack = true,
                isDebuff = false,
                iconSprite = AssetLoader.mainAssetBundle.LoadAsset<Sprite>("WyattVelocity"),
                buffColor = new Color(1f, 0.7882353f, 0.05490196f)
            }.BuildBuff();

            this.wyattFlowDef = new BuffBuilder()
            {
                canStack = false,
                isDebuff = false,
                iconSprite = AssetLoader.mainAssetBundle.LoadAsset<Sprite>("WyattFlow"),
                buffColor = CCUtilities.HexToColor("#37323e"),
            }.BuildBuff();

            MAIDManager janniePower = survivorBody.AddComponent<MAIDManager>();
            SfxLocator sfxLocator = survivorBody.GetComponent<SfxLocator>();
            //CharacterDeathBehavior characterDeathBehavior = wyattBody.GetComponent<CharacterDeathBehavior>();
            GenericCursorEnemyTracker tracker = survivorBody.AddComponent<GenericCursorEnemyTracker>();
            survivorBody.AddComponent<CustodianWalkmanBehavior>();
            //kil
            //Cloudburst.Content.ContentHandler.Loadouts.RegisterEntityState(typeof(DeathState));
            //characterDeathBehavior.deathState = new SerializableEntityStateType(typeof(DeathState));

            //sfx
            sfxLocator.fallDamageSound = "Play_MULT_shift_hit";
            sfxLocator.landingSound = "play_char_land";

            var indicator = BandaidConvert.Resources.Load<GameObject>("Prefabs/EngiShieldRetractIndicator").transform.Find("Holder").GetComponent<SpriteRenderer>(); //
            indicator.sprite = AssetLoader.mainAssetBundle.LoadAsset<Sprite>("texWyattIndicator");
            indicator.color = CCUtilities.HexToColor("00A86B");

            tracker.maxTrackingAngle = 20;
            tracker.maxTrackingDistance = 55;
            tracker.trackerUpdateFrequency = 5;
            tracker.indicatorPrefab = BandaidConvert.Resources.Load<GameObject>("Prefabs/EngiShieldRetractIndicator");
        }
    }
}
