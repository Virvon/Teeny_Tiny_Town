using Assets.Sources.Data.World;
using Assets.Sources.Services.PersistentProgress;
using Assets.Sources.Services.SaveLoadProgress;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Assets.Sources.UI.Windows.Start.Store
{
    public class GainsGameplayStoreItem : MonoBehaviour
    {
        const uint ItemsCount = 2;

        [SerializeField] private uint _cost;
        [SerializeField] private Button _buyButton;
        [SerializeField] private TMP_Text _costValue;

        private IPersistentProgressService _persistentProgressServcie;
        private ISaveLoadService _saveLoadService;

        [Inject]
        private void Construct(IPersistentProgressService persistentProgressService, ISaveLoadService saveLoadService)
        {
            _persistentProgressServcie = persistentProgressService;
            _saveLoadService = saveLoadService;

            _costValue.text = _cost.ToString();

            _buyButton.onClick.AddListener(OnBuyButtonClicked);
        }

        private void OnDestroy() =>
            _buyButton.onClick.RemoveListener(OnBuyButtonClicked);

        private void OnBuyButtonClicked()
        {
            if(_persistentProgressServcie.Progress.Wallet.TryGet(_cost))
            {
                WorldData worldData = _persistentProgressServcie.Progress.GetWorldData(_persistentProgressServcie.Progress.LastPlayedWorldDataId);

                worldData.BulldozerItems.AddItems(ItemsCount);
                worldData.ReplaceItems.AddItems(ItemsCount);

                _saveLoadService.SaveProgress();
            }
        }
    }
}
