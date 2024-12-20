﻿using Assets.Sources.Data.World.Currency;
using Assets.Sources.Utils;
using TMPro;
using UnityEngine;
using Zenject;

namespace Assets.Sources.UI.Windows.World.Panels
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

            OnWorldWalletValueChanged(_worldData.WorldWallet.Value);
        }

        private void OnDestroy() =>
            _worldData.WorldWallet.ValueChanged -= OnWorldWalletValueChanged;

        private void OnWorldWalletValueChanged(uint value) =>
            _walletValue.text = DigitUtils.CutDigit(value);
    }
}
