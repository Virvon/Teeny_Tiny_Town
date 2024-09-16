using Assets.Sources.Gameplay.Tile;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Assets.Sources.Services.StaticDataService.Configs
{
    [CreateAssetMenu(fileName = "MergeChainConfig", menuName = "StaticData/Create new merge chain config", order = 51)]
    public class MergeConfig : ScriptableObject
    {
        public BuildingType BuildingType;
        public AssetReferenceGameObject AssetReference;
        public BuildingType NextBuilding;
    }
}
