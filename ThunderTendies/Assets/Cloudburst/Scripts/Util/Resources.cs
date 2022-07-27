using UnityEngine.AddressableAssets;

namespace BandaidConvert
{
    class Resources
    {
        public static T Load<T>(string path)
        {
            string truePath = CCUtilities.ConvertResourceLoadToGUID(path);
            return Addressables.LoadAssetAsync<T>(key: truePath).WaitForCompletion();
        }
    }
}
