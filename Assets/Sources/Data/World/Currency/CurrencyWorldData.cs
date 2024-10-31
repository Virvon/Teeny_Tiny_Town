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
            List<BuildingType> buildingsStoreList,
            uint[] goals,
            GainStoreItemData[] gainsStoreList)
            : base(id, tiles, nextBuildingTypeForCreation, availableBuildingForCreation, size, goals)
        {
            WorldWallet = new();
            MovesCounter = new();
            WorldStore = new(buildingsStoreList, gainsStoreList);
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
