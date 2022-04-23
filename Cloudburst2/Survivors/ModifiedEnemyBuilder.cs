using Cloudburst;
using Cloudburst.Cores;
using R2API;
using RoR2;
using RoR2.Skills;
using UnityEngine;

internal abstract class ModifiedEnemyBuilder
{
    /// <summary>
    /// The body of the enemy
    /// </summary>
    public GameObject enemyBody;

    /// <summary>
    /// The master of the enemy;
    /// </summary>
    public GameObject enemyMaster;

    /// <summary>
    /// How much this enemy costs
    /// </summary>
    public abstract int DirectorCost { get; }

    public abstract bool NoElites { get; }
    public abstract bool ForbiddenAsBoss { get; }
    public abstract HullClassification HullClassification { get; }
    public abstract RoR2.Navigation.MapNodeGroup.GraphType GraphType { get; }

    //strings

    /// <summary>
    /// The path to the original body
    /// </summary>
    protected abstract string resourceBodyPath { get; }

    /// <summary>
    /// The name of the newly cloned body
    /// </summary>
    protected abstract string bodyName { get; }

    /// <summary>
    /// The path to the original master
    /// </summary>
    protected abstract string resourceMasterPath { get; }


    /// <summary>
    /// Should the body and master be registered?
    /// </summary>
    protected abstract bool registerNetwork { get; }

    public CharacterSpawnCard characterSpawnCard;

    public virtual void Create()
    {
        enemyBody = BandaidConvert.Resources.Load<GameObject>(resourceBodyPath).InstantiateClone(bodyName + "Body", registerNetwork);
        enemyMaster = BandaidConvert.Resources.Load<GameObject>(resourceMasterPath).InstantiateClone(bodyName + "Master", registerNetwork);

        CCUtilities.RegisterNewBody(enemyBody);

        enemyMaster.GetComponent<CharacterMaster>().bodyPrefab = enemyBody;

        OverrideMasterAI(enemyMaster.GetComponent<CharacterMaster>());
        OverrideCharacterbody(enemyBody.GetComponent<CharacterBody>());
        OverrideVisuals(enemyBody.GetComponent<ModelLocator>().modelTransform.GetComponent<CharacterModel>());
        OverrideSkills();

        CCUtilities.RegisterNewMaster(enemyMaster);

        CreateDirectorCard();
    }

    public virtual void CreateDirectorCard()
    {
       // On.RoR2.CharacterSpawnCard.Awake += GlobalHooks.CharacterSpawnCard_Awake;
        characterSpawnCard = ScriptableObject.CreateInstance<CharacterSpawnCard>();
       // On.RoR2.CharacterSpawnCard.Awake -= GlobalHooks.CharacterSpawnCard_Awake;
        characterSpawnCard.directorCreditCost = DirectorCost;
        characterSpawnCard.forbiddenAsBoss = ForbiddenAsBoss;
        characterSpawnCard.name = "csc" + bodyName;
        //characterSpawnCard.forbiddenFlags = RoR2.Navigation.NodeFlags.None;
        characterSpawnCard.hullSize = HullClassification;
        characterSpawnCard.loadout = new SerializableLoadout();
        characterSpawnCard.nodeGraphType = GraphType;
        characterSpawnCard.noElites = NoElites;
        characterSpawnCard.occupyPosition = false;
        characterSpawnCard.prefab = enemyMaster;
        characterSpawnCard.sendOverNetwork = true;
    }

    public virtual void OverrideMasterAI(CharacterMaster master)
    {

    }

    public virtual void OverrideCharacterbody(CharacterBody body)
    {

    }

    public virtual void OverrideVisuals(CharacterModel mdl)
    {

    }

    public virtual void OverrideSkills()
    {
        CCUtilities.CreateEmptySkills(enemyBody);
        SkillLocator locator = enemyBody.GetComponent<SkillLocator>();
        CreatePrimary(locator, locator.primary.skillFamily);
        CreateSecondary(locator, locator.secondary.skillFamily);
        CreateUtility(locator, locator.utility.skillFamily);
        CreateSpecial(locator, locator.special.skillFamily);
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
}