using Assets.Sources.Data;
using Assets.Sources.Gameplay.GameplayMover;
using Assets.Sources.Gameplay.World.StateMachine;
using Assets.Sources.Gameplay.World.StateMachine.States;
using Assets.Sources.Infrastructure.Factories.UiFactory;
using Assets.Sources.Services.StaticDataService.Configs.Building;
using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Assets.Sources.Gameplay.Store
{
    public class ItemsStore : MonoBehaviour
    {
        private IUiFactory _uiFactory;
        private IWorldData _worldData;
        private List<StoreItem> _storeItems;
        private IGameplayMover _gameplayMover;
        private WorldStateMachine _worldStateMachine;

        [Inject]
        private void Construct(IUiFactory uiFactory, IWorldData worldData, IGameplayMover gameplayMover, WorldStateMachine worldStateMachine)
        {
            _uiFactory = uiFactory;
            _worldData = worldData;
            _gameplayMover = gameplayMover;
            _worldStateMachine = worldStateMachine;

            _storeItems = new ();

            _worldData.StoreListUpdated += OnStoreListUpdated;
        }

        private async void Start()
        {
            foreach (var buildingType in _worldData.StoreList)
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
            StoreItem storeItem = await _uiFactory.CreateStoreItem(buildingType, transform);

            storeItem.Buyed += OnStoreItemBuyed;
            _storeItems.Add(storeItem);
        }

        private async void OnStoreListUpdated(BuildingType type) => 
            await CreateStoreItem(type);
    }
    public class Store
    {
        public Store(WorldData worldData)
        {

        }
    }
}
