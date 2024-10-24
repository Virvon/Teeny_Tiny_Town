﻿using Assets.Sources.Gameplay.Store;
using Assets.Sources.Services.AssetManagement;
using Assets.Sources.Services.StaticDataService;
using Assets.Sources.Services.StaticDataService.Configs.Building;
using Assets.Sources.Services.StaticDataService.Configs.Reward;
using Assets.Sources.Services.StaticDataService.Configs.Windows;
using Assets.Sources.UI;
using Assets.Sources.UI.Windows.World.Panels.Reward;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace Assets.Sources.Infrastructure.Factories.UiFactory
{
    public class UiFactory : IUiFactory
    {
        private readonly Window.Factory _windowFactory;
        private readonly IStaticDataService _staticDataService;
        private readonly StoreItem.Factory _storeItemFactory;
        private readonly RewardPanel.Factory _rewardPanelFactory;
        private readonly IAssetProvider _assetProvider;

        public UiFactory(Window.Factory windowFactory, IStaticDataService staticDataService, StoreItem.Factory storeItemFactory, RewardPanel.Factory rewardPanelFactory, IAssetProvider assetProvider)
        {
            _windowFactory = windowFactory;
            _staticDataService = staticDataService;
            _storeItemFactory = storeItemFactory;
            _rewardPanelFactory = rewardPanelFactory;
            _assetProvider = assetProvider;
        }

        public async UniTask<RewardPanel> CreateRewardPanel(RewardType type, Transform parent)
        {
            RewardConfig rewardConfig = _staticDataService.GetReward(type);
            RewardPanel rewardPanel = await _rewardPanelFactory.Create(rewardConfig.AssetReference, parent);
            Sprite icon = await _assetProvider.Load<Sprite>(rewardConfig.IconAssetReference);

            rewardPanel.Init(icon);

            return rewardPanel;
        }

        public async UniTask<StoreItem> CreateStoreItem(BuildingType buildingType, Transform parent)
        {
            GameplayStoreItemConfig config = _staticDataService.GetStoreItem(buildingType);
            StoreItem storeItem = await _storeItemFactory.Create(_staticDataService.StoreItemsConfig.AssetReference, parent);

            storeItem.Init(config.Price, buildingType);

            return storeItem;
        }

        public async UniTask<Window> CreateWindow(WindowType type)
        {
            Window window = await _windowFactory.Create(_staticDataService.GetWindow(type).AssetReference);

            window.Hide();

            return window;
        }
    }
}
