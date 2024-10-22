using System.Collections.Generic;
using Random = UnityEngine.Random;
using Assets.Sources.Gameplay.World.WorldInfrastructure.Tiles;
using Assets.Sources.Services.StaticDataService.Configs.Building;
using Assets.Sources.Data.WorldDatas;
using System;

namespace Assets.Sources.Gameplay.World.WorldInfrastructure.NextBuildingForPlacing
{
    public class NextBuildingForPlacingCreator
    {
        private readonly IWorldData _worldData;

        public NextBuildingForPlacingCreator(IWorldData worldData) =>
            _worldData = worldData;

        public BuildingsForPlacingData BuildingsForPlacingData { get; private set; }

        public event Action<BuildingsForPlacingData> DataChanged;

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

        private BuildingType CreateBuildingType()
        {
            IReadOnlyList<BuildingType> availableBuildingTypes = _worldData.AvailableBuildingsForCreation;

            return availableBuildingTypes[Random.Range(0, availableBuildingTypes.Count)];
        }


        private void FindTileToPlacing(IReadOnlyList<Tile> tiles)
        {
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
    }
}
