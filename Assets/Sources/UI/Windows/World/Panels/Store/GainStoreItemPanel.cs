using Assets.Sources.Data.WorldDatas.Currency;
using Assets.Sources.Services.StaticDataService;
using Assets.Sources.Services.StaticDataService.Configs.WorldStore;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;
using Zenject;

namespace Assets.Sources.UI.Windows.World.Panels.Store
{
    public abstract class GainStoreItemPanel : MonoBehaviour
    {
        [SerializeField] private TMP_Text _costValue;
        [SerializeField] private Image _icon;
        [SerializeField] private TMP_Text _name;
        [SerializeField] private Button _buyButton;
        [SerializeField] private Color _purchasePermittingColor;
        [SerializeField] private Color _prohibitingPurchaseColor;

        private IStaticDataService _staticDataService;

        [Inject]
        private void Construct(ICurrencyWorldData currencyWorldData, IStaticDataService staticDataService)
        {
            CurrencyWorldData = currencyWorldData;
            _staticDataService = staticDataService;

            CurrencyWorldData.WorldWallet.ValueChanged += ChangeCostValueColor;
        }

        protected ICurrencyWorldData CurrencyWorldData { get; private set; }
        protected GainStoreItemType Type { get; private set; }
        protected abstract GainStoreItemData Data { get; }

        private void OnDisable() =>
            CurrencyWorldData.WorldWallet.ValueChanged -= ChangeCostValueColor;

        public virtual void Init(GainStoreItemType type, Sprite icon)
        {
            Type = type;

            GetData();

            _costValue.text = Data.Cost.ToString();
            _icon.sprite = icon;
            _name.text = _staticDataService.GetGainStoreItem(type).Name;

            ChangeCostValueColor(CurrencyWorldData.WorldWallet.Value);
        }

        protected abstract void GetData();

        private void ChangeCostValueColor(uint WorldWalletValue) =>
            _costValue.color = WorldWalletValue >= Data.Cost ? _purchasePermittingColor : _prohibitingPurchaseColor;

        public class Factory : PlaceholderFactory<AssetReferenceGameObject, Transform, UniTask<GainStoreItemPanel>>
        {
        }
    }
}
