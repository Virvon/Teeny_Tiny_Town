using Assets.Sources.Services.PersistentProgress;
using TMPro;
using UnityEngine;
using Zenject;

namespace Assets.Sources.UI
{
    public class GameplayWalletPanel : MonoBehaviour
    {
        [SerializeField] private TMP_Text _value;

        private IPersistentProgressService _persistentProgressService;

        [Inject]
        private void Construct(IPersistentProgressService persistentProgressService)
        {
            _persistentProgressService = persistentProgressService;

            ChangeValue(_persistentProgressService.Progress.Wallet.Value);

            _persistentProgressService.Progress.Wallet.ValueChanged += ChangeValue;
        }

        private void OnDestroy() =>
            _persistentProgressService.Progress.Wallet.ValueChanged -= ChangeValue;

        private void ChangeValue(uint value) =>
            _value.text = value.ToString();
    }
}
