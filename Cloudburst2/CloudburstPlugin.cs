using BepInEx;
using BepInEx.Configuration;
using Cloudburst.Builders;
using Cloudburst.Content;
using Cloudburst.Cores;
using Cloudburst.Items.Common;
using R2API;
using R2API.Utils;
using RoR2;
using RoR2.UI;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

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

        /// <summary>
        /// The static instance of Cloudburst. There will only be one instance of the Cloudburst class. If there is more, then there is a problem.
        /// </summary>
        public static CloudburstPlugin instance;
        /// <summary>
        /// This is invoked on the first frame of the game.
        /// </summary>
        public event Action PluginStart;

        public void  Start() {
            PluginStart?.Invoke();
        }
        
        public ConfigFile GetConfig()
        {
            return Config;
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

        }

        public List<Core> activatedCores;

        private void Initialize()
        {
            SceneManager.activeSceneChanged += SceneManager_activeSceneChanged;

            GlobalHooks.Init();

            ContentHandler.Load();

            activatedCores = new List<Core>();
            activatedCores.Add(new AssetLoader());
            activatedCores.Add(new Effects());
            new Engineer.Engineer();
            activatedCores.Add(new ItemDisplayLoader());
            activatedCores.Add(new ItemManager());
            var ctd = new Custodian.Custodian();
            ctd.Init(configFile);
        }

        private void SceneManager_activeSceneChanged(Scene arg0, Scene arg1)
        {

            if (arg1 != default && arg1.name == "title")
            {
                var menu = GameObject.Find("MainMenu");
                var title = menu.transform.Find("MENU: Title/TitleMenu/SafeZone/ImagePanel (JUICED)/LogoImage");
                var indicator = menu.transform.Find("MENU: Title/TitleMenu/MiscInfo/Copyright/Copyright (1)");

                var build = indicator.GetComponent<HGTextMeshProUGUI>();

                build.fontSize += 4;
                build.text = build.text + Environment.NewLine + $"Cloudburst Version: {version}";

                title.GetComponent<Image>().sprite = AssetLoader.mainAssetBundle.LoadAsset<Sprite>("Assets/Cloudburst/cloudburstlogo.png");
            }
        }

        public void Awake() 
        {
            Initialize();
        }


        public void OnDisable()
        {
            //SingletonHelper.Unassign<CloudburstPlugin>(CloudburstPlugin.instance, this);
            //CCUtilities.LogI("Cloudburst instance unassigned.");
        }
    }
}