using Assets.Sources.Data.WorldDatas.Currency;
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

            foreach (var buildingType in _worldData.WorldStore.BuildingsStoreList)
                await CreateStoreItem(buildingType);
        }

        private void OnDestroy()
        {
            foreach (var storeItem in _storeItems)
                storeItem.Buyed -= OnStoreItemBuyed;
        }

        private void OnStoreItemBuyed(BuildingType buildingType, uint price)
        {
            if (_worldData.WorldWallet.TryGet(price))
            {
                _gameplayMover.ChangeBuildingForPlacing(buildingType, price);
                _worldStateMachine.Enter<WorldChangingState>().Forget();
            }
            else
            {
                Debug.Log("no money");
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
