using Assets.Sources.Gameplay.World;
using TMPro;
using UnityEngine;
using Zenject;

namespace Assets.Sources.UI
{
    public class WalletPanel : MonoBehaviour
    {
        [SerializeField] private TMP_Text _walletValue;

        private World _world;

        [Inject]
        private void Construct(World world)
        {
            _world = world;

            _world.WorldData.WorldWallet.ValueChanged += OnWorldWalletValueChanged;
        }

        private void OnDestroy() =>
            _world.WorldData.WorldWallet.ValueChanged -= OnWorldWalletValueChanged;

        private void OnWorldWalletValueChanged(uint value) =>
            _walletValue.text = value.ToString();
    }
}
