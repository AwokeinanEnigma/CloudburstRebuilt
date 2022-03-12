using R2API;
using RoR2;
using static RoR2.DotController;

namespace Cloudburst.Builders
{
    /// <summary>
    /// Use this for quick and easy DoTs.
    /// </summary>
    public class DoTBuilder 
    {
        /// <summary>
        /// The buff is that is assosicated with this DoT.
        /// </summary>
        public BuffDef assosicatedBuff;
        /// <summary>
        /// The damage coefficient of the DoT.
        /// </summary>
        public float damageCoefficient;
        /// <summary>
        /// The damage color index of the dot. I.E when damage numbers show up due to the DoT dealing damage, what 
        /// </summary>
        public DamageColorIndex damageColorIndex;
        /// <summary>
        /// I dunno man.
        /// </summary>
        public float interval;
        /// <summary>
        /// Think of how bleed resets its timer whenever you add a new stack. If true, it'll do that. If false, it won't.
        /// </summary>
        public bool resetTimerOnAdd;

        /// <summary>
        /// The custom behavior assosicated with this DoT. Most of the time you can leave this empty if you want.
        /// </summary>
        public DotAPI.CustomDotBehaviour customDotBehaviour ;
        /// <summary>
        /// The custom DoT visual, this will allow you add special visual effects like bleed's blood effect provided you have the effect prefab ready to go.
        /// </summary>
        public DotAPI.CustomDotVisual customDotVisual;
        public DotIndex BuildDoT() {
            
            DotDef def = new DotDef();
            
            def.associatedBuff = assosicatedBuff;
            def.damageCoefficient = damageCoefficient;
            def.damageColorIndex = damageColorIndex;
            def.interval = interval;
            def.resetTimerOnAdd = resetTimerOnAdd;
            
            return DotAPI.RegisterDotDef(def, customDotBehaviour, customDotVisual); 
        }
    }
}
