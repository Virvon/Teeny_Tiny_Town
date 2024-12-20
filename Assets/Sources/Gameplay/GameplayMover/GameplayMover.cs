﻿using System;
using Assets.Sources.Data.World;
using Assets.Sources.Gameplay.GameplayMover.Commands;
using Assets.Sources.Gameplay.World.WorldInfrastructure.NextBuildingForPlacing;
using Assets.Sources.Gameplay.World.WorldInfrastructure.WorldChangers;
using Assets.Sources.Services.Input;
using Assets.Sources.Services.PersistentProgress;
using Assets.Sources.Services.StaticDataService.Configs.Building;
using UnityEngine;

namespace Assets.Sources.Gameplay.GameplayMover
{
    public class GameplayMover : IGameplayMover, IDisposable
    {
        protected readonly IWorldChanger WorldChanger;
        protected readonly IWorldData WorldData;
        protected readonly NextBuildingForPlacingCreator NextBuildingForPlacingCreator;

        private readonly IInputService _inputService;
        private readonly IPersistentProgressService _persistentProgressService;

        private bool _isUndoStarted;

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

            _isUndoStarted = false;

            _inputService.UndoButtonPressed += TryUndoCommand;
        }

        protected Command LastCommand { get; private set; }

        public void Dispose() =>
            _inputService.UndoButtonPressed -= TryUndoCommand;

        public void PlaceNewBuilding(Vector2Int gridPosition, BuildingType type) =>
            ExecuteCommand(new PlaceNewBuildingCommand(WorldChanger, gridPosition, WorldData, type, NextBuildingForPlacingCreator, _persistentProgressService));

        public void RemoveBuilding(Vector2Int gridPosition) =>
            ExecuteCommand(new RemoveBuildingCommand(WorldChanger, WorldData, gridPosition, NextBuildingForPlacingCreator, _persistentProgressService));

        public void ReplaceBuilding(Vector2Int fromGridPosition, BuildingType fromBuildingType, Vector2Int toGridPosition, BuildingType toBuildingType)
        {
            ExecuteCommand(new ReplaceBuildingCommand(
                WorldChanger,
                WorldData,
                fromGridPosition,
                fromBuildingType,
                toGridPosition,
                toBuildingType,
                NextBuildingForPlacingCreator,
                _persistentProgressService));
        }

        public virtual void OpenChest(Vector2Int chestGridPosition, uint reward) =>
            ExecuteCommand(new RemoveChestCommand(WorldChanger, WorldData, chestGridPosition, NextBuildingForPlacingCreator, _persistentProgressService));

        public async void TryUndoCommand()
        {
            if (LastCommand == null || _isUndoStarted)
                return;

            _isUndoStarted = true;

            await LastCommand.Undo();
            LastCommand = null;

            _isUndoStarted = false;
        }

        protected void ExecuteCommand(Command command)
        {
            LastCommand = command;
            command.Execute();
        }
    }
}
