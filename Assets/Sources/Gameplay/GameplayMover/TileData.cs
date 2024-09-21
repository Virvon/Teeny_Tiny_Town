using Assets.Sources.Gameplay.World.WorldInfrastructure;
using Assets.Sources.Services.StaticDataService.Configs;
using UnityEngine;

namespace Assets.Sources.Gameplay.GameplayMover
{
    public class TileData
    {
        public readonly Vector2Int GridPosition;
        public readonly BuildingType BuildingType;
        public readonly GroundType GroundType;
        public readonly GroundRotation GroundRotation;

        public TileData(Vector2Int tileGridPosition, BuildingType buildingType, GroundType groundType, GroundRotation groundRotation)
        {
            GridPosition = tileGridPosition;
            BuildingType = buildingType;
            GroundType = groundType;
            GroundRotation = groundRotation;
        }
    }
}
