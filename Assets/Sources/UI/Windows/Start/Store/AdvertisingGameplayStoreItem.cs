using Assets.Sources.Services.PersistentProgress;
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

        [Inject]
        private void Construct(IPersistentProgressService persistentProgressService)
        {
            _persistentProgressServcie = persistentProgressService;

            _rewardValue.text = _reward.ToString();

            _button.onClick.AddListener(OnButtonClicked);
        }

        private void OnDestroy() =>
            _button.onClick.RemoveListener(OnButtonClicked);

        private void OnButtonClicked()
        {
            _persistentProgressServcie.Progress.Wallet.Give(_reward);
        }
    }
}
