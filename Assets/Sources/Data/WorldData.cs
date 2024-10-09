using Assets.Sources.Gameplay.World.WorldInfrastructure;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Sources.Data
{
    [Serializable]
    public class WorldData
    {
        public List<TileData> Tiles;
        public BuildingType NextBuildingTypeForCreation;
        public uint NextBuildingForCreationBuildsCount;
        public List<BuildingType> AvailableBuildingForCreation;

        public WorldData(List<TileData> tiles, BuildingType nextBuildingTypeForCreation, List<BuildingType> availableBuildingForCreation)
        {
            Tiles = tiles;
            NextBuildingTypeForCreation = nextBuildingTypeForCreation;
            AvailableBuildingForCreation = availableBuildingForCreation;

            NextBuildingForCreationBuildsCount = 0;
        }

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
        }
    }
}
