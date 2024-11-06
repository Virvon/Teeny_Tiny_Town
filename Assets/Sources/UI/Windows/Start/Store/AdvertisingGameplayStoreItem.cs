using Agava.YandexGames;
using Assets.Sources.Services.PersistentProgress;
using Assets.Sources.Services.SaveLoadProgress;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Assets.Sources.UI.Windows.Start.Store
{
    public class AdvertisingGameplayStoreItem : MonoBehaviour
    {
        [SerializeField] private uint _reward;
        [SerializeField] private Button _button;
        [SerializeField] private TMP_Text _rewardValue;

        private IPersistentProgressService _persistentProgressServcie;
        private ISaveLoadService _saveLoadService;

        [Inject]
        private void Construct(IPersistentProgressService persistentProgressService, ISaveLoadService saveLoadService)
        {
            _persistentProgressServcie = persistentProgressService;
            _saveLoadService = saveLoadService;

            _rewardValue.text = _reward.ToString();

            _button.onClick.AddListener(OnButtonClicked);
        }

        private void OnDestroy() =>
            _button.onClick.RemoveListener(OnButtonClicked);

        private void OnButtonClicked()
        {
#if !UNITY_EDITOR && UNITY_WEBGL
            InterstitialAd.Show(onCloseCallback: (_) =>
            {
                _persistentProgressServcie.Progress.Wallet.Give(_reward);
                _saveLoadService.SaveProgress();
            });
#else
            _persistentProgressServcie.Progress.Wallet.Give(_reward);
            _saveLoadService.SaveProgress();
#endif
        }
    }
}
