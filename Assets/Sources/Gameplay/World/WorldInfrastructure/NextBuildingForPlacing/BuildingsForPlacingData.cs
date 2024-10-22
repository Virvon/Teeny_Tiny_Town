using Assets.Sources.Services.StaticDataService.Configs.Building;
using UnityEngine;

namespace Assets.Sources.Gameplay.World.WorldInfrastructure.NextBuildingForPlacing
{
    public class BuildingsForPlacingData
    {
        public Vector2Int StartGridPosition;
        public BuildingType CurrentBuildingType;
        public BuildingType NextBuildingType;

        public BuildingsForPlacingData(BuildingType type, BuildingType nextBuildingType)
        {
            CurrentBuildingType = type;
            NextBuildingType = nextBuildingType;
        }

        public BuildingsForPlacingData(Vector2Int startGridPosition, BuildingType currentBuildingType, BuildingType nextBuildingType)
        {
            StartGridPosition = startGridPosition;
            CurrentBuildingType = currentBuildingType;
            NextBuildingType = nextBuildingType;
        }

        public void MoveToNext() =>
            CurrentBuildingType = NextBuildingType;
    }
}
