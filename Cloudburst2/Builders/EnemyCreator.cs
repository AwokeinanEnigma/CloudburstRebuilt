using BepInEx.Configuration;
using Cloudburst;

using RoR2;
using RoR2.Skills;
using System;
using System.Collections.Generic;
using UnityEngine;
namespace Cloudburst.Builders
{
    public abstract class EnemyCreator<T> : EnemyCreator where T : EnemyCreator<T>
    {
        public static T instance { get; private set; }

        public EnemyCreator()
        {
            if (instance != null) throw new InvalidOperationException("Singleton class \"" + typeof(T).Name + "\" was instantiated twice!");
            instance = this as T;
        }
    }
    public abstract class EnemyCreator
    {
        /// <summary>
        /// How much this enemy costs
        /// </summary>
        public abstract int DirectorCost { get; }

        public abstract bool NoElites { get; }
        public abstract bool ForbiddenAsBoss { get; }
        public abstract HullClassification HullClassification { get; }
        public abstract RoR2.Navigation.MapNodeGroup.GraphType GraphType { get; }


        /// <summary>
        /// The lore of the survivor
        /// </summary>
        public abstract string EnemyLore { get; }
        /// <summary>
        /// The survivor's name
        /// </summary>
        public abstract string EnemyName { get; }
        /// <summary>
        /// The survivor's name that's used in Language tokens.
        /// </summary>
        public abstract string EnemyInternalName { get; }
        /// <summary>
        /// The survivor's internal name.
        /// </summary>
        public abstract string BodyName { get; }

        /// <summary>
        /// The survivor's subtitle.
        /// </summary>
        public abstract string EnemySubtitle { get; }
        /// <summary>
        /// The model used for the enemy.
        /// </summary>
        public abstract GameObject EnemyMdl { get; }

        public PrefabBuilder builder;

        public CharacterSpawnCard characterSpawnCard;

        public GameObject EnemyBody;
        public GameObject EnemyMaster;

        protected abstract void Initialization();



        /// <summary>
        /// Only override when you know what you are doing, or call base.Init()!
        /// </summary>
        /// <param name="config"></param>
        internal virtual void Init(ConfigFile file)
        {
            CreatePrefab();
            Initialization();
            CreateLang();
            SetupCharacterBody(EnemyBody.GetComponent<CharacterBody>());
            AlterStatemachines(EnemyBody.GetComponent<SetStateOnHurt>(), EnemyBody.GetComponent<NetworkStateMachine>());
            CreateSkills();
            CreateMaster();
            GenerateCharacterSpawncard();
            Hooks();
            CloudburstPlugin.instance.PluginStart += Start;
        }

        public virtual void Start()
        {
            CCUtilities.LogD("this might not work");
            builder.GetAdditionalItemDisplays += GenerateItemDisplays;
            builder.GetAdditionalItemDisplays += GenerateEquipmentDisplays;
        }

        public virtual void CreateMaster()
        {


        }

        public virtual void GenerateCharacterSpawncard()
        {
           // On.RoR2.CharacterSpawnCard.Awake += GlobalHooks.CharacterSpawnCard_Awake;
            characterSpawnCard = ScriptableObject.CreateInstance<CharacterSpawnCard>();
           // On.RoR2.CharacterSpawnCard.Awake -= GlobalHooks.CharacterSpawnCard_Awake;
            characterSpawnCard.directorCreditCost = DirectorCost;
            characterSpawnCard.forbiddenAsBoss = ForbiddenAsBoss;
            characterSpawnCard.name = "csc" + EnemyInternalName;
            //characterSpawnCard.forbiddenFlags = RoR2.Navigation.NodeFlags.None;
            characterSpawnCard.hullSize = HullClassification;
            characterSpawnCard.loadout = new SerializableLoadout();
            characterSpawnCard.nodeGraphType = GraphType;
            characterSpawnCard.noElites = NoElites;
            characterSpawnCard.occupyPosition = false;
            characterSpawnCard.prefab = EnemyMaster;
            characterSpawnCard.sendOverNetwork = true;

        }

        public virtual void AlterStatemachines(SetStateOnHurt hurt, NetworkStateMachine network)
        {

        }

        public virtual void SetupCharacterBody(CharacterBody characterBody)
        {

        }

        public abstract Material GetMasteryMat();
        #region Skills 
        public virtual void CreateSkills()
        {
            CCUtilities.CreateEmptySkills(EnemyBody);
            SkillLocator locator = EnemyBody.GetComponent<SkillLocator>();
            CreateMainState(EnemyBody.GetComponent<EntityStateMachine>());
            CreatePassive(locator);
            CreatePrimary(locator, locator.primary.skillFamily);
            CreateSecondary(locator, locator.secondary.skillFamily);
            CreateUtility(locator, locator.utility.skillFamily);
            CreateSpecial(locator, locator.special.skillFamily);
        }

        public virtual void CreateMainState(EntityStateMachine machine)
        {

        }

        public virtual void CreatePassive(SkillLocator locator)
        {

        }

        public virtual void CreatePrimary(SkillLocator skillLocator, SkillFamily skillFamily)
        {

        }
        public virtual void CreateSecondary(SkillLocator skillLocator, SkillFamily skillFamily)
        {

        }
        public virtual void CreateUtility(SkillLocator skillLocator, SkillFamily skillFamily)
        {

        }
        public virtual void CreateSpecial(SkillLocator skillLocator, SkillFamily skillFamily)
        {

        }
        #endregion
        #region PrefabBuilder
        protected void CreatePrefab()
        {
            builder = new PrefabBuilder();
            builder.prefabName = BodyName + "Body";
            builder.masteryAchievementUnlockable = null;
            builder.model = EnemyMdl;
            builder.defaultSkinIcon = null;
            builder.masterySkinIcon = null;
            builder.masterySkinDelegate = GetMasteryMat;
            builder.GetAdditionalRenderInfos += GenerateRenderInfos;

            EnemyBody = builder.CreatePrefab();
        }
        public virtual void GenerateEquipmentDisplays(List<ItemDisplayRuleSet.KeyAssetRuleGroup> obj)
        {
        }
        public virtual void GenerateItemDisplays(List<ItemDisplayRuleSet.KeyAssetRuleGroup> obj)
        {
        }
        public virtual void GenerateRenderInfos(List<CharacterModel.RendererInfo> arg1, Transform arg2)
        {

        }
        #endregion

        protected virtual void CreateLang()
        {
            R2API.LanguageAPI.Add(EnemyInternalName + "_BODY_NAME", EnemyName);
            R2API.LanguageAPI.Add(EnemyInternalName + "_BODY_LORE", EnemyLore);
            R2API.LanguageAPI.Add(EnemyInternalName + "_BODY_SUBTITLE", EnemySubtitle);
        }

        public virtual void Hooks() { }
    }
}