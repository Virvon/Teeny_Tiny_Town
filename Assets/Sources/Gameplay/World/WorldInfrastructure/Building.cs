using Assets.Sources.Services.StaticDataService.Configs.Building;
using UnityEngine;

namespace Assets.Sources.Gameplay.World.WorldInfrastructure
{
    public class Building
    {
        public readonly Vector2Int GridPosition;
        public readonly BuildingType Type;

        public Building(Vector2Int gridPosition, BuildingType type)
        {
            GridPosition = gridPosition;
            Type = type;
        }
    }
}
