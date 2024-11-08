using System;
using System.Collections.Generic;
using Assets.Sources.Services.StaticDataService;
using Assets.Sources.Services.StaticDataService.Configs.Building;
using UnityEngine;
using UnityEngine.InputSystem.Utilities;

namespace Assets.Sources.Data.World
{
    public interface IWorldData
    {
        event Action<BuildingType> BuildingUpgraded;

        ReadOnlyArray<TileData> Tiles { get; }
        Vector2Int Size { get; set; }
        IReadOnlyList<BuildingType> AvailableBuildingsForCreation { get; }
        string Id { get; }
        BuildingType NextBuildingTypeForCreation { get; set; }
        uint NextBuildingForCreationBuildsCount { get; set; }
        bool IsChangingStarted { get; set; }
        PointsData PointsData { get; }
        UpgradeData BulldozerItems { get; }
        UpgradeData ReplaceItems { get; }
        bool IsUnlocked { get; set; }
        public BuildingType[] Inventory { get; }

        void TryAddBuildingTypeForCreation(BuildingType createdBuilding, uint requiredCreatedBuildingsToAddNext, IStaticDataService staticDataService);
        void UpdateTileDatas(TileData[] tileDatas);
        void UpdateAvailableBuildingForCreation(IReadOnlyList<BuildingType> availableBuildingsForCreation);
        void Update(TileData[] tiles, BuildingType nextBuildingTypeForCreation, List<BuildingType> availableBuildingsForCreation);
    }
}