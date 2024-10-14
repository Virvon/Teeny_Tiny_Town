using Assets.Sources.Services.StaticDataService.Configs.Building;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Assets.Sources.Data
{
    [Serializable]
    public class WorldData : IWorldData
    {
        public string Id;
        public List<TileData> Tiles;
        public BuildingType NextBuildingTypeForCreation;
        public uint NextBuildingForCreationBuildsCount;
        public List<BuildingType> AvailableBuildingsForCreation;
        public WorldWallet WorldWallet;
        public uint Length;
        public uint Width;
        public List<BuildingType> StoreList;

        public WorldData(
            string id,
            List<TileData> tiles,
            BuildingType nextBuildingTypeForCreation,
            List<BuildingType> availableBuildingForCreation,
            uint length,
            uint width,
            List<BuildingType> storeList)
        {
            Tiles = tiles;
            NextBuildingTypeForCreation = nextBuildingTypeForCreation;
            AvailableBuildingsForCreation = availableBuildingForCreation;
            Length = length;
            Width = width;
            StoreList = storeList;

            WorldWallet = new();
            NextBuildingForCreationBuildsCount = 0;
            Id = id;
        }

        public event Action<BuildingType> StoreListUpdated;
        WorldWallet IWorldData.WorldWallet => WorldWallet;
        string IWorldData.Id => Id;
        BuildingType IWorldData.NextBuildingTypeForCreation
        {
            get => NextBuildingTypeForCreation;
            set => NextBuildingTypeForCreation = value;
        }
        uint IWorldData.NextBuildingForCreationBuildsCount
        {
            get => NextBuildingForCreationBuildsCount;
            set => NextBuildingForCreationBuildsCount = value;
        }
        uint IWorldData.Length
        {
            get => Length;
            set => Length = value;
        }
        uint IWorldData.Width
        {
            get => Width;
            set => Width = value;
        }

        IReadOnlyList<TileData> IWorldData.Tiles => Tiles;

        IReadOnlyList<BuildingType> IWorldData.AvailableBuildingsForCreation => AvailableBuildingsForCreation;


        public virtual bool TryAddBuildingTypeForCreation(BuildingType createdBuilding, uint requiredCreatedBuildingsToAddNext)
        {
            if (NextBuildingTypeForCreation != createdBuilding)
                return false;

            NextBuildingForCreationBuildsCount++;

            return NextBuildingForCreationBuildsCount >= requiredCreatedBuildingsToAddNext;
        }

        public void AddNextBuildingTypeForCreation(BuildingType type)
        {
            AvailableBuildingsForCreation.Add(NextBuildingTypeForCreation);
            NextBuildingTypeForCreation = type;
            NextBuildingForCreationBuildsCount = 0;

            if (StoreList.Contains(type) == false)
            {
                StoreList.Add(type);
                StoreListUpdated?.Invoke(type);
            }
        }

        public void UpdateTileDatas(TileData[] TargetTileDatas)
        {
            foreach(TileData targetTileData in TargetTileDatas)
            {
                TileData tileData = Tiles.First(value => value.GridPosition == targetTileData.GridPosition);
                tileData.BuildingType = targetTileData.BuildingType;
            }
        }

        public void UpdateAvailableBuildingForCreation(IReadOnlyList<BuildingType> availableBuildingsForCreation) =>
            AvailableBuildingsForCreation = availableBuildingsForCreation.Intersect(AvailableBuildingsForCreation).ToList();
    }
}
