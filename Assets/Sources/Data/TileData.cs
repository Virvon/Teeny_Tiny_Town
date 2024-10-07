using Assets.Sources.Gameplay.World.WorldInfrastructure;
using Assets.Sources.Services.StaticDataService.Configs.World;
using UnityEngine;

namespace Assets.Sources.Data
{
    public class TileData
    {
        public Vector2Int GridPosition;
        public BuildingType BuildingType;
        public TileType Type;

        public TileData(Vector2Int gridPosition, BuildingType buildingType, TileType type)
        {
            GridPosition = gridPosition;
            BuildingType = buildingType;
            Type = type;
        }
    }
}
