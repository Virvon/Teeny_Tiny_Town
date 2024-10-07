using UnityEngine;

namespace Assets.Sources.Gameplay.GameplayMover
{
    public class RemoveBuildingCommand : Command
    {
        private readonly Vector2Int _removedBuildingGridPosition;

        public RemoveBuildingCommand(World.WorldInfrastructure.WorldChanger world, Vector2Int removedBuildingGridPosition)
            : base(world) =>
            _removedBuildingGridPosition = removedBuildingGridPosition;

        public override void Change()
        {
            WorldChanger.RemoveBuilding(_removedBuildingGridPosition);
        }
    }
}
