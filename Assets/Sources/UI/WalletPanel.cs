using Assets.Sources.Data;
using TMPro;
using UnityEngine;
using Zenject;

namespace Assets.Sources.UI
{
    public class WalletPanel : MonoBehaviour
    {
        [SerializeField] private TMP_Text _walletValue;

        private WorldData _worldData;

        [Inject]
        private void Construct(WorldData worldData)
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
