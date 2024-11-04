using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Assets.Sources.Services.StaticDataService.Configs.WorldStore
{
    [CreateAssetMenu(fileName = "BildingStoreItemsConfig", menuName = "StaticData/WorldStore/Create new buildings store list config", order = 51)]
    public class BuildingStoreItemsCofnig : ScriptableObject
    {
        public AssetReferenceGameObject AssetReference;
        public BuildingStoreItemConfig[] Configs;
    }
}
