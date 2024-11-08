using System;
using Assets.Sources.Services.StaticDataService.Configs.WorldStore;

namespace Assets.Sources.Data.World.Currency
{
    [Serializable]
    public class GainStoreItemData
    {
        public GainStoreItemType Type;
        public uint BuyingCount;
        public uint RemainingCount;
        public bool IsLimited;
        public uint StartRemainingCount;

        public GainStoreItemData(GainStoreItemType type)
        {
            Type = type;

            BuyingCount = 0;
            IsLimited = false;
        }

        public GainStoreItemData(GainStoreItemType type, uint startCount)
        {
            Type = type;
            RemainingCount = startCount;

            IsLimited = true;
            StartRemainingCount = startCount;
        }

        public event Action BuyingCountChanged;

        public virtual void ChangeBuyingCount(uint count)
        {
            RemainingCount = RemainingCount < count ? 0 : RemainingCount - count;
            BuyingCount += count;
            BuyingCountChanged?.Invoke();
        }

        public void Clear()
        {
            BuyingCount = 0;
            RemainingCount = StartRemainingCount;

            BuyingCountChanged?.Invoke();
        }
    }
}
