using Cloudburst.Builders;
using RoR2;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Cloudburst.Cores
{
    public class ItemDisplayLoader : Core
    {
        public static Dictionary<string, GameObject> itemDisplayPrefabs = new Dictionary<string, GameObject>();

        public override string Name => "Item Display Loader";

        public override bool Priority => true;

        public override void OnLoaded()
        {
        }

        public override void Start()
        {
            base.Start();
          //  PopulateDisplays();
        }

        //This code was borrowed from enforcer -- don't even ASK how it works.

        private void PopulateDisplays()
        {
            ItemDisplayRuleSet itemDisplayRuleSet = Addressables.LoadAssetAsync<GameObject>("Prefabs/CharacterBodies/CommandoBody").WaitForCompletion().GetComponent<ModelLocator>().modelTransform.GetComponent<CharacterModel>().itemDisplayRuleSet;

            ItemDisplayRuleSet.KeyAssetRuleGroup[] item = itemDisplayRuleSet.keyAssetRuleGroups;

            for (int i = 0; i < item.Length; i++)
            {
                ItemDisplayRule[] rules = item[i].displayRuleGroup.rules;

                for (int j = 0; j < rules.Length; j++)
                {
                    GameObject followerPrefab = rules[j].followerPrefab;
                    if (followerPrefab)
                    {
                        string name = followerPrefab.name;
                        string key = (name != null) ? name.ToLower() : null;
                        if (!itemDisplayPrefabs.ContainsKey(key))
                        {
                            itemDisplayPrefabs[key] = followerPrefab;
                        }
                    }
                }
            }
        }

        public static GameObject LoadDisplay(string name)
        {
            if (itemDisplayPrefabs.ContainsKey(name.ToLower()))
            {
                //LogCore.LogI("HI112");
                if (itemDisplayPrefabs[name.ToLower()])
                {
                    return itemDisplayPrefabs[name.ToLower()];
                }
            }
            return null;
        }

        #region Rule Creation
        public static ItemDisplayRuleSet.KeyAssetRuleGroup CreateFollowerDisplayRule(ItemDef itemDef, string prefabName, Vector3 position, Vector3 rotation, Vector3 scale)
        {
            ItemDisplayRuleSet.KeyAssetRuleGroup displayRule = new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = itemDef,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            childName = "Base",
                            followerPrefab = LoadDisplay(prefabName),
                            limbMask = LimbFlags.None,
                            localPos = position,
                            localAngles = rotation,
                            localScale = scale
                        }
                    }
                }
            };

            return displayRule;
        }
        public static ItemDisplayRuleSet.KeyAssetRuleGroup CreateGenericItemDisplayRule(ItemDef def, string prefabName, string childName, Vector3 position, Vector3 rotation, Vector3 scale)
        {
            ItemDisplayRuleSet.KeyAssetRuleGroup displayRule = new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = def,
                displayRuleGroup = new DisplayRuleGroup
                {

                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            childName = childName,
                            followerPrefab = LoadDisplay(prefabName),
                            limbMask = LimbFlags.None,
                            localPos = position,
                            localAngles = rotation,
                            localScale = scale
                        }
                    }
                }
            };
            return displayRule;
        }
        public static ItemDisplayRuleSet.KeyAssetRuleGroup CreateGenericEquipmentDisplayRule(EquipmentDef def, string prefabName, string childName, Vector3 position, Vector3 rotation, Vector3 scale)
        {
            ItemDisplayRuleSet.KeyAssetRuleGroup displayRule = new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = def,
                displayRuleGroup = new DisplayRuleGroup
                {

                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            childName = childName,
                            followerPrefab = LoadDisplay(prefabName),
                            limbMask = LimbFlags.None,
                            localPos = position,
                            localAngles = rotation,
                            localScale = scale
                        }
                    }
                }
            };
            return displayRule;
        }
        public static ItemDisplayRuleSet.KeyAssetRuleGroup CreateFollowerDisplayRule(EquipmentDef itemDef, string prefabName, Vector3 position, Vector3 rotation, Vector3 scale)
        {
            ItemDisplayRuleSet.KeyAssetRuleGroup displayRule = new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = itemDef,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            childName = "Base",
                            followerPrefab = LoadDisplay(prefabName),
                            limbMask = LimbFlags.None,
                            localPos = position,
                            localAngles = rotation,
                            localScale = scale
                        }
                    }
                }
            };

            return displayRule;
        }
        #endregion

        #region Obsoletes
        [Obsolete]
        public static ItemDisplayRuleSet.NamedRuleGroup CreateGenericDisplayRule(string itemName, GameObject itemPrefab, string childName, Vector3 position, Vector3 rotation, Vector3 scale)
        {
            ItemDisplayRuleSet.NamedRuleGroup displayRule = new ItemDisplayRuleSet.NamedRuleGroup
            {
                name = itemName,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            childName = childName,
                            followerPrefab = itemPrefab,
                            limbMask = LimbFlags.None,
                            localPos = position,
                            localAngles = rotation,
                            localScale = scale
                        }
                    }
                }
            };

            return displayRule;
        }

        [Obsolete]
        public static ItemDisplayRuleSet.NamedRuleGroup CreateFollowerDisplayRule(string itemName, GameObject itemPrefab, Vector3 position, Vector3 rotation, Vector3 scale)
        {
            ItemDisplayRuleSet.NamedRuleGroup displayRule = new ItemDisplayRuleSet.NamedRuleGroup
            {
                name = itemName,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            childName = "Base",
                            followerPrefab = itemPrefab,
                            limbMask = LimbFlags.None,
                            localPos = position,
                            localAngles = rotation,
                            localScale = scale
                        }
                    }
                }
            };

            return displayRule;
        }
        #endregion
    }
}
