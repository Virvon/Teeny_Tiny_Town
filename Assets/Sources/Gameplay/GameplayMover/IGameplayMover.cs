using Assets.Sources.Services.StaticDataService.Configs.Building;
using UnityEngine;

namespace Assets.Sources.Gameplay.GameplayMover
{
    public interface IGameplayMover
    {
        void OpenChest(Vector2Int chestGridPosition, uint reward);
        void PlaceNewBuilding(Vector2Int gridPosition, BuildingType type);
        void RemoveBuilding(Vector2Int gridPosition);
        void ReplaceBuilding(Vector2Int fromGridPosition, BuildingType fromBuildingType, Vector2Int toGridPosition, BuildingType toBuildingType);
        void TryUndoCommand();
    }
}