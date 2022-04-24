using EntityStates;
using RoR2;
using RoR2.Projectile;
using UnityEngine;

namespace Cloudburst.CEntityStates.Guardsman
{
    public class Stomp : BaseSkillState
    {
        public static float damageCoefficient = 4f;
        public static float procCoefficient = 1f;
        public static float baseDuration = 1.55f;
        public static float throwForce = 3000f;
        public static float projectileCount = 6f;
        public static float blastForce = 3000f;
        public static float blastRadius = 13f;
        private float duration;
        private float fireTime;
        private bool hasFired;
        private Animator animator;
        private Transform muzzleTransform;
        public override void OnEnter()
        {
            base.OnEnter();
            this.duration = Stomp.baseDuration / this.attackSpeedStat;
            this.fireTime = 0.5f * this.duration;
            base.characterBody.SetAimTimer(2f);
            this.animator = base.GetModelAnimator();
            base.characterDirection.forward = GetAimRay().direction;
            base.PlayAnimation("FullBody, Override", "Stomp", "ThrowBomb.playbackRate", this.duration);

            if (base.modelLocator)
            {
                ChildLocator component = base.modelLocator.modelTransform.GetComponent<ChildLocator>();
                if (component)
                {
                    this.muzzleTransform = component.FindChild("FootL");
                }
            }
        }

        public override void OnExit()
        {
            base.OnExit();
        }

        private void Fire()
        {
            if (!this.hasFired)
            {
                this.hasFired = true;
                Util.PlaySound("EnemyBombThrow", base.gameObject);

                if (base.isAuthority)
                {
                    EffectManager.SpawnEffect(EntityStates.Destructible.ExplosivePotDeath.explosionEffectPrefab, new EffectData
                    {
                        origin = base.transform.position,
                        scale = Stomp.blastRadius
                    }, true);
                    new BlastAttack
                    {
                        attacker = base.gameObject,
                        procChainMask = default(ProcChainMask),
                        impactEffect = EffectIndex.Invalid,
                        losType = BlastAttack.LoSType.NearestHit,
                        damageColorIndex = DamageColorIndex.Default,
                        damageType = DamageType.Generic,
                        procCoefficient = Stomp.procCoefficient,
                        bonusForce = Vector3.up * Stomp.blastForce,
                        baseForce = 1500f,
                        baseDamage = Stomp.damageCoefficient * this.damageStat,
                        falloffModel = BlastAttack.FalloffModel.Linear,
                        radius = Stomp.blastRadius,
                        position = base.transform.position,
                        attackerFiltering = AttackerFiltering.NeverHitSelf,
                        teamIndex = base.GetTeam(),
                        inflictor = base.gameObject,
                        crit = base.RollCrit(),
                    }.Fire();
                }
            }
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();

            if (base.fixedAge >= this.fireTime)
            {
                this.Fire();
            }

            if (base.fixedAge >= this.duration && base.isAuthority)
            {
                this.outer.SetNextStateToMain();
                return;
            }
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.PrioritySkill;
        }
    }
}