using Assets.Sources.Services.StaticDataService;
using Assets.Sources.Services.StaticDataService.Configs.Building;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Sources.Data.WorldDatas
{
    public class ExpandingWorldData : CurrencyWorldData, ICurrencyWorldData
    {
        public ExpandingWorldData(
            string id,
            TileData[] tiles,
            BuildingType nextBuildingTypeForCreation,
            List<BuildingType> availableBuildingForCreation,
            Vector2Int size,
            List<BuildingType> storeList,
            uint[] goals)
            : base(id, tiles, nextBuildingTypeForCreation, availableBuildingForCreation, size, storeList, goals)
        {
        }
    }
}
