﻿using System;
using System.Collections.Generic;
using System.Linq;
using Assets.Sources.Services.StaticDataService.Configs.Building;
using Assets.Sources.Services.StaticDataService.Configs.WorldStore;

namespace Assets.Sources.Data.World.Currency
{
    [Serializable]
    public class WorldStore
    {
        public GainStoreItemData[] GainStoreList;
        public List<BuildingStoreItemData> BuildingsStoreList;

        public WorldStore(BuildingType[] startBuildingStoreList, GainStoreItemData[] gainsStoreList)
        {
            GainStoreList = gainsStoreList;

            BuildingsStoreList = new ();

            foreach (BuildingType buildingType in startBuildingStoreList)
                TryAddBuilding(buildingType);
        }

        public event Action<BuildingType> BuildingsStoreListUpdated;
        public event Action Cleared;

        public void TryAddBuilding(BuildingType type)
        {
            if (BuildingsStoreList.Any(data => data.Type == type) == false)
            {
                BuildingStoreItemData data = new BuildingStoreItemData(type);
                BuildingsStoreList.Add(data);
                BuildingsStoreListUpdated?.Invoke(type);
            }
        }

        public GainStoreItemData GetGainData(GainStoreItemType type) =>
            GainStoreList.First(data => data.Type == type);

        public BuildingStoreItemData GetBuildingData(BuildingType type) =>
            BuildingsStoreList.First(data => data.Type == type);

        public void Clear(BuildingType[] startBuildingsStoreList)
        {
            BuildingsStoreList.Clear();

            foreach (BuildingType buildingType in startBuildingsStoreList)
                TryAddBuilding(buildingType);

            foreach (GainStoreItemData gainStoreItemData in GainStoreList)
                gainStoreItemData.Clear();

            Cleared?.Invoke();
        }
    }
}
