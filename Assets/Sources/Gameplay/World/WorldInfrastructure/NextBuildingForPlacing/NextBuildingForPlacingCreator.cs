using System.Collections.Generic;
using Random = UnityEngine.Random;
using Assets.Sources.Gameplay.World.WorldInfrastructure.Tiles;
using Assets.Sources.Services.StaticDataService.Configs.Building;
using System;
using System.Linq;
using Assets.Sources.Services.StaticDataService.Configs.World;
using UnityEngine;
using Assets.Sources.Data.World;
using Assets.Sources.Services.StaticDataService;

namespace Assets.Sources.Gameplay.World.WorldInfrastructure.NextBuildingForPlacing
{
    public class NextBuildingForPlacingCreator
    {
        private readonly IWorldData _worldData;
        private readonly IStaticDataService _staticDataService;

        public NextBuildingForPlacingCreator(IWorldData worldData, IStaticDataService staticDataService)
        {
            _worldData = worldData;
            _staticDataService = staticDataService;
        }

        public BuildingsForPlacingData BuildingsForPlacingData { get; private set; }

        public event Action<BuildingsForPlacingData> DataChanged;
        public event Action NoMoreEmptyTiles;

        public void CreateData(IReadOnlyList<Tile> tiles)
        {
            BuildingsForPlacingData = new(CreateBuildingType(), CreateBuildingType());

            FindTileToPlacing(tiles);

            DataChanged?.Invoke(BuildingsForPlacingData);
        }

        public void Update(BuildingsForPlacingData newData)
        {
            BuildingsForPlacingData = newData;

            DataChanged?.Invoke(BuildingsForPlacingData);
        }

        public void ChangeCurrentBuildingForPlacing(BuildingType type)
        {
            BuildingsForPlacingData.CurrentBuildingType = type;

            DataChanged?.Invoke(BuildingsForPlacingData);
        }

        public void MoveToNextBuilding()
        {
            BuildingsForPlacingData.MoveToNext();
            BuildingsForPlacingData.NextBuildingType = CreateBuildingType();

            DataChanged?.Invoke(BuildingsForPlacingData);
        }

        public void MoveToNextBuilding(IReadOnlyList<Tile> tiles)
        {
            FindTileToPlacing(tiles);
            MoveToNextBuilding();
        }

        public void FindTileToPlacing(IReadOnlyList<Tile> tiles)
        {
            if (tiles.Any(tile => tile.IsEmpty && tile.Type != TileType.WaterSurface) == false)
            {
                NoMoreEmptyTiles?.Invoke();
                return;
            }

            bool isPositionFree = false;

            while (isPositionFree == false)
            {
                Tile tile = tiles[Random.Range(0, tiles.Count)];

                if (tile.IsEmpty)
                {
                    BuildingsForPlacingData.StartGridPosition = tile.GridPosition;
                    isPositionFree = true;
                }
            }
        }

        private BuildingType CreateBuildingType()
        {
            IReadOnlyList<BuildingType> availableBuildingTypes = _worldData.AvailableBuildingsForCreation;

            BuildingConfig[] buildingConfigs = availableBuildingTypes
                .Select(buildingType => _staticDataService.GetBuilding<BuildingConfig>(buildingType))
                .OrderBy(buildingConfig => buildingConfig.ProportionOfLoss)
                .ToArray();

            int proportionsOfLossSum = (int)buildingConfigs.Sum(value => value.ProportionOfLoss);

            int resultChance = Random.Range(0, proportionsOfLossSum) + 1;
            uint chance = 0;

            for (int i = 0; i < buildingConfigs.Length; i++)
            {
                chance += buildingConfigs[i].ProportionOfLoss;

                if (resultChance <= chance)
                    return buildingConfigs[i].BuildingType;
            }

            Debug.LogError("Reward type not founded");
            return BuildingType.Bush;
        }       
    }
}
