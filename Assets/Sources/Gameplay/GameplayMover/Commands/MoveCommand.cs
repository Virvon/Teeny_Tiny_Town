using Assets.Sources.Data.WorldDatas;
using Assets.Sources.Gameplay.World.WorldInfrastructure.NextBuildingForPlacing;
using Assets.Sources.Gameplay.World.WorldInfrastructure.WorldChangers;
using Assets.Sources.Services.PersistentProgress;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Assets.Sources.Gameplay.GameplayMover.Commands
{
    public abstract class MoveCommand : Command
    {
        private readonly IPersistentProgressService _persistentProgressService;

        private readonly uint _ramainMovesCount;


        public MoveCommand(
            IWorldChanger worldChanger,
            IWorldData worldData,
            NextBuildingForPlacingCreator nextBuildingForPlacingCreator,
            IPersistentProgressService persistentProgressService)
            : base(worldChanger, worldData, nextBuildingForPlacingCreator)
        {
            _persistentProgressService = persistentProgressService;

            _ramainMovesCount = _persistentProgressService.Progress.GameplayMovesCounter.RemainingMovesCount;
        }

        public override void Change() =>
            _persistentProgressService.Progress.GameplayMovesCounter.Move();

        public override async UniTask Undo()
        {
            await base.Undo();

            _persistentProgressService.Progress.GameplayMovesCounter.SetCount(_ramainMovesCount);
        }
    }
}
