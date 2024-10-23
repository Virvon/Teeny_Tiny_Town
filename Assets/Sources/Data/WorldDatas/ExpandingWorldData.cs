using Assets.Sources.Services.StaticDataService;
using Assets.Sources.Services.StaticDataService.Configs.Building;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Sources.Data.WorldDatas
{
    public class ExpandingWorldData : CurrencyWorldData, IExpandingWorldData
    {
        public ExpandingWorldData(
            string id,
            TileData[] tiles,
            BuildingType nextBuildingTypeForCreation,
            List<BuildingType> availableBuildingForCreation,
            Vector2Int size,
            List<BuildingType> storeList)
            : base(id, tiles, nextBuildingTypeForCreation, availableBuildingForCreation, size, storeList)
        {
        }

        public event Action<BuildingType> BuildingUpdated;

        public override void TryAddBuildingTypeForCreation(BuildingType createdBuilding, uint requiredCreatedBuildingsToAddNext, IStaticDataService staticDataService)
        {
            BuildingUpdated?.Invoke(createdBuilding);
            base.TryAddBuildingTypeForCreation(createdBuilding, requiredCreatedBuildingsToAddNext, staticDataService);
        }
    }
}
