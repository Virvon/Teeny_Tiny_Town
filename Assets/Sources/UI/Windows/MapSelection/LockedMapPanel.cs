using Assets.Sources.Services.PersistentProgress;
using Assets.Sources.Services.StaticDataService;
using Assets.Sources.Services.StaticDataService.Configs.World;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Assets.Sources.UI.Windows.MapSelection
{
    public class LockedMapPanel : MapSelectionPanel
    {
        [SerializeField] private Button _buyButton;
        [SerializeField] private TMP_Text _costValue;
        [SerializeField] private MapSelectionWindow _mapSelectionWindow;

        private IPersistentProgressService _persistentProgressService;
        private IStaticDataService _staticDataServcie;

        private uint _currentWorldCost;

        [Inject]
        private void Construct(IPersistentProgressService persistentProgressService, IStaticDataService staticDataService)
        {
            _persistentProgressService = persistentProgressService;
            _staticDataServcie = staticDataService;

            _buyButton.onClick.AddListener(OnBuyButtonClicked);
        }

        private void OnDestroy()
        {
            _buyButton.onClick.RemoveListener(OnBuyButtonClicked);
        }

        public override void Open()
        {
            base.Open();

            WorldConfig worldConfig = _staticDataServcie.GetWorld<WorldConfig>(_persistentProgressService.Progress.CurrentWorldData.Id);
            _currentWorldCost = worldConfig.Cost;
            _costValue.text = _currentWorldCost.ToString();
        }

        private void OnBuyButtonClicked()
        {
            if(_persistentProgressService.Progress.Wallet.TryGet(_currentWorldCost))
            {
                _persistentProgressService.Progress.CurrentWorldData.IsUnlocked = true;
                _mapSelectionWindow.ChangeCurrentWorldInfo();
            }
        }
    }
}
