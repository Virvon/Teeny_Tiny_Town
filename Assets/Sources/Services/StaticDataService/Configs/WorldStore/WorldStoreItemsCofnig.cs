using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Assets.Sources.Services.StaticDataService.Configs.WorldStore
{
    [CreateAssetMenu(fileName = "WorldStoreItemsConfig", menuName = "StaticData/WorldStore/Create new world store items config", order = 51)]
    public class WorldStoreItemsCofnig : ScriptableObject
    {
        public AssetReferenceGameObject AssetReference;
        public GameplayStoreItemConfig[] Configs;
    }
}
