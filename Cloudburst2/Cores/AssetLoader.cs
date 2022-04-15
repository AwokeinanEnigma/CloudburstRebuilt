using Cloudburst.Builders;
using RoR2.ContentManagement;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using ThreeEyedGames;
using UnityEngine;
using UnityEngine.AddressableAssets;



namespace Cloudburst.Cores
{
    public class AssetLoader : Core
    {
        /// <summary>
        /// This is the global cloudburst asset bundle!
        /// </summary>
        public static AssetBundle mainAssetBundle;

        public override string Name => "Asset Loader";

        public override bool Priority => true;

        public override void OnLoaded()
        {
            // populate ASSETS
            using (var assetStream = Assembly.GetExecutingAssembly().GetManifestResourceStream("Cloudburst2.assetburst"))
            {
                mainAssetBundle = AssetBundle.LoadFromStream(assetStream);
            }

            ContentManager.onContentPacksAssigned += ContentManager_onContentPacksAssigned;
        }

        private void ContentManager_onContentPacksAssigned(HG.ReadOnlyArray<ReadOnlyContentPack> obj)
        {
            Material[] source = Resources.FindObjectsOfTypeAll<Material>();
            Material mfMat = Addressables.LoadAssetAsync<GameObject>(CCUtilities.ConvertResourceLoadToGUID("prefabs/temporaryvisualeffects/SlowDownTime")).WaitForCompletion().transform.Find("Visual/Mesh").GetComponent<Renderer>().material;

            foreach (GameObject gameObject in mainAssetBundle.LoadAllAssets<GameObject>())
            {
                MaterialSwapper[] quickSwap = gameObject.GetComponentsInChildren<MaterialSwapper>();
                foreach (MaterialSwapper swap in quickSwap) 
                {
                    //CCUtilities.LogI(swap.gameObject.name);
                    //CCUtilities.LogI(swap.materialName);
                    Material material = (from p in source
                                         where p.name == swap.materialName//"ppLocalUnderwater"
                                         select p).FirstOrDefault<Material>();

                    Decal decal = swap.GetComponent<Decal>();
                    if (!decal)
                    {
                        Renderer renderer = swap.gameObject.GetComponent<Renderer>();
                        if (renderer)
                        {
                            if (swap.materialName == "matBaubleEffect")
                            {
                                material = mfMat;
                            }
                            if (material)
                            {
                                renderer.material = material;
                                renderer.sharedMaterial = material;
                            }


                            else
                            {
                                CCUtilities.LogE($"{swap.materialName} could not be found! Attempting alternative loading method! GameObject: {swap.gameObject}");
                                Material newMat = Addressables.LoadAssetAsync<Material>(CCUtilities.ConvertResourceLoadToGUID("Material/" + swap.materialName)).WaitForCompletion();

                                if (newMat)
                                {

                                    renderer.material = newMat;
                                    renderer.sharedMaterial = newMat;
                                }
                                else
                                {
                                    CCUtilities.LogE(swap.materialName + " cannot be found! Alternative loading method failed!");

                                }
                            }
                        }
                    }
                    else
                    {
                        decal.Material = material;
                    }
                    CloudburstPlugin.Destroy(swap);
                }
            }

            foreach (GameObject gameObject in mainAssetBundle.LoadAllAssets<GameObject>())
            {
                MaterialArraySwapper[] quickSwaps = gameObject.GetComponentsInChildren<MaterialArraySwapper>();

                foreach (MaterialArraySwapper swap in quickSwaps)
                {
                    //CCUtilities.LogI("swap");
                    Renderer renderer = swap.gameObject.GetComponent<Renderer>();
                    if (renderer && swap.materialName != null)
                    {
                        Material material = (from p in source
                                             where p.name == swap.materialName
                                             select p).FirstOrDefault<Material>();
                        Material secondMaterial = (from p in source
                                                   where p.name == swap.secondMaterialName
                                                   select p).FirstOrDefault<Material>();
                        if (material)
                        {
                            List<Material> newMats = new List<Material>()
                            {
                                  renderer.material,
                                  material
                            };

                            if (secondMaterial)
                            {
                                newMats.Add(secondMaterial);
                            }
                            renderer.materials = newMats.ToArray();
                            renderer.sharedMaterials = newMats.ToArray();
                        }
                    }
                    CloudburstPlugin.Destroy(swap);
                }
            }
        }

        public override void Start()
        {
            base.Start();
        }
    }
}
