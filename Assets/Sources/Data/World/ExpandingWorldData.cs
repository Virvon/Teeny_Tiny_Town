using Assets.Sources.Data.World.Currency;
using Assets.Sources.Services.StaticDataService;
using Assets.Sources.Services.StaticDataService.Configs.Building;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Sources.Data.World
{
    public class ExpandingWorldData : CurrencyWorldData, ICurrencyWorldData
    {
        public ExpandingWorldData(
            string id,
            TileData[] tiles,
            BuildingType nextBuildingTypeForCreation,
            List<BuildingType> availableBuildingForCreation,
            Vector2Int size,
            BuildingType[] startBuildingsStoreList,
            uint[] goals,
            GainStoreItemData[] gainsStoreList,
            bool isUnlocked)
            : base(id, tiles, nextBuildingTypeForCreation, availableBuildingForCreation, size, startBuildingsStoreList, goals, gainsStoreList, isUnlocked)
        {
        }
    }
}
