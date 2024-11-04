using Assets.Sources.Services.StaticDataService.Configs.WorldStore;

namespace Assets.Sources.Data.World.Currency
{
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
