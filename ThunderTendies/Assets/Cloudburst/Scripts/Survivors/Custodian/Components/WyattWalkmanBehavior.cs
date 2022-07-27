using System;
using RoR2;
using RoR2.Stats;
using UnityEngine;
using UnityEngine.Networking;

namespace Cloudburst.Custodian.Components
{
    public class CustodianWalkmanBehavior : NetworkBehaviour, IOnDamageDealtServerReceiver
    {
        private CharacterBody characterBody;
        private ParticleSystem grooveEffect;
        private ParticleSystem grooveEffect2;
        private ChildLocator childLocator;
        private ParticleSystem flowEffect;
        private bool loseStacks { get { return stopwatch >= 3 && !flowing; } }

        private float stopwatch = 0;

        private float drainTimer = 0;

        [SyncVar]
        public bool flowing = false;

        private void Awake()
        {
            characterBody = base.GetComponent<CharacterBody>();
            childLocator = base.gameObject.GetComponentInChildren<ChildLocator>();

            grooveEffect = childLocator.FindChild("MusicEffect1").GetComponent<ParticleSystem>();
            grooveEffect2 = childLocator.FindChild("MusicEffect2").GetComponent<ParticleSystem>();

            flowEffect = childLocator.FindChild("MusicEffect3").GetComponent<ParticleSystem>();
        }

        private void Start()
        {
            GlobalHooks.onFinalBuffStackLost += GlobalHooks_onFinalBuffStackLost;
        }

        private void GlobalHooks_onFinalBuffStackLost(CharacterBody body, BuffDef def)
        {
            if (flowing && NetworkServer.active && characterBody == body)
            {
                //flowing has stopped
                //CCUtilities.SafeRemoveAllOfBuff(Custodian.instance.wyattCombatDef, characterBody);
                flowing = false;

                flowEffect.Stop();
            }
        }

        public void FixedUpdate()
        {
            if (NetworkServer.active)
            {
                //fixedupdate but only on server
                ServerFixedUpdate();

            }
        }

        private void ServerFixedUpdate()
        {
            if (flowing == false)
            {
                stopwatch += Time.fixedDeltaTime;
                if (loseStacks)
                {
                    drainTimer += Time.fixedDeltaTime;
                    if (drainTimer >= 0.5f)
                    {
                      //  CCUtilities.SafeRemoveBuffs(Custodian.instance.wyattCombatDef, characterBody, 2);
                        drainTimer = 0;
                    }
                }
            }

        }

        [Server]
        private void TriggerBehaviorInternal(float stacks)
        {
            var cap = 9 + stacks;
            if (characterBody && characterBody.GetBuffCount(Custodian.instance.wyattCombatDef) < cap)
            {

                grooveEffect2.Play();
                grooveEffect.Play();
                /*EffectManager.SpawnEffect(Effects.wyattGrooveEffect, new EffectData()
                {
                    scale = 1,
                    origin = grooveEffect.transform.position
                }, true);*/
                characterBody.AddBuff(Custodian.instance.wyattCombatDef);
                //characterBody.AddTimedBuff(Custodian.instance.wyattCombatDef, 3);
            }
            stopwatch = 0;
        }



        public void ActivateFlowAuthority()
        {
            if (NetworkServer.active)
            {
                ActivateFlowInternal();
                return;
            }
            CmdActivateFlow();
        }

        [Command]
        private void CmdActivateFlow()
        {
            ActivateFlowInternal();
        }

        [Server]
        private void ActivateFlowInternal()
        {
            int grooveCount = characterBody.GetBuffCount(Custodian.instance.wyattCombatDef);
            float duration = 4;

            for (int i = 0; i < grooveCount; i++)
            {
                //add flow until we can't
                if (duration != 8)
                {
                    duration += 0.4f;
                }
            }

            characterBody.AddTimedBuff(Custodian.instance.wyattFlowDef, duration);
            flowing = true;

            flowEffect.Play();
        }

        public void OnDamageDealtServer(DamageReport damageReport)
        {
            if (damageReport.damageInfo?.inflictor == base.gameObject && flowing == false)
            {
                TriggerBehaviorInternal(1);
            }
        }
    }
}