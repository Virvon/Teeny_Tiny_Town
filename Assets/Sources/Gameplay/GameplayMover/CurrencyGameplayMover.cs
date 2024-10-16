﻿using Assets.Sources.Data;
using Assets.Sources.Gameplay.GameplayMover.Commands;
using Assets.Sources.Gameplay.World.WorldInfrastructure.WorldChangers;
using Assets.Sources.Services.Input;
using Assets.Sources.Services.PersistentProgress;
using Assets.Sources.Services.StaticDataService.Configs.Building;
using UnityEngine;

namespace Assets.Sources.Gameplay.GameplayMover
{
    public class CurrencyGameplayMover : GameplayMover, ICurrencyGameplayMover
    {
        private readonly ICurrencyWorldData _currencyWorldData;

        public CurrencyGameplayMover(
            IWorldChanger worldChanger,
            IInputService inputService,
            ICurrencyWorldData worldData,
            IPersistentProgressService persistentProgressService)
            : base(worldChanger, inputService, worldData, persistentProgressService)
        {
            _currencyWorldData = worldData;
        }

        public override void OpenChest(Vector2Int chestGridPosition, uint reward) =>
            ExecuteCommand(new OpenChestCommand(WorldChanger, WorldData, reward, chestGridPosition, _currencyWorldData.WorldWallet));

        public void ChangeBuildingForPlacing(BuildingType targetBuildingType, uint buildingPrice) =>
            ExecuteCommand(new ChangeBuildingForPlacingCommand(WorldChanger, WorldData, targetBuildingType, buildingPrice, _currencyWorldData.WorldWallet));
    }
}
