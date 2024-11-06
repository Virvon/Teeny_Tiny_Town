using Assets.Sources.Services.StaticDataService;
using Assets.Sources.Services.StaticDataService.Configs.Building;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem.Utilities;

namespace Assets.Sources.Data.World
{
    [Serializable]
    public partial class WorldData : IWorldData
    {
        private const int InventorySize = 3;

        public string Id;
        public TileData[] Tiles;
        public BuildingType NextBuildingTypeForCreation;
        public uint NextBuildingForCreationBuildsCount;
        public List<BuildingType> AvailableBuildingsForCreation;
        public Vector2Int Size;
        public bool IsChangingStarted;
        public PointsData PointsData;
        public UpgradeData BulldozerItems;
        public UpgradeData ReplaceItems;
        public bool IsUnlocked;
        public BuildingType[] Inventory;

        public WorldData(
            string id,
            TileData[] tiles,
            BuildingType nextBuildingTypeForCreation,
            List<BuildingType> availableBuildingForCreation,
            Vector2Int size,
            uint[] goals,
            bool isUnlocked)
        {
            Id = id;
            Tiles = tiles;
            NextBuildingTypeForCreation = nextBuildingTypeForCreation;
            AvailableBuildingsForCreation = availableBuildingForCreation;
            Size = size;
            IsUnlocked = isUnlocked;

            NextBuildingForCreationBuildsCount = 0;
            IsChangingStarted = false;
            PointsData = new(goals);
            BulldozerItems = new();
            ReplaceItems = new();
            Inventory = new BuildingType[InventorySize];
        }

        public event Action<BuildingType> BuildingUpgraded;

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
        UpgradeData IWorldData.BulldozerItems => BulldozerItems;
        UpgradeData IWorldData.ReplaceItems => ReplaceItems;
        bool IWorldData.IsUnlocked
        {
            get => IsUnlocked;
            set => IsUnlocked = value;
        }
        BuildingType[] IWorldData.Inventory => Inventory;

        public void TryAddBuildingTypeForCreation(BuildingType createdBuilding, uint requiredCreatedBuildingsToAddNext, IStaticDataService staticDataService)
        {
            BuildingUpgraded?.Invoke(createdBuilding);

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

        public void UpdateTileDatas(TileData[] targetTileDatas)
        {
            foreach (TileData targetTileData in targetTileDatas)
            {
                TileData tileData = Tiles.First(value => value.GridPosition == targetTileData.GridPosition);
                tileData.BuildingType = targetTileData.BuildingType;
            }
        }

        public void UpdateAvailableBuildingForCreation(IReadOnlyList<BuildingType> availableBuildingsForCreation) =>
            AvailableBuildingsForCreation = availableBuildingsForCreation.Intersect(AvailableBuildingsForCreation).ToList();

        public void Update(TileData[] tiles, BuildingType nextBuildingTypeForCreation, List<BuildingType> availableBuildingsForCreation)
        {
            UpdateTileDatas(tiles);
            NextBuildingTypeForCreation = nextBuildingTypeForCreation;
            AvailableBuildingsForCreation = availableBuildingsForCreation;

            NextBuildingForCreationBuildsCount = 0;
            Inventory = new BuildingType[InventorySize];
        }
    }
}