using Assets.Sources.Services.StaticDataService.Configs.Building;
using UnityEngine;

namespace Assets.Sources.Gameplay.World.WorldInfrastructure.Tiles.Buildings
{
    public class BuildingForPlacingInfo
    {
        public readonly Vector2Int GridPosition;
        public readonly BuildingType Type;

        public BuildingForPlacingInfo(Vector2Int gridPosition, BuildingType type)
        {
            GridPosition = gridPosition;
            Type = type;
        }
    }
}
