using Assets.Sources.Gameplay.World.WorldInfrastructure;
using UnityEngine;

namespace Assets.Sources.Gameplay.GameplayMover
{
    public class ReplaceBuildingCommand : Command
    {
        private readonly Vector2Int _fromBuildingGridPosition;
        private readonly BuildingType _fromBuildingType;
        private readonly Vector2Int _toBuildingGridPosition;
        private readonly BuildingType _toBuildingType;

        public ReplaceBuildingCommand(WorldChanger world, Vector2Int fromBuildingGridPosition, BuildingType fromBuildingType, Vector2Int toBuildingGridPosition, BuildingType toBuildingType) : base(world)
        {
            _fromBuildingGridPosition = fromBuildingGridPosition;
            _fromBuildingType = fromBuildingType;
            _toBuildingGridPosition = toBuildingGridPosition;
            _toBuildingType = toBuildingType;
        }

        public override async void Change()
        {
            await WorldChanger.ReplaceBuilding(_fromBuildingGridPosition, _fromBuildingType, _toBuildingGridPosition, _toBuildingType);
        }
    }
}
