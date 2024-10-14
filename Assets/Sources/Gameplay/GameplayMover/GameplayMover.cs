using Assets.Sources.Data;
using Assets.Sources.Gameplay.GameplayMover.Commands;
using Assets.Sources.Gameplay.World.WorldInfrastructure.WorldChangers;
using Assets.Sources.Services.Input;
using Assets.Sources.Services.PersistentProgress;
using Assets.Sources.Services.StaticDataService.Configs.Building;
using UnityEngine;

namespace Assets.Sources.Gameplay.GameplayMover
{
    public class GameplayMover : IGameplayMover
    {
        private readonly IWorldChanger _worldChanger;
        private readonly IInputService _inputService;
        private readonly IPersistentProgressService _persistentProgressService;

        protected readonly IWorldData WorldData;

        public GameplayMover(
            IWorldChanger worldChanger,
            IInputService inputService,
            IWorldData worldData,
            IPersistentProgressService persistentProgressService)
        {
            _worldChanger = worldChanger;
            _inputService = inputService;
            WorldData = worldData;

            _inputService.UndoButtonPressed += TryUndoCommand;
            _persistentProgressService = persistentProgressService;
        }

        ~GameplayMover()
        {
            _inputService.UndoButtonPressed -= TryUndoCommand;
        }

        protected Command LastCommand { get; private set; }

        public void PlaceNewBuilding(Vector2Int gridPosition) =>
            ExecuteCommand(new PlaceNewBuildingCommand(_worldChanger, gridPosition, WorldData));

        public void RemoveBuilding(Vector2Int gridPosition) =>
            ExecuteCommand(new RemoveBuildingCommand(_worldChanger, WorldData, gridPosition));

        public void ReplaceBuilding(Vector2Int fromGridPosition, BuildingType fromBuildingType, Vector2Int toGridPosition, BuildingType toBuildingType) =>
            ExecuteCommand(new ReplaceBuildingCommand(_worldChanger, WorldData, fromGridPosition, fromBuildingType, toGridPosition, toBuildingType));

        public void OpenChest(Vector2Int chestGridPosition, uint reward) =>
            ExecuteCommand(new OpenChestCommand(_worldChanger, WorldData, reward, chestGridPosition, WorldData.WorldWallet));

        public void ChangeBuildingForPlacing(BuildingType targetBuildingType, uint buildingPrice) =>
            ExecuteCommand(new ChangeBuildingForPlacingCommand(_worldChanger, WorldData, targetBuildingType, buildingPrice, WorldData.WorldWallet));

        public async void TryUndoCommand()
        {
            if (LastCommand == null)
                return;

            await LastCommand.Undo();
            LastCommand = null;
        }

        protected void ExecuteCommand(Command command)
        {
            LastCommand = command;
            command.Change();
            _persistentProgressService.Progress.MoveCounter.Move();
        }
    }
}
