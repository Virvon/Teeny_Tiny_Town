using Assets.Sources.Gameplay.World.WorldInfrastructure;
using Assets.Sources.Services.Input;
using Assets.Sources.Services.StaticDataService;
using System;
using UnityEngine;

namespace Assets.Sources.Gameplay.GameplayMover
{
    public class GameplayMover
    {
        private readonly WorldChanger _worldChanger;
        private readonly IInputService _inputService;
        private readonly World.World _world;
        private readonly IStaticDataService _staticDataService;

        private Command _lastCommand;

        public GameplayMover(WorldChanger worldChanger, IInputService inputService, World.World world, IStaticDataService staticDataService)
        {
            _worldChanger = worldChanger;
            _inputService = inputService;
            _world = world;

            _inputService.UndoButtonPressed += TryUndoCommand;
            _staticDataService = staticDataService;
        }

        ~GameplayMover()
        {
            _inputService.UndoButtonPressed -= TryUndoCommand;
        }

        public event Action GameplayMoved;

        public void PlaceNewBuilding(Vector2Int gridPosition) =>
            ExecuteCommand(new PlaceNewBuildingCommand(_worldChanger, gridPosition, _world.WorldData, _staticDataService));

        public void RemoveBuilding(Vector2Int gridPosition) =>
            ExecuteCommand(new RemoveBuildingCommand(_worldChanger, gridPosition));

        public void ReplaceBuilding(Vector2Int fromGridPosition, BuildingType fromBuildingType, Vector2Int toGridPosition, BuildingType toBuildingType) =>
            ExecuteCommand(new ReplaceBuildingCommand(_worldChanger, fromGridPosition, fromBuildingType, toGridPosition, toBuildingType));

        public async void TryUndoCommand()
        {
            if (_lastCommand == null)
                return;

            await _lastCommand.Undo();
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
