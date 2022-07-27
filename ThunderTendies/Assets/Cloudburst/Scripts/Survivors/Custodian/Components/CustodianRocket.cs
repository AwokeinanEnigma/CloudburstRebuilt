using Cloudburst.Cores;
using EntityStates.Merc;
using EntityStates.Toolbot;
using RoR2;
using RoR2.Orbs;
using RoR2.Skills;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace Cloudburst.Custodian.Components
{
    public class CustodianRocket : MonoBehaviour
    {
        private BasicOwnerInfo info;
        private Vector3 direction;
        private CharacterMotor motor;
        private CharacterDirection characterDirection;
        private OverlapAttack attack = new OverlapAttack();

        public float interval = 0;
        private float stopwatch;
        private bool hit = false;

        private List<HurtBox> victimsStruck = new List<HurtBox>();

        private float hitStopwatch = 0;
        public void Start()
        {
            info = new BasicOwnerInfo(base.gameObject, "");
            direction = info.inputBank.aimDirection;
            motor = info.characterMotor;
            characterDirection = GetComponent<CharacterDirection>();

            attack = new OverlapAttack()
            {
                attacker = base.gameObject,
                attackerFiltering = AttackerFiltering.Default,
                damage = 8 * info.characterBody.damage,
                damageColorIndex = DamageColorIndex.Default,
                //forceVector = new Vector3(0, force, 0),
                //hitEffectPrefab = hitEffectPrefab,
                impactSound = RoR2.Audio.NetworkSoundEventIndex.Invalid,
                inflictor = base.gameObject,
                isCrit = info.characterBody.RollCrit(),
                maximumOverlapTargets = 100,
                procChainMask = default,
                procCoefficient = 1,
                //pushAwayForce = 3500,
                forceVector = direction * 3000,
                hitBoxGroup = CCUtilities.FindHitBoxGroup("TempHitboxLunge", info.characterBody.modelLocator.modelTransform),
                teamIndex = info.characterBody.teamComponent.teamIndex
            };
            //R2API.DamageAPI.AddModdedDamageType(attack, DamageTypeCore.antiGrav);

            motor.onHitGroundServer += Motor_onHitGround;

        }

        private void SetToEmpty()
        {
            var thing = base.GetComponent<ModelLocator>().modelTransform.GetComponent<Animator>();
            thing.speed = 1f;
            thing.Update(0f);
            int layerIndex = thing.GetLayerIndex("Fullbody, Override");
            thing.CrossFadeInFixedTime("BuffEmpty", 0.5f, layerIndex);
        }

        void Motor_onHitGround(ref CharacterMotor.HitGroundInfo hitGroundInfo)
        {
            SetToEmpty();

            EffectManager.SpawnEffect(Effects.wyattSlam, new EffectData
            {
                scale = 10,
                rotation = Quaternion.identity,
                origin = hitGroundInfo.position,
            }, true);

            /*EffectManager.SpawnEffect(BandaidConvert.Resources.Load<GameObject>("prefabs/effects/impacteffects/BeetleQueenDeathImpact")/*Effects.wyattSlam/*BandaidConvert.Resources.Load<GameObject>("prefabs/effects/impacteffects/BeetleGuardGroundSlam"), new EffectData
            {
                scale = 1,
                rotation = Quaternion.identity,
                origin = hitGroundInfo.position,
            }, true);
            EffectManager.SpawnEffect(BandaidConvert.Resources.Load<GameObject>("prefabs/effects/impacteffects/BeetleGuardGroundSlam")/*Effects.wyattSlam/*BandaidConvert.Resources.Load<GameObject>("prefabs/effects/impacteffects/BeetleGuardGroundSlam"), new EffectData
            {
                scale = 1,
                rotation = Quaternion.identity,
                origin = hitGroundInfo.position,
            }, true);*/





            new BlastAttack
            {
                position = hitGroundInfo.position,
                //baseForce = 3000,
                attacker = base.gameObject,
                inflictor = gameObject,
                teamIndex = info.characterBody.teamComponent.teamIndex,
                baseDamage = info.characterBody.damage * 8,
                attackerFiltering = default,
                //bonusForce = new Vector3(0, -3000, 0),
                damageType = DamageType.Stun1s, //| DamageTypeCore.spiked,
                crit = info.characterBody.RollCrit(),
                damageColorIndex = DamageColorIndex.WeakPoint,
                falloffModel = BlastAttack.FalloffModel.None,
                //impactEffect = BandaidConvert.Resources.Load<GameObject>("prefabs/effects/impacteffects/PulverizedEffect").GetComponent<EffectIndex>(),
                procCoefficient = 0,
                radius = 15
            }.Fire();

            var sphere = Physics.OverlapSphere(transform.position, 10);
            foreach (var body in sphere)
            {
                var cb = body.gameObject.GetComponentInParent<CharacterBody>();
                if (cb)
                {
                    bool cannotHit = false;
                    if (cb.isChampion)
                    {
                        cannotHit = true;
                    }
                    if (cb.baseNameToken == "BROTHER_BODY_NAME")
                    {
                        cannotHit = false;
                    }
                    if (cb.characterMotor && cb != info.characterBody && cannotHit == false)
                    {
                        CCUtilities.AddExplosionForce(cb.characterMotor, cb.characterMotor.mass * 25, transform.position, 25, 5, false);
                    }
                }
            }

            motor.onHitGroundServer -= Motor_onHitGround;

            Destroy(this);
        }

        public void OnDestroy()
        {
            motor.onHitGroundServer -= Motor_onHitGround;
        }

        public void FixedUpdate()
        {
            hitStopwatch += Time.fixedDeltaTime;
            stopwatch += Time.fixedDeltaTime;
            if (NetworkServer.active)
            {
                if (stopwatch >= (interval - 0.001f) && hit == false)
                {
                    hit = true;
                    CCUtilities.LogI("force applied at end");
                    motor.ApplyForce((direction * 125 * Assaulter2.speedCoefficient), true, false);
                    characterDirection.forward = motor.rootMotion.normalized;
                }

                if (stopwatch >= interval)
                {
                    SetToEmpty();
                    //		protected void PlayCrossfade(string layerName, string animationStateName, float crossfadeDuration)
                    Destroy(this);
                    //base.PlayCrossfade("Fullbody, Override", "BufferEmpty", 0.5f);
                }

                var wow = (direction * 3 * Assaulter2.speedCoefficient) * Time.fixedDeltaTime;
                motor.rootMotion += wow;
                characterDirection.forward = motor.rootMotion.normalized;

                if (attack.Fire(victimsStruck))
                {
                    motor.Motor.ForceUnground();
                    info.healthComponent.TakeDamageForce(direction * -5000, true, false);
                    EffectManager.SpawnEffect(Effects.ericAndreMoment, new EffectData
                    {
                        scale = 2,
                        rotation = Quaternion.identity,
                        origin = victimsStruck[0].transform.position,
                    }, true);
                    //motor.ApplyForce(-(direction * 125 * Assaulter2.speedCoefficient), true, false);
                    SetToEmpty();
                    //		protected void PlayCrossfade(string layerName, string animationStateName, float crossfadeDuration)
                    Destroy(this);
                }
            }
        }
    }
}
