using Assets.Sources.Services.PersistentProgress;
using TMPro;
using UnityEngine;
using Zenject;

namespace Assets.Sources.UI
{
    public class WalletPanel : MonoBehaviour
    {
        [SerializeField] private TMP_Text _walletValue;

        private IPersistentProgressService _persistentProgressService;

        [Inject]
        private void Construct(IPersistentProgressService persistentProgressService)
        {
            _persistentProgressService = persistentProgressService;

            _persistentProgressService.Progress.WorldWallet.ValueChanged += OnWorldWalletValueChanged;
        }

        private void OnDestroy()
        {
            _persistentProgressService.Progress.WorldWallet.ValueChanged -= OnWorldWalletValueChanged;
        }

        private void OnWorldWalletValueChanged(uint value)
        {
            _walletValue.text = value.ToString();
        }
    }
}
