using Assets.Sources.Data.World.Currency;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace Assets.Sources.Audio
{
    public class WorldWalletSoundPlayer : MonoBehaviour
    {
        [SerializeField] private AudioSource _audioSource;

        private ICurrencyWorldData _currencyWorldData;

        [Inject]
        private void Construct(ICurrencyWorldData currencyWorldData)
        {
            _currencyWorldData = currencyWorldData;

            _currencyWorldData.WorldWallet.ValueChanged += OnWalletValueChanged;
        }

        private void OnDestroy() =>
            _currencyWorldData.WorldWallet.ValueChanged -= OnWalletValueChanged;

        private void OnWalletValueChanged(uint obj) =>
            _audioSource.Play();

        public class Factory : PlaceholderFactory<string, UniTask<WorldWalletSoundPlayer>>
        {
        }
    }
}
