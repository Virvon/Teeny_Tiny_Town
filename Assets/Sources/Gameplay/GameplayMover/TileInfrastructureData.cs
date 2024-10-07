using Assets.Sources.Gameplay.World.WorldInfrastructure;
using UnityEngine;

namespace Assets.Sources.Gameplay.GameplayMover
{
    public class TileInfrastructureData
    {
        public readonly Vector2Int GridPosition;
        public readonly BuildingType BuildingType;

        public TileInfrastructureData(Vector2Int tileGridPosition, BuildingType buildingType)
        {
            GridPosition = tileGridPosition;
            BuildingType = buildingType;
        }
    }
}
