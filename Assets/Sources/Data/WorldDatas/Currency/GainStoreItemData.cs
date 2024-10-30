using Assets.Sources.Services.StaticDataService.Configs.WorldStore;

namespace Assets.Sources.Data.WorldDatas.Currency
{
    public class GainStoreItemData
    {
        public GainStoreItemType Type;
        public uint Cost;

        public GainStoreItemData(GainStoreItemType type, uint startCost)
        {
            Type = type;
            Cost = startCost;
        }
    }
}
