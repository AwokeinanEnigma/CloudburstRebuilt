using BepInEx.Configuration;
using Cloudburst.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace Cloudburst.Cores
{
    public class ItemManager : Core
    {
        public override string Name => "Asset Loader";

        public override bool Priority => true;

        /// <summary>
        /// This is a list of enabled items.
        /// </summary>
        public static List<ItemBuilder> enabledItems;
        /// <summary>
        /// This is a list of disabled items.
        /// </summary>
        public static List<ItemBuilder> disabledItems;


        /// <summary>
        /// This is a list of enabled equipments.
        /// </summary>
        public static List<EquipmentBuilder> enabledEquips;
        /// <summary>
        /// This is a list of disabled equipments.
        /// </summary>
        public static List<EquipmentBuilder> disabledEquips;

        public bool ValidateItem(ItemBuilder item)
        {
            //Find if the item is enabled.d
            bool enabled = CloudburstPlugin.instance.Config.Bind<bool>("Item: " + item.ItemName, "Enable Item?", true, "Should this item appear in runs?").Value;
            //Find if the item is blacklisted.
            bool blacklisted = CloudburstPlugin.instance.Config.Bind<bool>("Item: " + item.ItemName, "Blacklist Item from AI Use?", false, "Should the AI not be able to obtain this item?").Value;
            //If our item is enabled
            if (enabled)
            {
                //Add it to the list of enabled items
                enabledItems.Add(item);
                if (blacklisted)
                {
                    //If the item is blacklisted from AI, make sure to manually enable that in the item.
                    item.AIBlacklisted = true;
                }
            }
            else
            {
                //Else we'll add it to the list of disabled items.
                disabledItems.Add(item);
            }

            //Enabled = Valid
            //Disabled = Invalid

            //Return whether or not enabled.
            return enabled;
        }

        public bool ValidateEquip(EquipmentBuilder item)
        {
            //Find if the item is enabled.d
            bool enabled = CloudburstPlugin.instance.Config.Bind<bool>("Item: " + item.EquipmentName, "Enable Equipment?", true, "Should this equipment appear in runs?").Value;
            //Find if the item is blacklisted.
            //If our item is enabled
            if (enabled)
            {
                //Add it to the list of enabled items
                enabledEquips.Add(item);
            }
            else
            {
                //Else we'll add it to the list of disabled items.
                disabledEquips.Add(item);
            }

            //Enabled = Valid
            //Disabled = Invalid

            //Return whether or not enabled.
            return enabled;
        }

        public override void OnLoaded()
        {
            enabledItems = new List<ItemBuilder>();
            disabledItems = new List<ItemBuilder>();
            enabledEquips = new List<EquipmentBuilder>();
            disabledEquips = new List<EquipmentBuilder>();

            //Collect item types.
            IEnumerable<Type> items = Assembly.GetExecutingAssembly().GetTypes().Where(type => !type.IsAbstract && type.IsSubclassOf(typeof(ItemBuilder)));
            foreach (Type type in items)
            {
                //Cast to ItemBuilder
                ItemBuilder item = (ItemBuilder)System.Activator.CreateInstance(type);
                //Check if the item is valid
                if (ValidateItem(item))
                {
                    //If so, initiate the item.
                    item.Init(CloudburstPlugin.instance.Config);
                    CCUtilities.LogD($"Initalizing Item: {item.ItemName}");
                }
            }

            //Collect equipment types.
            IEnumerable<Type> equips = Assembly.GetExecutingAssembly().GetTypes().Where(type => !type.IsAbstract && type.IsSubclassOf(typeof(EquipmentBuilder)));
            foreach (Type type in equips)
            {
                //Cast to ItemBuilder
                EquipmentBuilder equip = (EquipmentBuilder)System.Activator.CreateInstance(type);
                //Check if the item is valid
                if (ValidateEquip(equip))
                {
                    //If so, initiate the item.
                    equip.Init(CloudburstPlugin.instance.Config);
                    CCUtilities.LogD($"Initalizing Equipment: {equip.EquipmentName}");
                }
            }

        }
        public override void Start()
        {
            base.Start();
        }
    }
}
