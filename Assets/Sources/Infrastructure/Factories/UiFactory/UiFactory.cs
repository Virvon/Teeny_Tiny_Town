using Assets.Sources.Services.AssetManagement;
using Assets.Sources.Services.StaticDataService;
using Assets.Sources.Services.StaticDataService.Configs.AdditionalBonuses;
using Assets.Sources.Services.StaticDataService.Configs.Building;
using Assets.Sources.Services.StaticDataService.Configs.Reward;
using Assets.Sources.Services.StaticDataService.Configs.Windows;
using Assets.Sources.Services.StaticDataService.Configs.WorldStore;
using Assets.Sources.UI;
using Assets.Sources.UI.Windows.World.Panels;
using Assets.Sources.UI.Windows.World.Panels.AdditionalBonusOffer;
using Assets.Sources.UI.Windows.World.Panels.Reward;
using Assets.Sources.UI.Windows.World.Panels.Store;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Assets.Sources.Infrastructure.Factories.UiFactory
{
    public class UiFactory : IUiFactory
    {
        private readonly Window.Factory _windowFactory;
        private readonly IStaticDataService _staticDataService;
        private readonly BuildingStoreItem.Factory _storeItemFactory;
        private readonly RewardPanel.Factory _rewardPanelFactory;
        private readonly IAssetProvider _assetProvider;
        private readonly QuestPanel.Factory _questPanelFactory;
        private readonly RemainingMovesPanel.Factory _remainingMovesPanelFactory;
        private readonly AdditionalBonusOfferItem.Factory _additionalBonusOfferItemFactory;
        private readonly GainStoreItemPanel.Factory _gainStoreItemPanelFactory;

        public UiFactory(
            Window.Factory windowFactory,
            IStaticDataService staticDataService,
            BuildingStoreItem.Factory storeItemFactory,
            RewardPanel.Factory rewardPanelFactory,
            IAssetProvider assetProvider,
            QuestPanel.Factory questPanelFactory,
            RemainingMovesPanel.Factory remainingMovesPanelFactory,
            AdditionalBonusOfferItem.Factory additionalBonusOfferItemFactory,
            GainStoreItemPanel.Factory gainStoreItemPanelFactory)
        {
            _windowFactory = windowFactory;
            _staticDataService = staticDataService;
            _storeItemFactory = storeItemFactory;
            _rewardPanelFactory = rewardPanelFactory;
            _assetProvider = assetProvider;
            _questPanelFactory = questPanelFactory;
            _remainingMovesPanelFactory = remainingMovesPanelFactory;
            _additionalBonusOfferItemFactory = additionalBonusOfferItemFactory;
            _gainStoreItemPanelFactory = gainStoreItemPanelFactory;
        }

        public async UniTask CreateGainStoreItemPanel(GainStoreItemType type, Transform parent)
        {
            GainStoreItemConfig gainStoreItemConfig = _staticDataService.GetGainStoreItem(type);
            GainStoreItemPanel gainStoreItemPanel = await _gainStoreItemPanelFactory.Create(gainStoreItemConfig.PanelAssetReference, parent);
            Sprite icon = await _assetProvider.Load<Sprite>(gainStoreItemConfig.IconAssetReferecne);

            gainStoreItemPanel.Init(type, icon);
        }

        public async UniTask CreateAdditionBonusOfferItem(AdditionalBonusType type, Transform parent)
        {
            AdditionalBonusConfig additionalBonusConfig = _staticDataService.GetAdditionalBonus(type);
            AdditionalBonusOfferItem additionalBonusOfferItem = await _additionalBonusOfferItemFactory.Create(additionalBonusConfig.PanelAssetReference, parent);
            Sprite icon = await _assetProvider.Load<Sprite>(additionalBonusConfig.IconAssetReference);

            additionalBonusOfferItem.Init(type, icon);
        }

        public async UniTask CreateRemainingMovesPanel(Transform parent) =>
            await _remainingMovesPanelFactory.Create(UiFactoryAssets.RemainingMovesPanel, parent);

        public async UniTask<QuestPanel> CreateQuestPanel(string id, Transform parent)
        {
            QuestPanel questPanel = await _questPanelFactory.Create(_staticDataService.QuestsConfig.QuestPanelAssetReference, parent);
            questPanel.Init(id);

            return questPanel;
        }

        public async UniTask<RewardPanel> CreateRewardPanel(RewardType type, Transform parent)
        {
            RewardConfig rewardConfig = _staticDataService.GetReward(type);
            RewardPanel rewardPanel = await _rewardPanelFactory.Create(rewardConfig.AssetReference, parent);
            Sprite icon = await _assetProvider.Load<Sprite>(rewardConfig.IconAssetReference);

            rewardPanel.Init(icon, type);

            return rewardPanel;
        }

        public async UniTask<BuildingStoreItem> CreateStoreItem(BuildingType buildingType, Transform parent)
        {
            GameplayStoreItemConfig config = _staticDataService.GetWorldStoreItem(buildingType);
            BuildingStoreItem storeItem = await _storeItemFactory.Create(_staticDataService.StoreItemsConfig.AssetReference, parent);

            storeItem.Init(config.Price, buildingType);

            return storeItem;
        }

        public async UniTask<Window> CreateWindow(WindowType type)
        {
            Window window = await _windowFactory.Create(_staticDataService.GetWindow(type).AssetReference);

            window.HideImmediately();

            return window;
        }
    }
}
