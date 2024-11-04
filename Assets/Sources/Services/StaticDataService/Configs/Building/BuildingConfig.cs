using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Assets.Sources.Services.StaticDataService.Configs.Building
{
    [CreateAssetMenu(fileName = "BuildingConfig", menuName = "StaticData/Building/Create new building config", order = 51)]
    public class BuildingConfig : ScriptableObject
    {
        public BuildingType BuildingType;
        public AssetReferenceGameObject AssetReference;
        public uint PointsRewardForMerge;
        public AssetReference IconAssetReference;
        public AssetReference LockIconAssetReference;
        public string Name;
        public string Title;
    }
}
