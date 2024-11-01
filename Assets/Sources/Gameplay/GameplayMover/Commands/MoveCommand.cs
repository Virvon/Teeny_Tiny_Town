using Assets.Sources.Data.WorldDatas;
using Assets.Sources.Gameplay.World.WorldInfrastructure.NextBuildingForPlacing;
using Assets.Sources.Gameplay.World.WorldInfrastructure.WorldChangers;
using Assets.Sources.Services.PersistentProgress;
using Cysharp.Threading.Tasks;

namespace Assets.Sources.Gameplay.GameplayMover.Commands
{
    public abstract class MoveCommand : Command
    {
        protected readonly IPersistentProgressService PersistentProgressService;

        private readonly uint _ramainMovesCount;

        public MoveCommand(
            IWorldChanger worldChanger,
            IWorldData worldData,
            NextBuildingForPlacingCreator nextBuildingForPlacingCreator,
            IPersistentProgressService persistentProgressService)
            : base(worldChanger, worldData, nextBuildingForPlacingCreator)
        {
            PersistentProgressService = persistentProgressService;

            _ramainMovesCount = PersistentProgressService.Progress.GameplayMovesCounter.RemainingMovesCount;
        }

        public override void Execute() =>
            PersistentProgressService.Progress.GameplayMovesCounter.Move();

        public override async UniTask Undo()
        {
            PersistentProgressService.Progress.GameplayMovesCounter.SetCount(_ramainMovesCount);

            await base.Undo();
        }
    }
}
