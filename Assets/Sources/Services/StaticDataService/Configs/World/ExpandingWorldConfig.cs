using Assets.Sources.Data;
using System.Linq;
using UnityEngine;

namespace Assets.Sources.Services.StaticDataService.Configs.World
{
    [CreateAssetMenu(fileName = "WorldConfig", menuName = "StaticData/WorldConfig/Create new expanding world config", order = 51)]
    public class ExpandingWorldConfig : WorldConfig
    {
        public override WorldData GetWorldData() =>
            new ExpandingWorldData(Id, TilesDatas, NextBuildingTypeForCreation, StartingAvailableBuildingTypes.ToList(), Length, Width, StartStoreList);
    }
}
