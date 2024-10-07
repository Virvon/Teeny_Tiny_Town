using Assets.Sources.Gameplay.World.WorldInfrastructure;
using Assets.Sources.Services.Input;
using System;
using UnityEngine;

namespace Assets.Sources.Gameplay.GameplayMover
{
    public class GameplayMover
    {
        private readonly WorldChanger _world;
        private readonly IInputService _inputService;

        private Command _lastCommand;

        public GameplayMover(WorldChanger world, IInputService inputService)
        {
            _world = world;
            _inputService = inputService;

            _inputService.UndoButtonPressed += TryUndoCommand;
        }

        ~GameplayMover()
        {
            _inputService.UndoButtonPressed -= TryUndoCommand;
        }

        public event Action GameplayMoved;

        public void PlaceNewBuilding(Vector2Int gridPosition) =>
            ExecuteCommand(new PlaceNewBuildingCommand(_world, gridPosition));

        public void RemoveBuilding(Vector2Int gridPosition) =>
            ExecuteCommand(new RemoveBuildingCommand(_world, gridPosition));

        public void ReplaceBuilding(Vector2Int fromGridPosition, BuildingType fromBuildingType, Vector2Int toGridPosition, BuildingType toBuildingType) =>
            ExecuteCommand(new ReplaceBuildingCommand(_world, fromGridPosition, fromBuildingType, toGridPosition, toBuildingType));

        public void TryUndoCommand()
        {
            if (_lastCommand == null)
                return;

            //_world.Update(_lastCommand.TileDatas, _lastCommand.BuildingForPlacing);
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
