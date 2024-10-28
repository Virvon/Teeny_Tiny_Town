using Assets.Sources.Data.WorldDatas;
using Assets.Sources.Gameplay.World.WorldInfrastructure.NextBuildingForPlacing;
using Assets.Sources.Gameplay.World.WorldInfrastructure.WorldChangers;
using Assets.Sources.Services.PersistentProgress;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Assets.Sources.Gameplay.GameplayMover.Commands
{
    public class RemoveBuildingCommand : MoveCommand
    {
        private readonly Vector2Int _removedBuildingGridPosition;
        private readonly uint _bulldozerItemsCount;

        public RemoveBuildingCommand(
            IWorldChanger world,
            IWorldData worldData,
            Vector2Int removedBuildingGridPosition,
            NextBuildingForPlacingCreator nextBuildingForPlacingCreator,
            IPersistentProgressService persistentProgressService)
            : base(world, worldData, nextBuildingForPlacingCreator, persistentProgressService)
        {
            _removedBuildingGridPosition = removedBuildingGridPosition;

            _bulldozerItemsCount = WorldData.BulldozerItems.Count;
        }

        public override async void Execute()
        {
            if (WorldData.BulldozerItems.TryGet() == false)
                Debug.LogError("Has not bulozer items");

            await WorldChanger.RemoveBuilding(_removedBuildingGridPosition);
            base.Execute();
        }

        public override async UniTask Undo()
        {
            await base.Undo();
            WorldData.BulldozerItems.SetItemsCount(_bulldozerItemsCount);
        }
    }
}
