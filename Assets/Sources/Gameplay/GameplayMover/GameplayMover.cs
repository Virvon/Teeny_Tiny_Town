using Assets.Sources.Gameplay.World.WorldInfrastructure;
using Assets.Sources.Services.Input;
using System;
using UnityEngine;

namespace Assets.Sources.Gameplay.GameplayMover
{
    public class GameplayMover
    {
        private readonly WorldChanger _worldChanger;
        private readonly IInputService _inputService;

        private Command _lastCommand;

        public GameplayMover(WorldChanger world, IInputService inputService)
        {
            _worldChanger = world;
            _inputService = inputService;

            _inputService.UndoButtonPressed += TryUndoCommand;
        }

        ~GameplayMover()
        {
            _inputService.UndoButtonPressed -= TryUndoCommand;
        }

        public event Action GameplayMoved;

        public void PlaceNewBuilding(Vector2Int gridPosition) =>
            ExecuteCommand(new PlaceNewBuildingCommand(_worldChanger, gridPosition));

        public void RemoveBuilding(Vector2Int gridPosition) =>
            ExecuteCommand(new RemoveBuildingCommand(_worldChanger, gridPosition));

        public void ReplaceBuilding(Vector2Int fromGridPosition, BuildingType fromBuildingType, Vector2Int toGridPosition, BuildingType toBuildingType) =>
            ExecuteCommand(new ReplaceBuildingCommand(_worldChanger, fromGridPosition, fromBuildingType, toGridPosition, toBuildingType));

        public void TryUndoCommand()
        {
            if (_lastCommand == null)
                return;

            _worldChanger.Update(_lastCommand.TileDatas, _lastCommand.BuildingForPlacing);
            _lastCommand = null;
        }

        private void ExecuteCommand(Command command)
        {
            _lastCommand = command;
            command.Change();
            GameplayMoved?.Invoke();
        }
    }
}
