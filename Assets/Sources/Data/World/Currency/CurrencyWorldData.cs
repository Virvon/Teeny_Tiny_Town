using Assets.Sources.Services.StaticDataService.Configs.Building;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Sources.Data.World.Currency
{
    [Serializable]
    public class CurrencyWorldData : WorldData, ICurrencyWorldData
    {
        public WorldWallet WorldWallet;
        public WorldMovesCounterData MovesCounter;
        public WorldStore WorldStore;

        public CurrencyWorldData(
            string id,
            TileData[] tiles,
            BuildingType nextBuildingTypeForCreation,
            List<BuildingType> availableBuildingForCreation,
            Vector2Int size,
            BuildingType[] startBuildingsStoreList,
            uint[] goals,
            GainStoreItemData[] gainsStoreList,
            bool isUnlocked)
            : base(id, tiles, nextBuildingTypeForCreation, availableBuildingForCreation, size, goals, isUnlocked)
        {
            WorldWallet = new();
            MovesCounter = new();
            WorldStore = new(startBuildingsStoreList, gainsStoreList);
        }

        WorldWallet ICurrencyWorldData.WorldWallet => WorldWallet;
        WorldMovesCounterData ICurrencyWorldData.MovesCounter => MovesCounter;
        WorldStore ICurrencyWorldData.WorldStore => WorldStore;

        protected override void AddNextBuildingTypeForCreation(BuildingType type)
        {
            base.AddNextBuildingTypeForCreation(type);
            WorldStore.TryAddBuilding(type);
        }
    }
}
