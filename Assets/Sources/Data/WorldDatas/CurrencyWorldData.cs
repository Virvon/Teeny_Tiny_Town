using Assets.Sources.Services.StaticDataService.Configs.Building;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Sources.Data.WorldDatas
{
    [Serializable]
    public class CurrencyWorldData : WorldData, ICurrencyWorldData
    {
        public WorldWallet WorldWallet;
        public List<BuildingType> StoreList;

        public CurrencyWorldData(
            string id,
            TileData[] tiles,
            BuildingType nextBuildingTypeForCreation,
            List<BuildingType> availableBuildingForCreation,
            Vector2Int size,
            List<BuildingType> storeList,
            uint[] goals)
            : base(id, tiles, nextBuildingTypeForCreation, availableBuildingForCreation, size, goals)
        {
            StoreList = storeList;

            WorldWallet = new();
        }

        public event Action<BuildingType> StoreListUpdated;

        WorldWallet ICurrencyWorldData.WorldWallet => WorldWallet;
        IReadOnlyList<BuildingType> ICurrencyWorldData.StoreList => StoreList;

        protected override void AddNextBuildingTypeForCreation(BuildingType type)
        {
            base.AddNextBuildingTypeForCreation(type);

            if (StoreList.Contains(type) == false)
            {
                StoreList.Add(type);
                StoreListUpdated?.Invoke(type);
            }
        }
    }
}
