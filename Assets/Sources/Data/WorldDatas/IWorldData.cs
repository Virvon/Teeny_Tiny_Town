﻿using Assets.Sources.Services.StaticDataService.Configs.Building;
using System.Collections.Generic;

namespace Assets.Sources.Data
{
    public interface IWorldData
    {
        IReadOnlyList<TileData> Tiles { get; }
        uint Length { get; set; }
        uint Width { get; set; }
        IReadOnlyList<BuildingType> AvailableBuildingsForCreation { get; }
        string Id { get; }
        BuildingType NextBuildingTypeForCreation { get; set; }
        uint NextBuildingForCreationBuildsCount { get; set; }

        void AddNextBuildingTypeForCreation(BuildingType type);
        bool TryAddBuildingTypeForCreation(BuildingType createdBuilding, uint requiredCreatedBuildingsToAddNext);
        void UpdateTileDatas(TileData[] tileDatas);
        void UpdateAvailableBuildingForCreation(IReadOnlyList<BuildingType> availableBuildingsForCreation);
    }
}