using BepInEx;
using BepInEx.Configuration;
using Cloudburst.Builders;
using Cloudburst.Content;
using Cloudburst.Cores;
using Cloudburst.Items.Common;
using R2API;
using R2API.Utils;
using RoR2;
using System;
using System.Collections.Generic;
using UnityEngine;
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

        /// <summary>
        /// The static instance of Cloudburst. There will only be one instance of the Cloudburst class. If there is more, then there is a problem.
        /// </summary>
        public static CloudburstPlugin instance;
        /// <summary>
        /// This is invoked on the first frame of the game.
        /// </summary>
        public event Action PluginStart;

        public void Start() {
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
            Initialize();
        }

        public List<Core> activatedCores;

        private void Initialize()
        {
            GlobalHooks.Init();

            ContentHandler.Load();

            activatedCores = new List<Core>();
            activatedCores.Add(new AssetLoader());

            //this code is quite retarded, i should redo it
            var materials = AssetLoader.mainAssetBundle.LoadAllAssets<Material>();
            for (int i = 0; i < materials.Length; i++)
            {
                if (materials[i].shader.name == "Standard")
                {
                    materials[i].shader = BandaidConvert.Resources.Load<Shader>("shaders/deferred/hgstandard");
                }
                if (materials[i].name.Contains("GLASS"))
                {
                    materials[i].shader = BandaidConvert.Resources.Load<Shader>("shaders/fx/hgintersectioncloudremap");
                }
                switch (materials[i].shader.name)
                {

                    case "Hopoo Games/FX/Cloud Remap Proxy":
                        //LogCore.LogI("material");
                        materials[i].shader = BandaidConvert.Resources.Load<Shader>("shaders/fx/hgcloudremap");
                        //LogCore.LogI(materials[i].shader.name);
                        break;
                }

                if (materials[i].shader.name == "stubbed_Hopoo Games/Deferred/Standard Proxy" || materials[i].shader.name == "Hopoo Games/Deferred/Standard Proxy")
                {
                    materials[i].shader = BandaidConvert.Resources.Load<Shader>("shaders/deferred/hgstandard");
                }
                if (materials[i].shader.name == "Hopoo Games/FX/Cloud Remap Proxy" || materials[i].shader.name == "stubbed_Hopoo Games/FX/Cloud Remap Proxy")
                {
                    materials[i].shader = BandaidConvert.Resources.Load<Shader>("shaders/fx/hgcloudremap");
                }
                if (materials[i].shader.name == "stubbed_Hopoo Games/FX/Solid Parallax Proxy")
                {
                    materials[i].shader = BandaidConvert.Resources.Load<Shader>("shaders/fx/hgsolidparallax");
                }
                if (materials[i].shader.name == "stubbed_Hopoo Games/Environment/Distant Water Proxy")
                {
                    materials[i].shader = BandaidConvert.Resources.Load<Shader>("shaders/environment/hgdistantwater");
                }
                if (materials[i].shader.name == "Hopoo Games/FX/Cloud Intersection Remap Proxy")
                {
                    materials[i].shader = BandaidConvert.Resources.Load<Shader>("shaders/fx/hgintersectioncloudremap");
                }
            }

            activatedCores.Add(new ItemDisplayLoader());
            activatedCores.Add(new ItemManager());
            var ctd = new Custodian.Custodian();
            ctd.Init(configFile);
            object builder = System.Activator.CreateInstance(typeof(RiftBubble));
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