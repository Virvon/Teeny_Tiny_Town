using Assets.Sources.Gameplay.World.WorldInfrastructure;
using Assets.Sources.Infrastructure.Factories.UiFactory;
using Assets.Sources.Services.PersistentProgress;
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
        private IPersistentProgressService _persisitentProgressService;
        private List<StoreItem> _storeItems;

        [Inject]
        private void Construct(IUiFactory uiFactory, IPersistentProgressService persistentProgressService)
        {
            _uiFactory = uiFactory;
            _persisitentProgressService = persistentProgressService;

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
            if (_persisitentProgressService.Progress.WorldWallet.TryGet(cost))
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
