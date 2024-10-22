using Assets.Sources.Data.WorldDatas;
using Assets.Sources.Gameplay.GameplayMover.Commands;
using Assets.Sources.Gameplay.World.WorldInfrastructure.NextBuildingForPlacing;
using Assets.Sources.Gameplay.World.WorldInfrastructure.WorldChangers;
using Assets.Sources.Services.Input;
using Assets.Sources.Services.PersistentProgress;
using Assets.Sources.Services.StaticDataService.Configs.Building;
using UnityEngine;

namespace Assets.Sources.Gameplay.GameplayMover
{
    public class GameplayMover : IGameplayMover
    {
        protected readonly IWorldChanger WorldChanger;
        protected readonly IWorldData WorldData;
        protected readonly NextBuildingForPlacingCreator NextBuildingForPlacingCreator;

        private readonly IInputService _inputService;
        private readonly IPersistentProgressService _persistentProgressService;

        public GameplayMover(
            IWorldChanger worldChanger,
            IInputService inputService,
            IWorldData worldData,
            IPersistentProgressService persistentProgressService,
            NextBuildingForPlacingCreator nextBuildingForPlacingCreator)
        {
            WorldChanger = worldChanger;
            _inputService = inputService;
            WorldData = worldData;
            _persistentProgressService = persistentProgressService;
            NextBuildingForPlacingCreator = nextBuildingForPlacingCreator;

            _inputService.UndoButtonPressed += TryUndoCommand;
        }

        ~GameplayMover()
        {
            _inputService.UndoButtonPressed -= TryUndoCommand;
        }

        protected Command LastCommand { get; private set; }

        public void PlaceNewBuilding(Vector2Int gridPosition, BuildingType type) =>
            ExecuteCommand(new PlaceNewBuildingCommand(WorldChanger, gridPosition, WorldData, type, NextBuildingForPlacingCreator));

        public void RemoveBuilding(Vector2Int gridPosition) =>
            ExecuteCommand(new RemoveBuildingCommand(WorldChanger, WorldData, gridPosition, NextBuildingForPlacingCreator));

        public void ReplaceBuilding(Vector2Int fromGridPosition, BuildingType fromBuildingType, Vector2Int toGridPosition, BuildingType toBuildingType) =>
            ExecuteCommand(new ReplaceBuildingCommand(WorldChanger, WorldData, fromGridPosition, fromBuildingType, toGridPosition, toBuildingType, NextBuildingForPlacingCreator));

        public virtual void OpenChest(Vector2Int chestGridPosition, uint reward) =>
            ExecuteCommand(new RemoveBuildingCommand(WorldChanger, WorldData, chestGridPosition, NextBuildingForPlacingCreator));

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
