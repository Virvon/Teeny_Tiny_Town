using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Assets.Sources.Services.StaticDataService.Configs.Windows
{
    [CreateAssetMenu(fileName = "GameplayStoreItemsConfig", menuName = "StaticData/Create new gameplay store items config", order = 51)]
    public class GameplayStoreItemsConfig : ScriptableObject
    {
        public AssetReferenceGameObject AssetReference;
        public GameplayStoreItemConfig[] Configs;
    }
}
