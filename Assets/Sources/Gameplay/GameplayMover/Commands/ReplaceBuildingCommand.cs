﻿using Assets.Sources.Data.WorldDatas;
using Assets.Sources.Gameplay.World.WorldInfrastructure.NextBuildingForPlacing;
using Assets.Sources.Gameplay.World.WorldInfrastructure.WorldChangers;
using Assets.Sources.Services.PersistentProgress;
using Assets.Sources.Services.StaticDataService.Configs.Building;
using UnityEngine;

namespace Assets.Sources.Gameplay.GameplayMover.Commands
{
    public class ReplaceBuildingCommand : MoveCommand
    {
        private readonly Vector2Int _fromBuildingGridPosition;
        private readonly BuildingType _fromBuildingType;
        private readonly Vector2Int _toBuildingGridPosition;
        private readonly BuildingType _toBuildingType;

        public ReplaceBuildingCommand(
            IWorldChanger world,
            IWorldData worldData,
            Vector2Int fromBuildingGridPosition,
            BuildingType fromBuildingType,
            Vector2Int toBuildingGridPosition,
            BuildingType toBuildingType,
            NextBuildingForPlacingCreator nextBuildingForPlacingCreator,
            IPersistentProgressService persistentProgressService)
            : base(world, worldData, nextBuildingForPlacingCreator, persistentProgressService)
        {
            _fromBuildingGridPosition = fromBuildingGridPosition;
            _fromBuildingType = fromBuildingType;
            _toBuildingGridPosition = toBuildingGridPosition;
            _toBuildingType = toBuildingType;
        }

        public override async void Execute()
        {
            await WorldChanger.ReplaceBuilding(_fromBuildingGridPosition, _fromBuildingType, _toBuildingGridPosition, _toBuildingType);
            base.Execute();
        }
    }
}
