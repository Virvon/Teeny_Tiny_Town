﻿using UnityEngine;
using Assets.Sources.Services.StaticDataService;
using Assets.Sources.Services.StaticDataService.Configs.Building;
using Assets.Sources.Gameplay.World.WorldInfrastructure.Tiles.Buildings;
using Assets.Sources.Services.PersistentProgress;
using Assets.Sources.Data.WorldDatas;
using Assets.Sources.Gameplay.World.WorldInfrastructure.NextBuildingForPlacing;

namespace Assets.Sources.Gameplay.World.WorldInfrastructure.WorldChangers
{
    public class CurrencyWorldChanger : WorldChanger
    {
        private readonly ICurrencyWorldData _currencyWorldData;

        public CurrencyWorldChanger
            (IStaticDataService staticDataService,
            ICurrencyWorldData worldData,
            IPersistentProgressService persistentProgressService,
            NextBuildingForPlacingCreator nextBuildingForPlacingCreator)
            : base(staticDataService, worldData, persistentProgressService, nextBuildingForPlacingCreator)
        {
            _currencyWorldData = worldData;
        }

        public override Building GetBuilding(BuildingType type, Vector2Int gridPosition)
        {
            switch (type)
            {
                case BuildingType.Undefined:
                    return null;
                case BuildingType.Bush:
                    return new Building(type);
                case BuildingType.Tree:
                    return new Building(type);
                case BuildingType.House:
                    return new PayableBuilding(type, StaticDataService, _currencyWorldData.WorldWallet, PersistentProgressService);
                case BuildingType.Stone:
                    return new Building(type);
                case BuildingType.Chest:
                    return new Chest(type, StaticDataService, gridPosition);
                case BuildingType.Lighthouse:
                    return new Lighthouse(type, _currencyWorldData.WorldWallet, PersistentProgressService, this, gridPosition);
                case BuildingType.Crane:
                    return new Crane(type, this, gridPosition);
                default:
                    Debug.LogError("building not founded");
                    return null;
            }
        }
    }
}
