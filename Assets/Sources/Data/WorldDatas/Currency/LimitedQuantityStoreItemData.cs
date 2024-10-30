using Assets.Sources.Services.StaticDataService.Configs.WorldStore;

namespace Assets.Sources.Data.WorldDatas.Currency
{
    public class LimitedQuantityStoreItemData : GainStoreItemData
    {
        public uint RemainingCount;

        public LimitedQuantityStoreItemData(GainStoreItemType type, uint startCost, uint startCount)
            : base(type, startCost)
        {
            RemainingCount = startCount;
        }
    }
}
