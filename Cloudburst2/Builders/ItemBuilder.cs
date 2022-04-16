using BepInEx.Configuration;
using RoR2;

using System.Collections.Generic;
using System;
using UnityEngine;
using R2API;
using Cloudburst.Cores;

namespace Cloudburst.Builders
{
    public abstract class ItemBuilder<T> : ItemBuilder where T : ItemBuilder<T>
    {
        public static T instance { get; private set; }

        public ItemBuilder()
        {
            if (instance != null) throw new InvalidOperationException("Singleton class \"" + typeof(T).Name + "\" was instantiated twice!");
            instance = this as T;
        }
    }

    public abstract class ItemBuilder
    {
        public abstract string ItemName { get; }
        public abstract string ItemLangTokenName { get; }
        public abstract string ItemPickupDesc { get; }
        public abstract string ItemFullDescription { get; }
        public abstract string ItemLore { get; }

        public abstract ItemTier Tier { get; }
        
        public virtual ItemTag[] ItemTags { get; set; } = new ItemTag[] { };

        public abstract string ItemModelPath { get; }

        public abstract string ItemIconPath { get; }

        public ItemDef Index;

        public virtual bool CanRemove { get; } = true;

        public virtual bool AIBlacklisted { get; set; } = false;

        public virtual UnlockableDef UnlockDef { get; set; } = null;

        public string ConfigName = "Item: ";

        protected abstract void Initialization();

        /// <summary>
        /// Only override when you know what you are doing, or call base.Init()!
        /// </summary>
        /// <param name="config"></param>
        internal virtual void Init(ConfigFile file)
        {
            ConfigName = ConfigName + ItemName;
            CreateConfig(file);
            CreateLang();
            CreateItem();
            Initialization();
            Hooks();
        }
                                                                                                                                                                            
        public GameObject Load()
        {
            return AssetLoader.mainAssetBundle.LoadAsset<GameObject>(ItemModelPath);
        }

        public virtual void CreateConfig(ConfigFile file) { }

        protected virtual void CreateLang()
        {
            R2API.LanguageAPI.Add("ITEM_" + ItemLangTokenName + "_NAME", ItemName);
            R2API.LanguageAPI.Add("ITEM_" + ItemLangTokenName + "_PICKUP", ItemPickupDesc);
            R2API.LanguageAPI.Add("ITEM_" + ItemLangTokenName + "_DESCRIPTION", ItemFullDescription);
            R2API.LanguageAPI.Add("ITEM_" + ItemLangTokenName + "_LORE", ItemLore);
        }

        public abstract ItemDisplayRuleDict CreateItemDisplayRules();
        protected void CreateItem()
        {
            if (AIBlacklisted)
            {
                ItemTags = new List<ItemTag>(ItemTags) { ItemTag.AIBlacklist }.ToArray();
            }
            Index = ScriptableObject.CreateInstance<ItemDef>();

            Index.name = "ITEM_" + ItemLangTokenName;
            Index.nameToken = "ITEM_" + ItemLangTokenName + "_NAME";
            Index.pickupToken = "ITEM_" + ItemLangTokenName + "_PICKUP";
            Index.descriptionToken = "ITEM_" + ItemLangTokenName + "_DESCRIPTION";
            Index.loreToken = "ITEM_" + ItemLangTokenName + "_LORE";
            Index.pickupModelPrefab = AssetLoader.mainAssetBundle.LoadAsset<GameObject>(ItemModelPath);
            Index.pickupIconSprite = AssetLoader.mainAssetBundle.LoadAsset<Sprite>(ItemIconPath);
            Index.hidden = false;
            Index.canRemove = CanRemove;
            Index.tier = Tier;
            if (ItemTags.Length > 0)
            {
                Index.tags = ItemTags;
            }
            if (UnlockDef != null)
            {
                Index.unlockableDef = UnlockDef;
            }
            var itemDisplayRules = CreateItemDisplayRules();
            R2API.ItemAPI.Add(new CustomItem(Index, itemDisplayRules));
        }

        public virtual void Hooks() { }

        //Based on ThinkInvis' methods
        public int GetCount(CharacterBody body)
        {
            if (!body || !body.inventory) { return 0; }

            return body.inventory.GetItemCount(Index);
        }

        public int GetCount(CharacterMaster master)
        {
            if (!master || !master.inventory) { return 0; }

            return master.inventory.GetItemCount(Index);
        }

        public int GetCount(Inventory inv)
        {
            if (!inv) { return 0; }

            return inv.GetItemCount(Index);
        }

        public int GetCountSpecific(CharacterBody body, ItemDef Items)
        {
            if (!body || !body.inventory) { return 0; }

            return body.inventory.GetItemCount(Index);
        }
    }
}