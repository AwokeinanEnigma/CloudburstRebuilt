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

namespace Cloudburst.Builders
{
    public class QuickBuffBuilder
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

    /// <summary>
    /// This is a more advanced version of the quick buff builder. Use this for advanced buff behaviors.
    /// </summary>
    public abstract class BuffBuilder
    {
        /// <summary>
        /// The icon of the buff. 
        /// </summary>
        public abstract Sprite iconSprite { get; set; }

        /// <summary>
        /// The color of the icon of the buff. Basically if the icon is colored white, and this color is set to red, it will make the icon red.
        /// </summary>
        public abstract Color buffColor { get; set; }

        /// <summary>
        /// If true, the buff will have multiple stacks. If not, the buff will only have one stack at a time, no matter how many times you apply it. 
        /// </summary>
        public abstract bool canStack { get; set; }

        /// <summary>
        /// The EliteDef of this buff. When an enemy is an elite, this is the buff they will be gievn.
        /// </summary>
        public abstract EliteDef eliteDef { get; set; }
        
        /// <summary>
        /// Is this buff a debuff? If true, blast shower and other cleansing mechanics will be able to remove this buff. If false, this buff will not be able to be cleansed.
        /// </summary>
        public abstract bool isDebuff { get; set; }

        /// <summary>
        /// The SFX that plays when this buff is applied. Don't bother with this for now.
        /// </summary>
        public abstract NetworkSoundEventDef startSfx { get; set; }

        /// <summary>
        /// The BuffDef of the buff. Use this to check if an entity has the buff.
        /// </summary>
        public BuffDef buff;

        /// <summary>
        /// Only override if you know what you're doing.
        /// </summary>
        public virtual void Initalize()
        {
            InitalizeDef();
            R2API.RecalculateStatsAPI.GetStatCoefficients += RecalculateStatsAPI_GetStatCoefficients;
        }

        private void RecalculateStatsAPI_GetStatCoefficients(CharacterBody sender, RecalculateStatsAPI.StatHookEventArgs args)
        {
            GetStatCoeffs(sender, ref args);
        }

        /// <summary>
        /// This is called
        /// </summary>
        /// <param name="sender">The CharacterBody with the buff.</param>
        /// <param name="args">The StatHookEventARGS, modify the fields in this to affect stats.</param>
        public virtual void GetStatCoeffs(CharacterBody sender, ref RecalculateStatsAPI.StatHookEventArgs args) { }

        /// <summary>
        /// Creates the BuffDef with all the fields that you have assigned.
        /// </summary>
        public virtual void InitalizeDef()
        {
            var buff = ScriptableObject.CreateInstance<BuffDef>();
            buff.canStack = canStack;
            buff.isDebuff = isDebuff;
            buff.iconSprite = iconSprite; 
            buff.buffColor = buffColor;

            if (startSfx)
            {
                buff.startSfx = startSfx;
            }
            if (eliteDef)
            {
                buff.eliteDef = eliteDef;
            }
            
            Cloudburst.Content.ContentHandler.Buffs.RegisterBuff(buff);
        }
    }

}
