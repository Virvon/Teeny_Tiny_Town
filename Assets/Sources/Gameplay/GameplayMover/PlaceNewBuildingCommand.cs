using Assets.Sources.Gameplay.World.WorldInfrastructure;
using UnityEngine;

namespace Assets.Sources.Gameplay.GameplayMover
{
    public class PlaceNewBuildingCommand : Command
    {
        private readonly Vector2Int _placedBuildingGridPosition;
        private readonly BuildingType _placedBuildingType;

        public PlaceNewBuildingCommand(World.WorldInfrastructure.World world, Vector2Int placedBuildingGridPosition, BuildingType placedBuildingType)
            : base(world)
        {
            _placedBuildingGridPosition = placedBuildingGridPosition;
            _placedBuildingType = placedBuildingType;
        }

        public override void Change() =>
            World.Update(_placedBuildingGridPosition, _placedBuildingType);
    }
}
