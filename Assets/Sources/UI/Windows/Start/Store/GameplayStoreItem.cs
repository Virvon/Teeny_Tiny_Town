using System;
using Assets.Sources.Services.PersistentProgress;
using Assets.Sources.Services.SaveLoadProgress;
using Assets.Sources.Services.StaticDataService;
using Assets.Sources.Services.StaticDataService.Configs.GameplayStore;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Assets.Sources.UI.Windows.Start.Store
{
    public class GameplayStoreItem : MonoBehaviour
    {
        [SerializeField] private GameplayStoreItemType _type;
        [SerializeField] private Button _buyButton;
        [SerializeField] private TMP_Text _costValue;

        private IPersistentProgressService _persistentProgressService;
        private ISaveLoadService _saveLoadService;

        private StoreItemConfig _storeItemConfig;

        [Inject]
        private void Construct(IPersistentProgressService persistentProgressService, IStaticDataService staticDataService, ISaveLoadService saveLoadService)
        {
            _persistentProgressService = persistentProgressService;
            _saveLoadService = saveLoadService;

            _storeItemConfig = staticDataService.GetGameplayStorItem(_type);

            _costValue.text = _storeItemConfig.Cost.ToString();

            if (_storeItemConfig.NeedToShow(_persistentProgressService.Progress.StoreData) == false)
                Destroy(gameObject);
        }

        private void OnEnable() =>
            _buyButton.onClick.AddListener(OnBuyButtonClicked);

        private void OnDisable() =>
            _buyButton.onClick.RemoveListener(OnBuyButtonClicked);

        private void OnBuyButtonClicked()
        {
            if (_persistentProgressService.Progress.Wallet.TryGet(_storeItemConfig.Cost))
            {
                _storeItemConfig.Unlock(_persistentProgressService.Progress.StoreData);
                _saveLoadService.SaveProgress();
                Destroy(gameObject);
            }
        }
    }
}
