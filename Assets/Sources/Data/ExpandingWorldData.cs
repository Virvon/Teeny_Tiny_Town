using Assets.Sources.Services.StaticDataService.Configs.Building;
using System.Collections.Generic;

namespace Assets.Sources.Data
{
    public class ExpandingWorldData : WorldData
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
    }
}
