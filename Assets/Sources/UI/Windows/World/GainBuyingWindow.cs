using Assets.Sources.Data.World.Currency;
using Assets.Sources.Gameplay.World.StateMachine;
using Assets.Sources.Gameplay.World.StateMachine.States;
using Assets.Sources.Services.AssetManagement;
using Assets.Sources.Services.StaticDataService;
using Assets.Sources.Services.StaticDataService.Configs;
using Assets.Sources.Services.StaticDataService.Configs.WorldStore;
using Assets.Sources.UI.Windows.World.Panels.Store;
using Cysharp.Threading.Tasks;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Assets.Sources.UI.Windows.World
{
    public class GainBuyingWindow : BluredBackgroundWindow
    {
        private const uint MinPurchasedQuantity = 1;
        private const uint MaxPurchasedQuantity = 10;

        [SerializeField] private Image _gainIcon;
        [SerializeField] private Button _increaseQuantityButton;
        [SerializeField] private Button _decreaseQuantityButton;
        [SerializeField] private TMP_Text _costValue;
        [SerializeField] private TMP_Text _countValue;
        [SerializeField] private Button _buyButton;
        [SerializeField] private Button _hideButton;

        private UnlimitedQuantityGainBuyer _gainBuyer;
        private IStaticDataService _staticDataService;
        private IAssetProvider _assetProvider;
        private AnimationsConfig _animationConfig;
        private ICurrencyWorldData _currencyWorldData;
        private WorldStateMachine _worldStateMachine;

        private uint _purchasedQuantity;
        private GainStoreItemConfig _gainStoreItemConfig;
        private uint _cost;

        [Inject]
        private void Construct(
            UnlimitedQuantityGainBuyer gainBuyer,
            IStaticDataService staticDataService,
            IAssetProvider assetProvider,
            ICurrencyWorldData currencyWorldData,
            WorldStateMachine worldStateMachine)
        {
            _gainBuyer = gainBuyer;
            _staticDataService = staticDataService;
            _assetProvider = assetProvider;
            _animationConfig = _staticDataService.AnimationsConfig;
            _currencyWorldData = currencyWorldData;
            _worldStateMachine = worldStateMachine;

            _increaseQuantityButton.onClick.AddListener(OnIncreaseQuantityButtonClicked);
            _decreaseQuantityButton.onClick.AddListener(OnDecreaseQuantityButtonClicked);
            _buyButton.onClick.AddListener(OnBuyButtonClicked);
            _hideButton.onClick.AddListener(EnterWorldStatrState);
        }

        private void OnDestroy()
        {
            _increaseQuantityButton.onClick.RemoveListener(OnIncreaseQuantityButtonClicked);
            _decreaseQuantityButton.onClick.RemoveListener(OnDecreaseQuantityButtonClicked);
            _buyButton.onClick.RemoveListener(OnBuyButtonClicked);
            _hideButton.onClick.RemoveListener(EnterWorldStatrState);
        }

        public override async void Open()
        {
            _gainIcon.sprite = await _assetProvider.Load<Sprite>(_staticDataService.GetGainStoreItem(_gainBuyer.GainStoreItemType).IconAssetReferecne);
            _purchasedQuantity = MinPurchasedQuantity;
            _gainStoreItemConfig = _staticDataService.GetGainStoreItem(_gainBuyer.GainStoreItemType);

            ChangeView();

            base.Open();
        }

        private void OnDecreaseQuantityButtonClicked()
        {
            _purchasedQuantity--;
            ChangeView();
        }

        private void OnIncreaseQuantityButtonClicked()
        {
            _purchasedQuantity++;
            ChangeView();
        }

        private void ChangeView()
        {
            _purchasedQuantity = (uint)Mathf.Clamp(_purchasedQuantity, MinPurchasedQuantity, MaxPurchasedQuantity);

            uint buyingCount = _currencyWorldData.WorldStore.GetGainData<GainStoreItemData>(_gainBuyer.GainStoreItemType).BuyingCount;

            _cost = _gainStoreItemConfig.GetCostsSum(buyingCount, buyingCount + _purchasedQuantity);

            _countValue.text = _purchasedQuantity.ToString();
            _costValue.text = _cost.ToString();
            _costValue.color = _currencyWorldData.WorldWallet.Value >= _cost ? _animationConfig.PurchasePermittingColor : _animationConfig.ProhibitingPurchaseColor;
        }

        private void OnBuyButtonClicked()
        {
            if (_currencyWorldData.WorldWallet.TryGet(_cost))
            {
                _gainBuyer.ChangeGainItemsCoutn(_purchasedQuantity);
                EnterWorldStatrState();
            }
        }

        private void EnterWorldStatrState() =>
            _worldStateMachine.Enter<WorldStartState>().Forget();
    }
}
