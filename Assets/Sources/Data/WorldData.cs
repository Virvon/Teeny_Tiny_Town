using Assets.Sources.Services.StaticDataService.Configs.Building;
using System;
using System.Collections.Generic;

namespace Assets.Sources.Data
{
    [Serializable]
    public class WorldData
    {
        public List<TileData> Tiles;
        public BuildingType NextBuildingTypeForCreation;
        public uint NextBuildingForCreationBuildsCount;
        public List<BuildingType> AvailableBuildingForCreation;
        public WorldWallet WorldWallet;
        public uint Length;
        public uint Width;
        public List<BuildingType> StoreList;

        public WorldData(
            List<TileData> tiles,
            BuildingType nextBuildingTypeForCreation,
            List<BuildingType> availableBuildingForCreation,
            uint length,
            uint width,
            List<BuildingType> storeList)
        {
            Tiles = tiles;
            NextBuildingTypeForCreation = nextBuildingTypeForCreation;
            AvailableBuildingForCreation = availableBuildingForCreation;
            Length = length;
            Width = width;
            StoreList = storeList;

            WorldWallet = new();
            NextBuildingForCreationBuildsCount = 0;
        }

        public event Action<BuildingType> StoreListUpdated;

        public bool TryAddBuildingTypeForCreation(BuildingType createdBuilding, uint requiredCreatedBuildingsToAddNext)
        {
            if (NextBuildingTypeForCreation != createdBuilding)
                return false;

            NextBuildingForCreationBuildsCount++;

            return NextBuildingForCreationBuildsCount >= requiredCreatedBuildingsToAddNext;
        }

        public void AddNextBuildingTypeForCreation(BuildingType type)
        {
            AvailableBuildingForCreation.Add(NextBuildingTypeForCreation);
            NextBuildingTypeForCreation = type;
            NextBuildingForCreationBuildsCount = 0;

            if(StoreList.Contains(type) == false)
            {
                StoreList.Add(type);
                StoreListUpdated?.Invoke(type);
            }
        }
    }
}
