using Assets.Sources.Data.WorldDatas;
using TMPro;
using UnityEngine;
using Zenject;

namespace Assets.Sources.UI
{
    public class WorldWalletPanel : MonoBehaviour
    {
        [SerializeField] private TMP_Text _walletValue;

        private ICurrencyWorldData _worldData;

        [Inject]
        private void Construct(ICurrencyWorldData worldData)
        {
            _worldData = worldData;

            _worldData.WorldWallet.ValueChanged += OnWorldWalletValueChanged;
        }

        private void OnDestroy() =>
            _worldData.WorldWallet.ValueChanged -= OnWorldWalletValueChanged;

        private void OnWorldWalletValueChanged(uint value) =>
            _walletValue.text = value.ToString();
    }
}
