﻿using System.Collections.Generic;
using Assets.Sources.Data.World;
using Assets.Sources.Gameplay.World.WorldInfrastructure.NextBuildingForPlacing;
using Assets.Sources.Gameplay.World.WorldInfrastructure.WorldChangers;
using Assets.Sources.Services.PersistentProgress;
using Assets.Sources.Services.StaticDataService.Configs.Building;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Assets.Sources.Gameplay.GameplayMover.Commands
{
    public class PlaceNewBuildingCommand : MoveCommand
    {
        private readonly Vector2Int _placedBuildingGridPosition;
        private readonly BuildingType _placedBuildingType;
        private readonly IWorldData _worldData;

        private readonly BuildingType _nextBuildingTypeForCreation;
        private readonly uint _nextBuildingForCreationBuildsCount;
        private readonly IReadOnlyList<BuildingType> _availableBuildingsForCreation;

        public PlaceNewBuildingCommand(
            IWorldChanger world,
            Vector2Int placedBuildingGridPosition,
            IWorldData worldData,
            BuildingType placedBuildingType,
            NextBuildingForPlacingCreator nextBuildingForPlacingCreator,
            IPersistentProgressService persistentProgressService)
            : base(world, worldData, nextBuildingForPlacingCreator, persistentProgressService)
        {
            _placedBuildingGridPosition = placedBuildingGridPosition;
            _placedBuildingType = placedBuildingType;
            _worldData = worldData;

            _nextBuildingTypeForCreation = _worldData.NextBuildingTypeForCreation;
            _nextBuildingForCreationBuildsCount = _worldData.NextBuildingForCreationBuildsCount;
            _availableBuildingsForCreation = _worldData.AvailableBuildingsForCreation;
        }

        public override async void Execute()
        {
            PersistentProgressService.Progress.AddBuildingToCollection(_placedBuildingType);
            await WorldChanger.PlaceNewBuilding(_placedBuildingGridPosition, _placedBuildingType);
            base.Execute();
        }

        public override async UniTask Undo()
        {
            await base.Undo();

            _worldData.NextBuildingTypeForCreation = _nextBuildingTypeForCreation;
            _worldData.NextBuildingForCreationBuildsCount = _nextBuildingForCreationBuildsCount;
            _worldData.UpdateAvailableBuildingForCreation(_availableBuildingsForCreation);
        }
    }
}
