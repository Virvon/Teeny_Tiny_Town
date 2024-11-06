using Assets.Sources.Services.StaticDataService.Configs.WorldStore;
using System;

namespace Assets.Sources.Data.World.Currency
{
    [Serializable]
    public class LimitedQuantityStoreItemData : GainStoreItemData
    {
        public uint RemainingCount;

        public LimitedQuantityStoreItemData(GainStoreItemType type, uint startCount)
            : base(type)
        {
            RemainingCount = startCount;
        }

        public override void ChangeBuyingCount(uint count)
        {
            RemainingCount = RemainingCount < count ? 0 : RemainingCount - count;

            base.ChangeBuyingCount(count);
        }
    }
}
