using Assets.Sources.Data;
using Assets.Sources.Gameplay.World.WorldInfrastructure;
using Assets.Sources.Services.Input;
using Assets.Sources.Services.PersistentProgress;
using Assets.Sources.Services.StaticDataService.Configs.Building;
using System;
using UnityEngine;

namespace Assets.Sources.Gameplay.GameplayMover
{
    public class GameplayMover
    {
        private readonly WorldChanger _worldChanger;
        private readonly IInputService _inputService;
        private readonly WorldData _worldData;
        private readonly IPersistentProgressService _persistentProgressService;

        private Command _lastCommand;

        public GameplayMover(
            WorldChanger worldChanger,
            IInputService inputService,
            WorldData worldData,
            IPersistentProgressService persistentProgressService)
        {
            _worldChanger = worldChanger;
            _inputService = inputService;
            _worldData = worldData;

            _inputService.UndoButtonPressed += TryUndoCommand;
            _persistentProgressService = persistentProgressService;
        }

        ~GameplayMover()
        {
            _inputService.UndoButtonPressed -= TryUndoCommand;
        }

        public void PlaceNewBuilding(Vector2Int gridPosition) =>
            ExecuteCommand(new PlaceNewBuildingCommand(_worldChanger, gridPosition, _worldData));

        public void RemoveBuilding(Vector2Int gridPosition) =>
            ExecuteCommand(new RemoveBuildingCommand(_worldChanger, gridPosition));

        public void ReplaceBuilding(Vector2Int fromGridPosition, BuildingType fromBuildingType, Vector2Int toGridPosition, BuildingType toBuildingType) =>
            ExecuteCommand(new ReplaceBuildingCommand(_worldChanger, fromGridPosition, fromBuildingType, toGridPosition, toBuildingType));

        public void OpenChest(Vector2Int chestGridPosition, uint reward) =>
            ExecuteCommand(new OpenChestCommand(_worldChanger, reward, chestGridPosition, _worldData.WorldWallet));

        public void ChangeBuildingForPlacing(BuildingType targetBuildingType, uint buildingPrice) =>
            ExecuteCommand(new ChangeBuildingForPlacingCommand(_worldChanger, targetBuildingType, buildingPrice, _worldData.WorldWallet));

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
            _persistentProgressService.Progress.MoveCounter.Move();
        }
    }
}
