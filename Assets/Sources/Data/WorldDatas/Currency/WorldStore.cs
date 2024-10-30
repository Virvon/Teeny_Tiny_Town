using Assets.Sources.Services.StaticDataService.Configs.Building;
using Assets.Sources.Services.StaticDataService.Configs.WorldStore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Assets.Sources.Data.WorldDatas.Currency
{
    [Serializable]
    public class WorldStore
    {
        public List<BuildingType> BuildingsStoreList;
        public GainStoreItemData[] GainStoreList;

        public WorldStore(List<BuildingType> buildingsStoreList, GainStoreItemData[] gainsStoreList)
        {
            BuildingsStoreList = buildingsStoreList;
            GainStoreList = gainsStoreList;
        }

        public event Action<BuildingType> BuildingsStoreListUpdated;

        public void TryAddBuilding(BuildingType type)
        {
            if (BuildingsStoreList.Contains(type) == false)
            {
                BuildingsStoreList.Add(type);
                BuildingsStoreListUpdated?.Invoke(type);
            }
        }

        public TData GetGainData<TData>(GainStoreItemType type)
            where TData : GainStoreItemData =>
            GainStoreList.First(data => data.Type == type) as TData;
    }
}
