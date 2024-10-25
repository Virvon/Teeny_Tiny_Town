using Assets.Sources.Data.WorldDatas;
using Assets.Sources.Gameplay.World.WorldInfrastructure.NextBuildingForPlacing;
using Assets.Sources.Gameplay.World.WorldInfrastructure.WorldChangers;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Assets.Sources.Gameplay.GameplayMover.Commands
{
    public class ExpandWorldCommand : Command
    {
        private readonly IExpandingWorldChanger _expandingWorldChanger;
        private readonly IWorldData _worldData;
        private readonly Vector2Int _size;
        private readonly Vector2Int _targetSize;
        private readonly Command _previousCommand;

        public ExpandWorldCommand(
            IExpandingWorldChanger expandingWorldChanger,
            IWorldData worldData,
            Vector2Int targetSize,
            Command previousCommand,
            NextBuildingForPlacingCreator nextBuildingForPlacingCreator)
            : base(expandingWorldChanger, worldData, nextBuildingForPlacingCreator)
        {
            _expandingWorldChanger = expandingWorldChanger;
            _worldData = worldData;
            _size = worldData.Size;
            _targetSize = targetSize;
            _previousCommand = previousCommand;
        }

        public override async void Execute()
        {
            _worldData.Size = _targetSize;

            await _expandingWorldChanger.Expand();
        }

        public override async UniTask Undo()
        {
            _worldData.Size = _size;

            await _expandingWorldChanger.Expand();
            await _previousCommand.Undo();
        }
    }
}
