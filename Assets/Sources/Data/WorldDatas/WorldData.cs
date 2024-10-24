using Assets.Sources.Services.StaticDataService;
using Assets.Sources.Services.StaticDataService.Configs.Building;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem.Utilities;

namespace Assets.Sources.Data.WorldDatas
{
    [Serializable]
    public class WorldData : IWorldData
    {
        public string Id;
        public TileData[] Tiles;
        public BuildingType NextBuildingTypeForCreation;
        public uint NextBuildingForCreationBuildsCount;
        public List<BuildingType> AvailableBuildingsForCreation;
        public Vector2Int Size;
        public bool IsChangingStarted;
        public PointsData PointsData;

        public WorldData(
            string id,
            TileData[] tiles,
            BuildingType nextBuildingTypeForCreation,
            List<BuildingType> availableBuildingForCreation,
            Vector2Int size,
            uint[] goals)
        {
            Id = id;
            Tiles = tiles;
            NextBuildingTypeForCreation = nextBuildingTypeForCreation;
            AvailableBuildingsForCreation = availableBuildingForCreation;
            Size = size;

            NextBuildingForCreationBuildsCount = 0;
            IsChangingStarted = false;
            PointsData = new(goals);
        }

        public event Action<BuildingType> BuildingUpdated;

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
        ReadOnlyArray<TileData> IWorldData.Tiles => Tiles;
        IReadOnlyList<BuildingType> IWorldData.AvailableBuildingsForCreation => AvailableBuildingsForCreation;
        bool IWorldData.IsChangingStarted
        {
            get => IsChangingStarted;
            set => IsChangingStarted = value;
        }
        Vector2Int IWorldData.Size
        {
            get => Size;
            set => Size = value;
        }
        PointsData IWorldData.PointsData => PointsData;

        public void TryAddBuildingTypeForCreation(BuildingType createdBuilding, uint requiredCreatedBuildingsToAddNext, IStaticDataService staticDataService)
        {
            BuildingUpdated?.Invoke(createdBuilding);

            if (NextBuildingTypeForCreation != createdBuilding || NextBuildingTypeForCreation == BuildingType.Undefined)
                return;

            NextBuildingForCreationBuildsCount++;

            if (NextBuildingForCreationBuildsCount >= requiredCreatedBuildingsToAddNext)
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
            foreach (TileData targetTileData in TargetTileDatas)
            {
                TileData tileData = Tiles.First(value => value.GridPosition == targetTileData.GridPosition);
                tileData.BuildingType = targetTileData.BuildingType;
            }
        }

        public void UpdateAvailableBuildingForCreation(IReadOnlyList<BuildingType> availableBuildingsForCreation) =>
            AvailableBuildingsForCreation = availableBuildingsForCreation.Intersect(AvailableBuildingsForCreation).ToList();

        public void Update(TileData[] tiles, BuildingType nextBuildingTypeForCreation, List<BuildingType> availableBuildingsForCreation)
        {
            Tiles = tiles;
            NextBuildingTypeForCreation = nextBuildingTypeForCreation;
            AvailableBuildingsForCreation = availableBuildingsForCreation;

            NextBuildingForCreationBuildsCount = 0;
        }
    }
}