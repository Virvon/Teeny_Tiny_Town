using Assets.Sources.Services.StaticDataService;
using Assets.Sources.Services.StaticDataService.Configs.Building;
using System;
using System.Collections.Generic;

namespace Assets.Sources.Data.WorldDatas
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

        public override void TryAddBuildingTypeForCreation(BuildingType createdBuilding, uint requiredCreatedBuildingsToAddNext, IStaticDataService staticDataService)
        {
            BuildingUpdated?.Invoke(createdBuilding);
            base.TryAddBuildingTypeForCreation(createdBuilding, requiredCreatedBuildingsToAddNext, staticDataService);
        }
    }
}
