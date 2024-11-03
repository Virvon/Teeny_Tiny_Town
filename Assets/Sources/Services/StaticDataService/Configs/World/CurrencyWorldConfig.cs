using Assets.Sources.Data.World;
using Assets.Sources.Data.World.Currency;
using Assets.Sources.Services.StaticDataService.Configs.Building;
using Assets.Sources.Services.StaticDataService.Configs.WorldStore;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Sources.Services.StaticDataService.Configs.World
{
    [CreateAssetMenu(fileName = "CurrencyWorldConfig", menuName = "StaticData/WorldConfig/Create new currency world config", order = 51)]
    public class CurrencyWorldConfig : WorldConfig
    {
        public List<BuildingType> StartStoreList;
        public GainStoreItemType[] AvailableGainStoreItems;

        public override WorldData GetWorldData(uint[] goals, IStaticDataService staticDataService) =>
            new CurrencyWorldData(Id, TilesDatas, NextBuildingTypeForCreation, StartingAvailableBuildingTypes.ToList(), Size, StartStoreList, goals, GetGainStoreItemsList(staticDataService), IsUnlockedOnStart);

        protected GainStoreItemData[] GetGainStoreItemsList(IStaticDataService staticDataService)
        {
            GainStoreItemData[] datas = new GainStoreItemData[AvailableGainStoreItems.Length];

            for(int i = 0; i < AvailableGainStoreItems.Length; i++)
                datas[i] = staticDataService.GetGainStoreItem(AvailableGainStoreItems[i]).GetData();

            return datas;
        }
    }
}
