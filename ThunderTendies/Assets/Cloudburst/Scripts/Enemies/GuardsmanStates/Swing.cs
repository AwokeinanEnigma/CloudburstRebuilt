using EntityStates;
using RoR2;
using RoR2.Audio;
using RoR2.Projectile;
using UnityEngine;
using UnityEngine.Networking;
using System;

namespace Cloudburst.CEntityStates.Guardsman
{
    public class Swing : BaseSkillState
    {
        public int swingIndex;

        protected string hitboxName = "Swing";

        protected DamageType damageType = DamageType.Generic;
        public static float damageCoefficient = 3f;
        protected float procCoefficient = 1f;
        protected float pushForce = 160f;
        protected Vector3 bonusForce = Vector3.zero;
        public static float baseDuration = 3f;
        private float attackStartTime = 0.573f;
        private float attackEndTime = 0.7067f;
        protected float baseEarlyExitTime = 0.3f;
        protected float hitStopDuration = 0.012f;
        protected float attackRecoil = 0.75f;
        protected float hitHopVelocity = 4f;
        protected bool cancelled = false;
        protected float animDuration;

        protected string swingSoundString = "";
        protected string hitSoundString = "";
        protected string muzzleString = "SwingCenter";
        protected GameObject swingTrailEffect;
        protected GameObject hitEffectPrefab;
        protected NetworkSoundEventIndex impactSound;

        private float earlyExitTime;
        public float duration;
        private bool hasFired;
        private float hitPauseTimer;
        private OverlapAttack attack;
        protected bool inHitPause;
        private bool hasHopped;
        protected float stopwatch;
        protected Animator animator;
        private BaseState.HitStopCachedState hitStopCachedState;
        private Vector3 storedVelocity;

        public static float distanceToTravel = -60f;
        public static float meleeRange = 14f;
        public static float swingAngle = 80f;
        private Transform muzzleTransform;

        public override void OnEnter()
        {
            base.OnEnter();

            attack = new OverlapAttack()
            {
                attacker = base.gameObject,
                attackerFiltering = AttackerFiltering.Default,
                damage = damageCoefficient * base.damageStat,
                damageColorIndex = DamageColorIndex.Default,
                //damageType = DamageTypeCore.antiGrav, //overclock.GetDamageType(),
                //forceVector = new Vector3(500, 0, 500),
                hitEffectPrefab = hitEffectPrefab,
                impactSound = RoR2.Audio.NetworkSoundEventIndex.Invalid,
                inflictor = base.gameObject,
                isCrit = base.RollCrit(),
                maximumOverlapTargets = 100,
                procChainMask = default,
                procCoefficient = 1,
                pushAwayForce = 5000,
                teamIndex = base.characterBody.teamComponent.teamIndex
            };

            Transform modelTransform = base.GetModelTransform();
            if (modelTransform)
            {
                this.attack.hitBoxGroup = Array.Find<HitBoxGroup>(modelTransform.GetComponents<HitBoxGroup>(), (HitBoxGroup element) => element.groupName == "HitboxSwing");
            }

            this.duration = Swing.baseDuration / this.attackSpeedStat;
            this.animDuration = this.duration;
            this.earlyExitTime = this.baseEarlyExitTime / this.attackSpeedStat;

            this.animator = base.GetModelAnimator();
            this.animator.SetFloat("swingPitchCycle", this.animator.GetFloat("aimPitchCycle"));

            this.swingSoundString = "GuardsmanSwing";
            this.hitSoundString = "GuardsmanHit";


            if (base.modelLocator)
            {
                ChildLocator component = base.modelLocator.modelTransform.GetComponent<ChildLocator>();
                if (component)
                {
                    this.muzzleTransform = component.FindChild("Blade");
                }
            }



            //this.hitEffectPrefab = Modules.Assets.swordHitImpactEffect;
            // this.impactSound = Modules.Assets.guardsmanHitSoundEvent.index;

            Util.PlaySound("GuardsmanWindup", base.gameObject);
            this.PlayAttackAnimation();

        }

        protected virtual void PlayAttackAnimation()
        {
            base.PlayAnimation("FullBody, Override", "Swing", "Slash.playbackRate", this.duration * 2f);

        }

        public override void OnExit()
        {
            //if (!this.hasFired && !this.cancelled)

            if (this.swingTrailEffect)
            {
                EntityState.Destroy(this.swingTrailEffect);
            }

            base.OnExit();

            this.animator.SetBool("attacking", false);
        }

        protected virtual void PlaySwingEffect()
        {
            //EffectManager.SimpleMuzzleFlash(Modules.Assets.guardsmanSwingEffect, base.gameObject, "Blade", true);
            //this.swingTrailEffect = UnityEngine.Object.Instantiate<GameObject>(Modules.Assets.guardsmanSwingTrail, this.muzzleTransform, false);
        }

        protected virtual void OnHitEnemyAuthority()
        {
            Util.PlaySound(this.hitSoundString, base.gameObject);

            if (!this.hasHopped)
            {
                if (base.characterMotor && !base.characterMotor.isGrounded && this.hitHopVelocity > 0f)
                {
                    base.SmallHop(base.characterMotor, this.hitHopVelocity);
                }

                this.hasHopped = true;
            }

            if (!this.inHitPause && this.hitStopDuration > 0f)
            {
                this.storedVelocity = base.characterMotor.velocity;
                this.hitStopCachedState = base.CreateHitStopCachedState(base.characterMotor, this.animator, "Slash.playbackRate");
                this.hitPauseTimer = this.hitStopDuration / this.attackSpeedStat;
                this.inHitPause = true;
            }
        }


        private void Fire()
        {
            if (!this.hasFired)
            {
                this.hasFired = true;
                Util.PlayAttackSpeedSound(this.swingSoundString, base.gameObject, this.attackSpeedStat);

                if (base.isAuthority)
                {
                    //Ray aimRay = base.GetAimRay();
                    attack.Fire();
                    //  GameObject projectile = Modules.Projectiles.swingProjectilePrefab;
                    //   //  // MeleeSwingProjectileComponent m = projectile.GetComponent<MeleeSwingProjectileComponent>();
                    //       m.bladeTransform = this.muzzleTransform;

                    this.PlaySwingEffect();
                    base.AddRecoil(-1f * this.attackRecoil, -2f * this.attackRecoil, -0.5f * this.attackRecoil, 0.5f * this.attackRecoil);
                    /*ProjectileManager.instance.FireProjectile(projectile,
                        aimRay.origin,
                        Util.QuaternionSafeLookRotation(aimRay.direction * -1),
                        base.gameObject,
                        damageCoefficient * this.damageStat,
                        1500f,
                        base.RollCrit(),
                        DamageColorIndex.Default);*/
                }
            }
        }

        private void FireAttack()
        {
            if (!this.hasFired)
            {


                if (base.isAuthority)
                {
                    this.Fire();
                }
            }

        }

        protected virtual void SetNextState()
        {
            int index = this.swingIndex;
            if (index == 0) index = 1;
            else index = 0;

            this.outer.SetNextState(new Swing
            {
                swingIndex = index
            });
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();

            this.hitPauseTimer -= Time.fixedDeltaTime;

            if (this.hitPauseTimer <= 0f && this.inHitPause)
            {
                base.ConsumeHitStopCachedState(this.hitStopCachedState, base.characterMotor, this.animator);
                this.inHitPause = false;
                base.characterMotor.velocity = this.storedVelocity;
            }

            if (!this.inHitPause)
            {
                this.stopwatch += Time.fixedDeltaTime;
            }
            else
            {
                if (base.characterMotor) base.characterMotor.velocity = Vector3.zero;
                if (this.animator) this.animator.SetFloat("Swing.playbackRate", 0f);
            }

            if (this.stopwatch >= (this.duration * this.attackStartTime) && this.stopwatch <= (this.duration * this.attackEndTime))
            {
                this.FireAttack();
            }
            if (this.stopwatch >= (this.duration * this.attackEndTime) && this.swingTrailEffect)
            {
                EntityState.Destroy(this.swingTrailEffect);
            }

            if (this.stopwatch >= (this.duration - this.earlyExitTime) && base.isAuthority)
            {
                if (base.inputBank.skill1.down)
                {
                    /*
                    if (!this.hasFired) this.FireAttack();
                    this.SetNextState();
                    return;
                    */
                }
            }

            if (this.stopwatch >= this.duration && base.isAuthority)
            {
                this.outer.SetNextStateToMain();
                return;
            }
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.Frozen;
        }

        public override void OnSerialize(NetworkWriter writer)
        {
            base.OnSerialize(writer);
            writer.Write(this.swingIndex);
        }

        public override void OnDeserialize(NetworkReader reader)
        {
            base.OnDeserialize(reader);
            this.swingIndex = reader.ReadInt32();
        }
    }
}