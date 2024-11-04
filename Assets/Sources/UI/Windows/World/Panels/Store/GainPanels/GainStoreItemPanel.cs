using Assets.Sources.Data.World.Currency;
using Assets.Sources.Services.StaticDataService;
using Assets.Sources.Services.StaticDataService.Configs;
using Assets.Sources.Services.StaticDataService.Configs.WorldStore;
using Assets.Sources.Utils;
using Cysharp.Threading.Tasks;
using System;
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

        private IStaticDataService _staticDataService;
        private AnimationsConfig _animationsConfig;

        [Inject]
        private void Construct(ICurrencyWorldData currencyWorldData, IStaticDataService staticDataService, GainBuyer gainBuyer)
        {
            CurrencyWorldData = currencyWorldData;
            _staticDataService = staticDataService;
            GainBuyer = gainBuyer;
            _animationsConfig = _staticDataService.AnimationsConfig;

            CurrencyWorldData.WorldWallet.ValueChanged += ChangeCostValue;
            _buyButton.onClick.AddListener(OnBuyButtonClicked);
        }

        protected ICurrencyWorldData CurrencyWorldData { get; private set; }
        protected GainStoreItemType Type { get; private set; }
        protected abstract GainStoreItemData Data { get; }
        protected GainBuyer GainBuyer { get; private set; }
        protected uint Cost => _staticDataService.GetGainStoreItem(Type).GetCost(Data.BuyingCount + 1);
        protected Button BuyButton => _buyButton;

        private void OnDestroy()
        {
            CurrencyWorldData.WorldWallet.ValueChanged -= ChangeCostValue;
            _buyButton.onClick.RemoveListener(OnBuyButtonClicked);
            Data.BuyingCountChanged -= OnBuyingCountChanged;
        }

        public virtual void Init(GainStoreItemType type, Sprite icon)
        {
            Type = type;

            GetData();

            _icon.sprite = icon;
            _name.text = _staticDataService.GetGainStoreItem(type).Name;

            ChangeCostValue(CurrencyWorldData.WorldWallet.Value);

            Data.BuyingCountChanged += OnBuyingCountChanged;
        }

        protected abstract void GetData();

        protected virtual void ChangeCostValue(uint worldWalletValue)
        {
            _costValue.text = DigitUtils.CutDigit(Cost);
            _costValue.color = worldWalletValue >= Cost ? _animationsConfig.PurchasePermittingColor : _animationsConfig.ProhibitingPurchaseColor;
        }
            
        protected abstract void OnBuyButtonClicked();

        private void OnBuyingCountChanged() =>
            ChangeCostValue(CurrencyWorldData.WorldWallet.Value);

        public class Factory : PlaceholderFactory<AssetReferenceGameObject, Transform, UniTask<GainStoreItemPanel>>
        {
        }
    }
}
