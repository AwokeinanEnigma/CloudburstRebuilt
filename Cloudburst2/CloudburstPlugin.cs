using BepInEx;
using BepInEx.Configuration;
using Cloudburst.Builders;
using Cloudburst.Content;
using Cloudburst.Cores;
using R2API;
using R2API.Utils;
using RoR2;
using System;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

namespace Cloudburst
{
    [BepInDependency(R2API.R2API.PluginGUID, BepInDependency.DependencyFlags.HardDependency)]
    [R2APISubmoduleDependency(nameof(ItemAPI))]
    [R2APISubmoduleDependency(nameof(LanguageAPI))]
    [R2APISubmoduleDependency(nameof(PrefabAPI))]
    [R2APISubmoduleDependency(nameof(LoadoutAPI))]
    [R2APISubmoduleDependency(nameof(DamageAPI))]
    [R2APISubmoduleDependency(nameof(CommandHelper))]
    [R2APISubmoduleDependency(nameof(RecalculateStatsAPI))]
    [R2APISubmoduleDependency(nameof(UnlockableAPI))]
    [BepInPlugin(guid, modName, version)]

    public class CloudburstPlugin : BaseUnityPlugin
    { 

        public const string guid = "com.TeamCloudburst.Cloudburst";
        public const string modName = "Cloudburst";
        public const string version = "0.3.0";

        
        public static CloudburstPlugin instance;

        public event Action PluginStart;

        public void Start() {
            PluginStart?.Invoke();
        }
        
        public ConfigFile GetConfig()
        {
            return Config;
        }

        public bool ValidateItem(ItemBuilder item, List<ItemBuilder> itemList)
        {
            var enabled = Config.Bind<bool>("Item: " + item.ItemName, "Enable Item?", true, "Should this item appear in runs?").Value;
            var aiBlacklist = Config.Bind<bool>("Item: " + item.ItemName, "Blacklist Item from AI Use?", false, "Should the AI not be able to obtain this item?").Value;
            if (enabled)
            {
                itemList.Add(item);
                if (aiBlacklist)
                {
                    item.AIBlacklisted = true;
                }
            }
            return enabled;
        }

        public ConfigFile configFile
        {
            get
            {
                return this.Config;
            }
            private set { }
        }

        public CloudburstPlugin()
        {
            instance = this;
            CCUtilities.logger = Logger;
            Initialize();
        }

        public List<Core> activatedCores;

        private void Initialize()
        {
            ContentHandler.Load();

            activatedCores = new List<Core>();
            activatedCores.Add(new AssetLoader());
            activatedCores.Add(new ItemDisplayLoader());
            var ctd = new Custodian.Custodian();
            ctd.Init(configFile);
        }

        public void Awake()
        {

        }


        public void OnDisable()
        {
            SingletonHelper.Unassign<CloudburstPlugin>(CloudburstPlugin.instance, this);
            CCUtilities.LogI("Cloudburst instance unassigned.");
        }
    }
}