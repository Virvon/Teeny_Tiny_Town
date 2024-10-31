using Assets.Sources.Gameplay.GameplayMover;
using Assets.Sources.Gameplay.World.RepresentationOfWorld.Markers;
using Assets.Sources.Services.StaticDataService.Configs.Building;
using Assets.Sources.Services.StaticDataService.Configs.World;
using UnityEngine;

namespace Assets.Sources.Gameplay.World.RepresentationOfWorld.ActionHandler
{
    public abstract class WorldActionHandlerState : ActionHandlerState
    {
        protected readonly IGameplayMover GameplayMover;

        protected WorldActionHandlerState(SelectFrame selectFrame, LayerMask layerMask, IGameplayMover gameplayMover)
            : base(selectFrame, layerMask)
        {
            GameplayMover = gameplayMover;
        }

        protected bool CheckBuildingAndTileCompatibility(BuildingType buildingType, TileType tileType)
        {
            return ((buildingType == BuildingType.Lighthouse && tileType != TileType.WaterSurface)
                || (buildingType != BuildingType.Lighthouse && tileType == TileType.WaterSurface))
                == false;
        }
    }
}
