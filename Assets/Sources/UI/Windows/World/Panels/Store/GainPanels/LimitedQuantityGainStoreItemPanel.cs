using Assets.Sources.Data.World.Currency;
using Assets.Sources.Gameplay.World.StateMachine;
using Assets.Sources.Gameplay.World.StateMachine.States;
using Assets.Sources.Services.StaticDataService.Configs.WorldStore;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;
using Zenject;

namespace Assets.Sources.UI.Windows.World.Panels.Store
{
    public class LimitedQuantityGainStoreItemPanel : GainStoreItemPanel
    {
        [SerializeField] private TMP_Text _remainCountValue;
        [SerializeField] private CanvasGroup _buyButtonCanvasGroup;
        [SerializeField] private CanvasGroup _endedItemsInfogCanvasGroup;

        private WorldStateMachine _worldStateMachine;

        private GainStoreItemData _data;

        [Inject]
        private void Construct(WorldStateMachine worldStateMachine) =>
            _worldStateMachine = worldStateMachine;

        protected override GainStoreItemData Data => _data;

        public override void Init(GainStoreItemType type, Sprite icon)
        {
            base.Init(type, icon);

            _remainCountValue.text = _data.RemainingCount.ToString();
        }

        protected override void GetData() =>
            _data = CurrencyWorldData.WorldStore.GetGainData(Type);

        protected override void OnBuyButtonClicked()
        {
            if (CurrencyWorldData.WorldWallet.TryGet(Cost) && _data.RemainingCount > 0)
            {
                _data.ChangeBuyingCount(1);
                GainBuyer.Buy(Type);
                _worldStateMachine.Enter<WorldStartState>().Forget();
            }
        }

        protected override void ChangeCostValue(uint worldWalletValue)
        {
            _remainCountValue.text = _data.RemainingCount.ToString();

            if (_data.RemainingCount != 0)
            {
                base.ChangeCostValue(worldWalletValue);
                SetActiveCanvasGroup(_buyButtonCanvasGroup, true);
                SetActiveCanvasGroup(_endedItemsInfogCanvasGroup, false);
                BuyButton.interactable = true;
            }
            else
            {
                SetActiveCanvasGroup(_buyButtonCanvasGroup, false);
                SetActiveCanvasGroup(_endedItemsInfogCanvasGroup, true);
                BuyButton.interactable = false;
            }
        }
        private void SetActiveCanvasGroup(CanvasGroup canvasGroup, bool isActive)
        {
            canvasGroup.alpha = isActive ? 1 : 0;
            canvasGroup.blocksRaycasts = isActive;
            canvasGroup.interactable = isActive;
        }
    }
}
