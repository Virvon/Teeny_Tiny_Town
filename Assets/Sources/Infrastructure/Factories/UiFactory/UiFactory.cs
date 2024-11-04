using Assets.Sources.Services.AssetManagement;
using Assets.Sources.Services.StaticDataService;
using Assets.Sources.Services.StaticDataService.Configs.AdditionalBonuses;
using Assets.Sources.Services.StaticDataService.Configs.Building;
using Assets.Sources.Services.StaticDataService.Configs.Reward;
using Assets.Sources.Services.StaticDataService.Configs.Windows;
using Assets.Sources.Services.StaticDataService.Configs.WorldStore;
using Assets.Sources.UI;
using Assets.Sources.UI.Windows.MapSelection;
using Assets.Sources.UI.Windows.Sandbox;
using Assets.Sources.UI.Windows.World.Panels;
using Assets.Sources.UI.Windows.World.Panels.AdditionalBonusOffer;
using Assets.Sources.UI.Windows.World.Panels.Reward;
using Assets.Sources.UI.Windows.World.Panels.Store;
using Cysharp.Threading.Tasks;
using System;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Zenject;

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
        private readonly SandboxPanelElement.Factory _sandboxPanelFactory;
        private readonly RotationPanel.Factory _rotationPanelFactory;
        private readonly Blur.BlurFactory _blurFactory;
        private readonly DiContainer _container;
        private readonly PeculiarityIconPanel.Factory _peculiarityIconPanelFactory;
        private readonly LockIcon.Factory _lockIconFactory;

        public UiFactory(
            Window.Factory windowFactory,
            IStaticDataService staticDataService,
            BuildingStoreItem.Factory storeItemFactory,
            RewardPanel.Factory rewardPanelFactory,
            IAssetProvider assetProvider,
            QuestPanel.Factory questPanelFactory,
            RemainingMovesPanel.Factory remainingMovesPanelFactory,
            AdditionalBonusOfferItem.Factory additionalBonusOfferItemFactory,
            GainStoreItemPanel.Factory gainStoreItemPanelFactory,
            SandboxPanelElement.Factory sandboxPanelFactory,
            RotationPanel.Factory rotationPanelFactory,
            Blur.BlurFactory blurFactory,
            DiContainer container,
            PeculiarityIconPanel.Factory peculiarityIconPanelFactory,
            LockIcon.Factory lockIconFactory)
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
            _sandboxPanelFactory = sandboxPanelFactory;
            _rotationPanelFactory = rotationPanelFactory;
            _blurFactory = blurFactory;
            _container = container;
            _peculiarityIconPanelFactory = peculiarityIconPanelFactory;
            _lockIconFactory = lockIconFactory;
        }

        public async UniTask CreateLockIcon(Transform parent) =>
            await _lockIconFactory.Create(UiFactoryAssets.LockIcon, parent);

        public async UniTask<PeculiarityIconPanel> CreatePeculiarityIconPanel(AssetReference iconAssetReference, Transform parent)
        {
            PeculiarityIconPanel peculiarityIconPanel = await _peculiarityIconPanelFactory.Create(UiFactoryAssets.PeculiarityIconPanel, parent);
            Sprite icon = await _assetProvider.Load<Sprite>(iconAssetReference);
            peculiarityIconPanel.Init(icon);

            return peculiarityIconPanel;
        }

        public async UniTask CreateBlur()
        {
            Blur blur = await _blurFactory.Create(UiFactoryAssets.Blur);
            blur.HideImmediately();
            _container.BindInstance(blur).AsSingle();
        }

        public async UniTask CreateRotationPanel(Transform parent) =>
            await _rotationPanelFactory.Create(UiFactoryAssets.RotationPanel, parent);

        public async UniTask<SandboxPanelElement> CreateSandboxPanelElement(Transform parent, AssetReference iconAssetReference)
        {
            SandboxPanelElement sandboxPanelElement = await _sandboxPanelFactory.Create(UiFactoryAssets.SandboxPanelElement, parent);
            Sprite icon = await _assetProvider.Load<Sprite>(iconAssetReference);

            sandboxPanelElement.Init(icon);

            return sandboxPanelElement;
        }

        public async UniTask CreateGainStoreItemPanel(GainStoreItemType type, Transform parent)
        {
            GainStoreItemConfig gainStoreItemConfig = _staticDataService.GetGainStoreItem(type);
            GainStoreItemPanel gainStoreItemPanel = await _gainStoreItemPanelFactory.Create(gainStoreItemConfig.PanelAssetReference, parent);
            Sprite icon = await _assetProvider.Load<Sprite>(gainStoreItemConfig.IconAssetReference);

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
            BuildingStoreItemConfig config = _staticDataService.GetBuildingStoreItem(buildingType);
            BuildingStoreItem storeItem = await _storeItemFactory.Create(_staticDataService.StoreItemsConfig.AssetReference, parent);
            Sprite icon = await _assetProvider.Load<Sprite>(config.IconAssetReference);

            storeItem.Init(buildingType, icon);

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
