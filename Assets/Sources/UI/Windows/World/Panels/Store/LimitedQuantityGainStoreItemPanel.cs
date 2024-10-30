using Assets.Sources.Data.WorldDatas.Currency;
using Assets.Sources.Services.StaticDataService.Configs.WorldStore;
using TMPro;
using UnityEngine;

namespace Assets.Sources.UI.Windows.World.Panels.Store
{
    public class LimitedQuantityGainStoreItemPanel : GainStoreItemPanel
    {
        [SerializeField] private TMP_Text _remainCountValue;

        private LimitedQuantityStoreItemData _data;

        protected override GainStoreItemData Data => _data;

        public override void Init(GainStoreItemType type, Sprite icon)
        {
            base.Init(type, icon);

            _remainCountValue.text = _data.RemainingCount.ToString();
        }

        protected override void GetData() =>
            _data = CurrencyWorldData.WorldStore.GetGainData<LimitedQuantityStoreItemData>(Type);
    }
}
