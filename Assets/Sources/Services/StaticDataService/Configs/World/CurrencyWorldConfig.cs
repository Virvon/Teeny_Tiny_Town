using Assets.Sources.Data.WorldDatas;
using Assets.Sources.Services.StaticDataService.Configs.Building;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Sources.Services.StaticDataService.Configs.World
{
    [CreateAssetMenu(fileName = "CurrencyWorldConfig", menuName = "StaticData/WorldConfig/Create new currency world config", order = 51)]
    public class CurrencyWorldConfig : WorldConfig
    {
        public List<BuildingType> StartStoreList;

        public override WorldData GetWorldData(uint[] goals) =>
            new CurrencyWorldData(Id, TilesDatas, NextBuildingTypeForCreation, StartingAvailableBuildingTypes.ToList(), Size, StartStoreList, goals);
    }
}
