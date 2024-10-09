using Assets.Sources.Infrastructure.Factories.UiFactory;
using Assets.Sources.Services.PersistentProgress;
using Assets.Sources.Services.StaticDataService.Configs.Building;
using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Assets.Sources.Gameplay.Store
{
    public class ItemsStore : MonoBehaviour
    {
        [SerializeField] private List<BuildingType> _availableBuildings;

        private IUiFactory _uiFactory;
        private World.World _world;
        private List<StoreItem> _storeItems;

        [Inject]
        private void Construct(IUiFactory uiFactory, World.World world)
        {
            _uiFactory = uiFactory;
            _world = world;

            _storeItems = new ();
        }

        private async void Start()
        {
            foreach (var buildingType in _availableBuildings)
            {
                StoreItem storeItem = await _uiFactory.CreateStoreItem(buildingType, transform);

                storeItem.Buyed += OnStoreItemBuyed;
                _storeItems.Add(storeItem);
            }
        }

        private void OnDestroy()
        {
            foreach (var storeItem in _storeItems)
                storeItem.Buyed -= OnStoreItemBuyed;
        }

        private void OnStoreItemBuyed(BuildingType buildingType, uint cost)
        {
            if (_world.WorldData.WorldWallet.TryGet(cost))
            {
                Debug.Log("sucsess");
            }
            else
            {
                Debug.Log("no money");
            }
        }
    }
}
