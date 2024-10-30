using Assets.Sources.Data.WorldDatas.Currency;

namespace Assets.Sources.UI.Windows.World.Panels.Store
{
    public class UnlimitedQuantityGainStoreItemPanel : GainStoreItemPanel
    {
        private GainStoreItemData _data;

        protected override GainStoreItemData Data => _data;

        protected override void GetData() =>
            _data = CurrencyWorldData.WorldStore.GetGainData<GainStoreItemData>(Type);

        protected override void OnBuyButtonClicked()
        {
            if (CurrencyWorldData.WorldWallet.TryGet(Cost))
                GainBuyer.Buy(Type);
        }
    }
}
