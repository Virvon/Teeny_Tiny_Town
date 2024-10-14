using Assets.Sources.Data;
using Assets.Sources.Gameplay.World.WorldInfrastructure.WorldChangers;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Assets.Sources.Gameplay.GameplayMover.Commands
{
    public class ExpandWorldCommand : Command
    {
        private readonly IExpandingWorldChanger _expandingWorldChanger;
        private readonly IWorldData _worldData;
        private readonly uint _length;
        private readonly uint _width;
        private readonly uint _targetLength;
        private readonly uint _targetWidth;
        private readonly Command _previousCommand;

        public ExpandWorldCommand(IExpandingWorldChanger expandingWorldChanger, IWorldData worldData, uint targetLength, uint targetWidth, Command previousCommand)
            : base(expandingWorldChanger, worldData)
        {
            _expandingWorldChanger = expandingWorldChanger;
            _worldData = worldData;
            _length = worldData.Length;
            _width = worldData.Width;
            _targetLength = targetLength;
            _targetWidth = targetWidth;
            _previousCommand = previousCommand;
        }

        public override async void Change()
        {
            _worldData.Length = _targetLength;
            _worldData.Width = _targetWidth;

            await _expandingWorldChanger.Expand();
        }

        public override async UniTask Undo()
        {
            _worldData.Length = _length;
            _worldData.Width = _width;

            await _expandingWorldChanger.Expand();
            await _previousCommand.Undo();
        }
    }
}
