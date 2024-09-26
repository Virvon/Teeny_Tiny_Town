using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Assets.Sources.Services.StaticDataService.Configs.Windows
{
    [CreateAssetMenu(fileName = "StoreItemsConfig", menuName = "StaticData/Create new store items config", order = 51)]
    public class StoreItemsConfig : ScriptableObject
    {
        public AssetReferenceGameObject AssetReference;
        public StoreItemConfig[] Configs;
    }
}
