using Assets.Sources.Services.StaticDataService;
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
        
        public uint Length;
        public uint Width;
        
        public WorldData(
            string id,
            List<TileData> tiles,
            BuildingType nextBuildingTypeForCreation,
            List<BuildingType> availableBuildingForCreation,
            uint length,
            uint width)
        {
            Tiles = tiles;
            NextBuildingTypeForCreation = nextBuildingTypeForCreation;
            AvailableBuildingsForCreation = availableBuildingForCreation;
            Length = length;
            Width = width;

            NextBuildingForCreationBuildsCount = 0;
            Id = id;
        }
        
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

        public virtual void TryAddBuildingTypeForCreation(BuildingType createdBuilding, uint requiredCreatedBuildingsToAddNext, IStaticDataService staticDataService)
        {
            if (NextBuildingTypeForCreation != createdBuilding || NextBuildingTypeForCreation == BuildingType.Undefined)
                return;

            NextBuildingForCreationBuildsCount++;

            if(NextBuildingForCreationBuildsCount >= requiredCreatedBuildingsToAddNext)
            {
                if (staticDataService.AvailableForConstructionBuildingsConfig.TryFindeNextBuilding(createdBuilding, out BuildingType nextBuildingType))
                    AddNextBuildingTypeForCreation(nextBuildingType);
                else
                    NextBuildingTypeForCreation = BuildingType.Undefined;
            }
        }

        protected virtual void AddNextBuildingTypeForCreation(BuildingType type)
        {
            AvailableBuildingsForCreation.Add(NextBuildingTypeForCreation);
            NextBuildingTypeForCreation = type;
            NextBuildingForCreationBuildsCount = 0;
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
