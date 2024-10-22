using Assets.Sources.Data.WorldDatas;
using Assets.Sources.Gameplay.World.WorldInfrastructure.NextBuildingForPlacing;
using Assets.Sources.Gameplay.World.WorldInfrastructure.WorldChangers;
using UnityEngine;

namespace Assets.Sources.Gameplay.GameplayMover.Commands
{
    public class RemoveBuildingCommand : Command
    {
        private readonly Vector2Int _removedBuildingGridPosition;

        public RemoveBuildingCommand(
            IWorldChanger world,
            IWorldData worldData,
            Vector2Int removedBuildingGridPosition,
            NextBuildingForPlacingCreator nextBuildingForPlacingCreator)
            : base(world, worldData, nextBuildingForPlacingCreator) =>
            _removedBuildingGridPosition = removedBuildingGridPosition;

        public override async void Change()
        {
            await WorldChanger.RemoveBuilding(_removedBuildingGridPosition);
        }
    }
}
