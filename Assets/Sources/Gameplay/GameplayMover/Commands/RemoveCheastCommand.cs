using Assets.Sources.Data.WorldDatas;
using Assets.Sources.Gameplay.World.WorldInfrastructure.NextBuildingForPlacing;
using Assets.Sources.Gameplay.World.WorldInfrastructure.WorldChangers;
using Assets.Sources.Services.PersistentProgress;
using UnityEngine;

namespace Assets.Sources.Gameplay.GameplayMover.Commands
{
    public class RemoveCheastCommand : MoveCommand
    {
        private readonly Vector2Int _cheastPosition;

        public RemoveCheastCommand(
            IWorldChanger world,
            IWorldData worldData,
            Vector2Int cheastPosition,
            NextBuildingForPlacingCreator nextBuildingForPlacingCreator,
            IPersistentProgressService persistentProgressService)
            : base(world, worldData, nextBuildingForPlacingCreator, persistentProgressService)
        {
            _cheastPosition = cheastPosition;
        }

        public override async void Execute()
        {
            await WorldChanger.RemoveBuilding(_cheastPosition);
            base.Execute();
        }
    }
}
