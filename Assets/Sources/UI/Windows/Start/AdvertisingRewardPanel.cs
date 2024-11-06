using Assets.Sources.Services.PersistentProgress;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Assets.Sources.UI.Windows.Start
{
    public class AdvertisingRewardPanel : MonoBehaviour
    {
        [SerializeField] private uint _reward;
        [SerializeField] private TMP_Text _rewardValue;
        [SerializeField] private Button _button;

        private IPersistentProgressService _persistnetProgressService;

        [Inject]
        private void Construct(IPersistentProgressService persistentProgressService)
        {
            _persistnetProgressService = persistentProgressService;

            _rewardValue.text = "+" + _reward.ToString();

            _button.onClick.AddListener(OnButtonClicked);
        }

        private void OnDestroy() =>
            _button.onClick.RemoveListener(OnButtonClicked);

        private void OnButtonClicked()
        {
#if !UNITY_EDITOR && UNITY_WEBGL
            InterstitialAd.Show(onCloseCallback: (_) =>
            {
                _persistnetProgressService.Progress.Wallet.Give(_reward);
            });
#else
            _persistnetProgressService.Progress.Wallet.Give(_reward);
#endif
        }
    }
}
