using UnityEngine;

using RoR2;
using TMPro;
using RoR2.UI;

using UnityEngine.Rendering.PostProcessing;
using System.Linq;
using R2API;
using Cloudburst.Builders;

namespace Cloudburst.Cores
{
    public class Effects : Core
    {
        public static GameObject unknownEffect;
        public static GameObject orbitalImpact;
        public static GameObject HANDRetrivalOrb;
        public static GameObject coinImpactEffect;
        public static GameObject wyattSwingTrail;
        public static GameObject wyatt2SwingTrail;

        public static GameObject magicArmor;
        public static GameObject magicRegen;
        public static GameObject magicAttackSpeed;

        public static GameObject coolEffect;
        public static GameObject reallyCoolEffect;
        public static GameObject trulyCoolEffect;

        public static GameObject maidCleanseEffect;

        public static GameObject maidTouchEffect;

        public static GameObject shaderEffect;

        public static GameObject blackHoleIncisionEffect;

        public static GameObject lumpkinEffect;
        public static GameObject willIsNotPoggers;
        public static GameObject willIsStillTotallyNotPoggers;
        public static GameObject wyattSlam;
        public static GameObject wyattGrooveEffect;

        public static GameObject ericAndreMoment;
        public static GameObject tiredOfTheDingDingDing;

        public static GameObject gooEffect;

        public static GameObject fabinin;

        public override string Name => "Effects";

        public override bool Priority => true;

        public override void OnLoaded()
        {
            CreateNewEffects();

            blackHoleIncisionEffect = CreateAsset("UniversalIncison", false, false, true, "", false, VFXAttributes.VFXIntensity.Medium, VFXAttributes.VFXPriority.Always);
            wyattSlam = CreateEffect("DebugEffect");//, false, false,      true, "", false, VFXAttributes.VFXIntensity.Medium, VFXAttributes.VFXPriority.Medium);
            maidTouchEffect = CreateEffect("TracerCaptainDefenseMatrix");
            shaderEffect = CreateEffect("ShaderTest");
            wyattGrooveEffect = CreateEffect("WyattGrooveEffect");
            ericAndreMoment = CreateEffect("WyattHitEffect");
            tiredOfTheDingDingDing = CreateEffect("WyattSpikeEffect");
            fabinin = CreateEffect("fabin");
        }

        private void CreateMAIDCleanseEffect()
        {
            maidCleanseEffect = CreateAsset("MAIDCleanEffect", false, false, true, "", false, VFXAttributes.VFXIntensity.Medium, VFXAttributes.VFXPriority.Always);
        }

        private void WillIsStillNotPoggers()
        {
            willIsNotPoggers = CreateAsset("WillIsNotPoggers", false, false, true, "", false, VFXAttributes.VFXIntensity.Low, VFXAttributes.VFXPriority.Always);
            var unfortunatelyWillIsStillNotPoggers = willIsNotPoggers.AddComponent<ShakeEmitter>();

            unfortunatelyWillIsStillNotPoggers.wave = new Wave()
            {
                amplitude = 0.5f,
                cycleOffset = 0,
                frequency = 100,
            };
            unfortunatelyWillIsStillNotPoggers.duration = 0.5f;
            unfortunatelyWillIsStillNotPoggers.radius = 51;
            unfortunatelyWillIsStillNotPoggers.amplitudeTimeDecay = true;
        }

        private void WillIsStillNotPoggersMonthsLater()
        {
            willIsStillTotallyNotPoggers = CreateAsset("WyattSuperJumpEffect", false, false, true, "", false, VFXAttributes.VFXIntensity.Low, VFXAttributes.VFXPriority.Always);
            var unfortunatelyWillIsStillNotPoggers = willIsStillTotallyNotPoggers.AddComponent<ShakeEmitter>();

            unfortunatelyWillIsStillNotPoggers.wave = new Wave()
            {
                amplitude = 0.5f,
                cycleOffset = 0,
                frequency = 100,
            };
            unfortunatelyWillIsStillNotPoggers.duration = 0.5f;
            unfortunatelyWillIsStillNotPoggers.radius = 51;
            unfortunatelyWillIsStillNotPoggers.amplitudeTimeDecay = true;
        }

        private void CreateLumpkinEffect()
        {
            lumpkinEffect = CreateAsset("LumpkinScreamEffect", false, false, true, "", false, VFXAttributes.VFXIntensity.Medium, VFXAttributes.VFXPriority.Always);
            var component = lumpkinEffect.AddComponent<ShakeEmitter>();
            var component2 = lumpkinEffect.AddComponent<DestroyOnTimer>();

            component.wave = new Wave()
            {
                amplitude = 0.7f,
                cycleOffset = 0,
                frequency = 100,
            };
            component.duration = 2;
            component.radius = 15;
            component.amplitudeTimeDecay = true;
            component2.duration = 2;
        }

        protected void CreateMagiciansEarringsEffects()
        {
            coolEffect = CreateAsset("HANDHeal", false, false, true, "", false, VFXAttributes.VFXIntensity.Low, VFXAttributes.VFXPriority.Medium);
            reallyCoolEffect = CreateAsset("HANDArmor", false, false, true, "", false, VFXAttributes.VFXIntensity.Medium, VFXAttributes.VFXPriority.Medium);
            trulyCoolEffect = CreateAsset("HANDAttackSpeed", false, false, true, "", false, VFXAttributes.VFXIntensity.Medium, VFXAttributes.VFXPriority.Medium);

        }

        protected void CreateNewEffects()
        {
            //throws an error
            //     MakeHANDOrbEffect();
            //'nother fuckin error
            //  CreateCoinImpactEffect();
            CreateMAIDCleanseEffect();
            //oh no!
            //CreateOrbitalImpact();
            //CreateMagicEffects();
            WillIsStillNotPoggersMonthsLater();
            WillIsStillNotPoggers();
            CreateMagiciansEarringsEffects();
            CreateLumpkinEffect();
        }

        private void CreateOrbitalImpact()
        {
            orbitalImpact = BandaidConvert.Resources.Load<GameObject>("prefabs/effects/impacteffects/ParentSlamEffect").InstantiateClone("OrbitalImpactEffect", false);

            var particleParent = orbitalImpact.transform.Find("Particles");
            var debris = particleParent.Find("Debris, 3D");
            var debris2 = particleParent.Find("Debris");
            var sphere = particleParent.Find("Nova Sphere");

            debris.gameObject.SetActive(false);
            debris2.gameObject.SetActive(false);
            sphere.gameObject.SetActive(false);

            Content.ContentHandler.Effects.RegisterEffect(Content.ContentHandler.Effects.CreateGenericEffectDef(orbitalImpact));
        }

        private void CreateUnknownEliteEffect()
        {
            unknownEffect = BandaidConvert.Resources.Load<GameObject>("prefabs/PoisonAffixEffect").InstantiateClone("AAAAA", false);
            var fire = BandaidConvert.Resources.Load<GameObject>("prefabs/projectileghosts/RedAffixMissileGhost");
            var flames = Object.Instantiate<GameObject>(fire.transform.Find("Particles (1)/Flames").gameObject);

            //LogCore.LogI(flames);
            //flames.transform.SetParent(fire.transform);
            flames.transform.SetParent(unknownEffect.transform);

        }

        private void CreateMagicEffects()
        {
            Armor();
            Regen();
            AttackSpeed();
            void Armor()
            {
                magicArmor = BandaidConvert.Resources.Load<GameObject>("Prefabs/Effects/BearProc").InstantiateClone("MagicEffectArmor", false);
                //LogCore.LogI("hi1");
                var tmp = magicArmor.transform.Find("TextCamScaler/TextRiser/TextMeshPro").GetComponent<TextMeshPro>();
                //LogCore.LogI("hi2");
                var langMeshController = magicArmor.transform.Find("TextCamScaler/TextRiser/TextMeshPro").GetComponent<LanguageTextMeshController>();
                //LogCore.LogI("hi3");
                magicArmor.transform.Find("Fluff").gameObject.SetActive(false);
                //LogCore.LogI("hi4");
                R2API.LanguageAPI.Add("MAGIC_ARMOR_EFFECT", "+Armor!");
                //LogCore.LogI("hi5");

                tmp.text = "+Armor!";
                //LogCore.LogI("hi6");
                R2API.LanguageAPI.Add("MAGIC_ARMOR_EFFECT", "+Armor!");

                langMeshController.token = "MAGIC_ARMOR_EFFECT";
                //LogCore.LogI("hi7");

                Content.ContentHandler.Effects.RegisterEffect(new EffectDef()
                {

                    prefab = magicArmor,
                    prefabEffectComponent = magicArmor.GetComponent<EffectComponent>(),
                    prefabVfxAttributes = magicArmor.GetComponent<VFXAttributes>(),
                    prefabName = magicArmor.name,
                });
            }
            void Regen()
            {
                magicRegen = BandaidConvert.Resources.Load<GameObject>("Prefabs/Effects/BearProc").InstantiateClone("MagicEffectRegen", false);
                var tmp = magicRegen.transform.Find("TextCamScaler/TextRiser/TextMeshPro").GetComponent<TextMeshPro>();
                var langMeshController = magicRegen.transform.Find("TextCamScaler/TextRiser/TextMeshPro").GetComponent<LanguageTextMeshController>();

                magicRegen.transform.Find("Fluff").gameObject.SetActive(false);

                R2API.LanguageAPI.Add("MAGIC_REGEN_EFFECT", "+Regeneration!");

                tmp.text = "+Regeneration!";
                langMeshController.token = "MAGIC_REGEN_EFFECT";

                Content.ContentHandler.Effects.RegisterEffect(new EffectDef()
                {

                    prefab = magicRegen,
                    prefabEffectComponent = magicRegen.GetComponent<EffectComponent>(),
                    prefabVfxAttributes = magicRegen.GetComponent<VFXAttributes>(),
                    prefabName = magicRegen.name,
                });
            }
            void AttackSpeed()
            {
                magicAttackSpeed = BandaidConvert.Resources.Load<GameObject>("Prefabs/Effects/BearProc").InstantiateClone("MagicEffectAttackSpeed", false);
                var tmp = magicAttackSpeed.transform.Find("TextCamScaler/TextRiser/TextMeshPro").GetComponent<TextMeshPro>();
                var langMeshController = magicAttackSpeed.transform.Find("TextCamScaler/TextRiser/TextMeshPro").GetComponent<LanguageTextMeshController>();

                magicAttackSpeed.transform.Find("Fluff").gameObject.SetActive(false);

                R2API.LanguageAPI.Add("MAGIC_ATKSPEED_EFFECT", "+Attack Speed!");

                tmp.text = "+Attack Speed!";
                langMeshController.token = "MAGIC_ATKSPEED_EFFECT";

                Content.ContentHandler.Effects.RegisterEffect(new EffectDef()
                {

                    prefab = magicAttackSpeed,
                    prefabEffectComponent = magicAttackSpeed.GetComponent<EffectComponent>(),
                    prefabVfxAttributes = magicAttackSpeed.GetComponent<VFXAttributes>(),
                    prefabName = magicAttackSpeed.name,
                });
            }
        }

        public static GameObject CreateEffect(string name)
        {
            GameObject obj = AssetLoader.mainAssetBundle.LoadAsset<GameObject>(name);

            Content.ContentHandler.Effects.RegisterEffect(new EffectDef()
            {
                prefab = obj,
            });
            return obj;
        }

        public static GameObject CreateAsset(string name, bool positionAtReferencedTransform, bool parentToReferencedTransform, bool applyScale, string soundName, bool disregardZScale, VFXAttributes.VFXIntensity intensity, VFXAttributes.VFXPriority priority)
        {
            GameObject obj = AssetLoader.mainAssetBundle.LoadAsset<GameObject>(name);
            EffectComponent effectComponent = obj.AddComponent<EffectComponent>();
            VFXAttributes attributes = obj.AddComponent<VFXAttributes>();
            DestroyOnParticleEnd destroyOnEnd = obj.AddComponent<DestroyOnParticleEnd>();

            effectComponent.effectIndex = EffectIndex.Invalid;
            effectComponent.positionAtReferencedTransform = positionAtReferencedTransform;
            effectComponent.parentToReferencedTransform = parentToReferencedTransform;
            effectComponent.applyScale = applyScale;
            effectComponent.soundName = soundName;
            effectComponent.disregardZScale = disregardZScale;

            attributes.vfxIntensity = intensity;
            attributes.vfxPriority = priority;


            Content.ContentHandler.Effects.RegisterEffect(new EffectDef()
            {

                prefab = obj,
                prefabEffectComponent = obj.GetComponent<EffectComponent>(),
                prefabVfxAttributes = obj.GetComponent<VFXAttributes>(),
                prefabName = obj.name,
            });
            return obj;
        }
    }
}
