using Assets.Sources.Data.WorldDatas.Currency;
using Assets.Sources.Services.StaticDataService.Configs.WorldStore;
using UnityEngine;

namespace Assets.Sources.UI.Windows.World.Panels.Store
{
    public class UnlimitedQuantityGainStoreItemPanel : GainStoreItemPanel
    {
        private GainStoreItemData _data;

        protected override GainStoreItemData Data => _data;

        protected override void GetData() =>
            _data = CurrencyWorldData.WorldStore.GetGainData<GainStoreItemData>(Type);
    }
}
