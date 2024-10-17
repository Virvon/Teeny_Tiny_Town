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
        private readonly IInputService _inputService;
        private readonly IPersistentProgressService _persistentProgressService;

        protected readonly IWorldChanger WorldChanger;
        protected readonly IWorldData WorldData;

        public GameplayMover(
            IWorldChanger worldChanger,
            IInputService inputService,
            IWorldData worldData,
            IPersistentProgressService persistentProgressService)
        {
            WorldChanger = worldChanger;
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
            ExecuteCommand(new PlaceNewBuildingCommand(WorldChanger, gridPosition, WorldData));

        public void RemoveBuilding(Vector2Int gridPosition) =>
            ExecuteCommand(new RemoveBuildingCommand(WorldChanger, WorldData, gridPosition));

        public void ReplaceBuilding(Vector2Int fromGridPosition, BuildingType fromBuildingType, Vector2Int toGridPosition, BuildingType toBuildingType) =>
            ExecuteCommand(new ReplaceBuildingCommand(WorldChanger, WorldData, fromGridPosition, fromBuildingType, toGridPosition, toBuildingType));

        public virtual void OpenChest(Vector2Int chestGridPosition, uint reward) =>
            ExecuteCommand(new RemoveBuildingCommand(WorldChanger, WorldData, chestGridPosition));

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
