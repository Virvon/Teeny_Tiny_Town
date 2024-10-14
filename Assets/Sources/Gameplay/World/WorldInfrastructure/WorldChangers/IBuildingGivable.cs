using Assets.Sources.Gameplay.World.WorldInfrastructure.Tiles.Buildings;
using Assets.Sources.Services.StaticDataService.Configs.Building;
using UnityEngine;

namespace Assets.Sources.Gameplay.World.WorldInfrastructure
{
    public interface IBuildingGivable
    {
        Building GetBuilding(BuildingType type, Vector2Int gridPosition);
    }
}