using Assets.Sources.Services.StaticDataService.Configs.WorldStore;
using System;

namespace Assets.Sources.Data.WorldDatas.Currency
{
    public class GainStoreItemData
    {
        public GainStoreItemType Type;
        public uint BuyingCount;

        public event Action BuyingCountChanged;

        public GainStoreItemData(GainStoreItemType type, uint startCost)
        {
            Type = type;

            BuyingCount = 0;
        }

        public virtual void ChangeBuyingCount(uint count)
        {
            BuyingCount += count;
            BuyingCountChanged?.Invoke();
        }
    }
}
