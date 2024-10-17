using Assets.Sources.Services.StaticDataService.Configs.Building;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Sources.Data
{
    public class ExpandingWorldData : CurrencyWorldData, IExpandingWorldData
    {
        public ExpandingWorldData(
            string id,
            List<TileData> tiles,
            BuildingType nextBuildingTypeForCreation,
            List<BuildingType> availableBuildingForCreation,
            uint length,
            uint width,
            List<BuildingType> storeList)
            : base(id, tiles, nextBuildingTypeForCreation, availableBuildingForCreation, length, width, storeList)
        {
        }

        public event Action<BuildingType> BuildingUpdated;

        public override bool TryAddBuildingTypeForCreation(BuildingType createdBuilding, uint requiredCreatedBuildingsToAddNext)
        {
            BuildingUpdated?.Invoke(createdBuilding);
            return base.TryAddBuildingTypeForCreation(createdBuilding, requiredCreatedBuildingsToAddNext);
        }
    }
}
