using RoR2;
using UnityEngine;

public struct BasicOwnerInfo
{        /// <summary>
         /// The owner's gameobject
         /// </summary>
    public readonly GameObject gameObject;

    /// <summary>
    /// The owner's characterBody
    /// </summary>
    public readonly CharacterBody characterBody;

    /// <summary>
    /// The owner's characterMotor
    /// </summary>
    public readonly CharacterMotor characterMotor;

    /// <summary>
    /// The owner's rigidbody
    /// </summary>
    public readonly Rigidbody rigidbody;

    /// <summary>
    /// The owner's health component
    /// </summary>
    public readonly HealthComponent healthComponent;

    /// <summary>
    /// The owner's selected statemachine
    /// </summary>
    public readonly EntityStateMachine stateMachine;

    /// <summary>
    /// If owner has authority
    /// </summary>
    public readonly bool hasEffectiveAuthority;

    /// <summary>
    /// The owner's input bank
    /// </summary>
    public readonly InputBankTest inputBank;
    public BasicOwnerInfo(GameObject ownerGameObject, string targetCustomName)
    {
        this = default(BasicOwnerInfo);
        this.gameObject = ownerGameObject;
        if (this.gameObject)
        {
            this.characterBody = this.gameObject.GetComponent<CharacterBody>();
            inputBank = characterBody.inputBank;
            this.characterMotor = this.gameObject.GetComponent<CharacterMotor>();
            this.rigidbody = this.gameObject.GetComponent<Rigidbody>();
            healthComponent = gameObject.GetComponent<HealthComponent>();
            this.hasEffectiveAuthority = Util.HasEffectiveAuthority(this.gameObject);
            EntityStateMachine[] components = characterBody.GetComponent<SetStateOnHurt>().idleStateMachine;
            for (int i = 0; i < components.Length; i++)
            {
                //LogCore.LogI(components[i]);
                if (components[i].customName == targetCustomName)
                {
                    this.stateMachine = components[i];
                    return;
                }
            }
        }
    }
}