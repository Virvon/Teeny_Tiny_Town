using System;
using Assets.Sources.Data.World.Currency;
using Assets.Sources.Services.StaticDataService;
using Assets.Sources.Services.StaticDataService.Configs;
using Assets.Sources.Services.StaticDataService.Configs.Building;
using Assets.Sources.Utils;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;
using Zenject;

namespace Assets.Sources.UI.Windows.World.Panels.Store
{
    public class BuildingStoreItem : MonoBehaviour
    {
        [SerializeField] private TMP_Text _costValue;
        [SerializeField] private Image _icon;
        [SerializeField] private Button _buyButton;

        private ICurrencyWorldData _currencyWorldData;
        private IStaticDataService _staticDataService;
        private AnimationsConfig _animationsConfig;

        private BuildingType _buildingType;
        private BuildingStoreItemData _data;

        public event Action<BuildingType, uint> Buyed;

        private uint Cost => _staticDataService.GetBuildingStoreItem(_buildingType).GetCost(_data.BuyingCount + 1);

        [Inject]
        private void Construct(ICurrencyWorldData currencyWorldData, IStaticDataService staticDataService)
        {
            _currencyWorldData = currencyWorldData;
            _staticDataService = staticDataService;
            _animationsConfig = staticDataService.AnimationsConfig;

            _buyButton.onClick.AddListener(OnBuyButtonClicked);
            _currencyWorldData.WorldWallet.ValueChanged += ChangeCostValue;
        }

        private void OnDestroy()
        {
            _buyButton.onClick.RemoveListener(OnBuyButtonClicked);
            _data.BuyingCountChanged -= OnBuyingCountChanged;
        }

        public void Init(BuildingType buildingType, Sprite icon)
        {
            _buildingType = buildingType;
            _data = _currencyWorldData.WorldStore.GetBuildingData(_buildingType);

            ChangeCostValue(_currencyWorldData.WorldWallet.Value);

            _data.BuyingCountChanged += OnBuyingCountChanged;

            _icon.sprite = icon;
            _icon.SetNativeSize();
        }

        private void OnBuyingCountChanged()
        {
            ChangeCostValue(_currencyWorldData.WorldWallet.Value);
        }

        private void OnBuyButtonClicked() =>
            Buyed?.Invoke(_buildingType, Cost);

        private void ChangeCostValue(uint worldWalletValue)
        {
            _costValue.text = DigitUtils.CutDigit(Cost);
            _costValue.color = worldWalletValue >= Cost ? _animationsConfig.PurchasePermittingColor : _animationsConfig.ProhibitingPurchaseColor;
        }

        public class Factory : PlaceholderFactory<AssetReferenceGameObject, Transform, UniTask<BuildingStoreItem>>
        {
        }
    }
}
