using UnityEngine;
using Assets.Sources.Gameplay.Tile;

namespace Assets.Sources.Gameplay.WorldGenerator.World
{
    public class BuildingModel
    {
        public readonly Vector2Int GridPosition;
        public readonly BuildingType Type;

        public BuildingModel(Vector2Int gridPosition, BuildingType type)
        {
            GridPosition = gridPosition;
            Type = type;
        }
    }
}
