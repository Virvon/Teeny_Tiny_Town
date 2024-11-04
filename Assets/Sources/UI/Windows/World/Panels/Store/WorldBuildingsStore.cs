using Assets.Sources.Data.World.Currency;
using Assets.Sources.Gameplay.GameplayMover;
using Assets.Sources.Gameplay.World.StateMachine;
using Assets.Sources.Gameplay.World.StateMachine.States;
using Assets.Sources.Infrastructure.Factories.UiFactory;
using Assets.Sources.Services.StaticDataService.Configs.Building;
using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Assets.Sources.UI.Windows.World.Panels.Store
{
    public class WorldBuildingsStore : MonoBehaviour
    {
        private IUiFactory _uiFactory;
        private ICurrencyWorldData _worldData;
        private List<BuildingStoreItem> _storeItems;
        private ICurrencyGameplayMover _gameplayMover;
        private WorldStateMachine _worldStateMachine;

        [Inject]
        private async void Construct(IUiFactory uiFactory, ICurrencyWorldData worldData, ICurrencyGameplayMover gameplayMover, WorldStateMachine worldStateMachine)
        {
            _uiFactory = uiFactory;
            _worldData = worldData;
            _gameplayMover = gameplayMover;
            _worldStateMachine = worldStateMachine;

            _storeItems = new();

            _worldData.WorldStore.BuildingsStoreListUpdated += OnBuildingsStoreListUpdated;

            foreach (BuildingStoreItemData data in _worldData.WorldStore.BuildingsStoreList)
                await CreateStoreItem(data.Type);
        }

        private void OnDestroy()
        {
            _worldData.WorldStore.BuildingsStoreListUpdated -= OnBuildingsStoreListUpdated;

            foreach (var storeItem in _storeItems)
                storeItem.Buyed -= OnStoreItemBuyed;
        }

        private void OnStoreItemBuyed(BuildingType buildingType, uint price)
        {
            if (_worldData.WorldWallet.TryGet(price))
            {
                _worldData.WorldStore.GetBuildingData(buildingType).ChangeBuyingCount();
                _gameplayMover.ChangeBuildingForPlacing(buildingType, price);
                _worldStateMachine.Enter<WorldChangingState>().Forget();
            }
        }

        private async UniTask CreateStoreItem(BuildingType buildingType)
        {
            BuildingStoreItem storeItem = await _uiFactory.CreateStoreItem(buildingType, transform);

            storeItem.Buyed += OnStoreItemBuyed;
            _storeItems.Add(storeItem);
        }

        private async void OnBuildingsStoreListUpdated(BuildingType type) =>
            await CreateStoreItem(type);
    }
}
