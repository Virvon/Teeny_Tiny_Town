using Assets.Sources.Gameplay.World.WorldInfrastructure;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Assets.Sources.Services.StaticDataService.Configs
{
    [CreateAssetMenu(fileName = "BuildingConfig", menuName = "StaticData/Building/Create new building config", order = 51)]
    public class BuildingConfig : ScriptableObject
    {
        public BuildingType BuildingType;
        public AssetReferenceGameObject AssetReference;
        public GroundType GroundType;
        public BuildingType NextBuilding;
    }
}
