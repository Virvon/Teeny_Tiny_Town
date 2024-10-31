using Assets.Sources.Data.World.Currency;
using Assets.Sources.Services.StaticDataService.Configs.WorldStore;
using UnityEngine;

namespace Assets.Sources.UI.Windows.World.Panels.Store
{
    public class UnlimitedQuantityGainBuyer
    {
        private readonly ICurrencyWorldData _currencyWorldData;

        public UnlimitedQuantityGainBuyer(ICurrencyWorldData currencyWorldData) =>
            _currencyWorldData = currencyWorldData;

        public GainStoreItemType GainStoreItemType { get; private set; }

        public void SetBuyingGainType(GainStoreItemType type)
        {
            GainStoreItemType = type;
        }

        public void ChangeGainItemsCoutn(uint count)
        {
            switch (GainStoreItemType)
            {
                case GainStoreItemType.ReplaceItems:
                    _currencyWorldData.ReplaceItems.AddItems(count);
                    break;
                case GainStoreItemType.Bulldozer:
                    _currencyWorldData.BulldozerItems.AddItems(count);
                    break;
                default:
                    Debug.LogError($"{typeof(GainStoreItemType)} can not be changed");
                    break;
            }

            _currencyWorldData.WorldStore.GetGainData<GainStoreItemData>(GainStoreItemType).ChangeBuyingCount(count);
        }
    }
}
