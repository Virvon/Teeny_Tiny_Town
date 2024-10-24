using Assets.Sources.Gameplay.Store;
using Assets.Sources.Services.StaticDataService;
using Assets.Sources.Services.StaticDataService.Configs.Building;
using Assets.Sources.Services.StaticDataService.Configs.Windows;
using Assets.Sources.UI;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace Assets.Sources.Infrastructure.Factories.UiFactory
{
    public class UiFactory : IUiFactory
    {
        private readonly DiContainer _container;
        private readonly Window.Factory _windowFactory;
        private readonly IStaticDataService _staticDataService;
        private readonly StoreItem.Factory _storeItemFactory;

        public UiFactory(DiContainer container, Window.Factory windowFactory, IStaticDataService staticDataService, StoreItem.Factory storeItemFactory)
        {
            _container = container;
            _windowFactory = windowFactory;
            _staticDataService = staticDataService;
            _storeItemFactory = storeItemFactory;
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
