using Assets.Sources.Data.WorldDatas;
using Assets.Sources.Services.StaticDataService.Configs.Building;
using System;
using System.Linq;
using UnityEngine;

namespace Assets.Sources.Services.StaticDataService.Configs.World
{
    [CreateAssetMenu(fileName = "WorldConfig", menuName = "StaticData/WorldConfig/Create new expanding world config", order = 51)]
    public class ExpandingWorldConfig : CurrencyWorldConfig
    {
        public Vector2Int StartSize;
        public ExpandConfig[] ExpandConfigs;

        public bool ContainsExpand(BuildingType type, out ExpandConfig expandConfig)
        {
            expandConfig = ExpandConfigs.FirstOrDefault(config => config.BuidldingType == type);
            
            return expandConfig != null;
        }

        public override WorldData GetWorldData(uint[] goals) =>
            new ExpandingWorldData(Id, TilesDatas, NextBuildingTypeForCreation, StartingAvailableBuildingTypes.ToList(), StartSize, StartStoreList, goals);
    }
    [Serializable]
    public class ExpandConfig
    {
        public BuildingType BuidldingType;
        public Vector2Int ExpandedSize;
    }
}
