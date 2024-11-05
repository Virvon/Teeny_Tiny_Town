using Assets.Sources.Gameplay.World;
using Assets.Sources.Services.PersistentProgress;
using Assets.Sources.Services.StaticDataService;
using Assets.Sources.Services.StaticDataService.Configs.World;
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
        private WorldsList _worldsList;

        private uint _currentWorldCost;

        [Inject]
        private void Construct(IPersistentProgressService persistentProgressService, IStaticDataService staticDataService, WorldsList worldsList)
        {
            _persistentProgressService = persistentProgressService;
            _staticDataServcie = staticDataService;
            _worldsList = worldsList;

            _buyButton.onClick.AddListener(OnBuyButtonClicked);
        }

        private void OnDestroy()
        {
            _buyButton.onClick.RemoveListener(OnBuyButtonClicked);
        }

        public override void Open()
        {
            base.Open();

            WorldConfig worldConfig = _staticDataServcie.GetWorld<WorldConfig>(_worldsList.CurrentWorldData.Id);
            _currentWorldCost = worldConfig.Cost;
            _costValue.text = _currentWorldCost.ToString();
        }

        private void OnBuyButtonClicked()
        {
            if(_persistentProgressService.Progress.Wallet.TryGet(_currentWorldCost))
            {
                _worldsList.CurrentWorldData.IsUnlocked = true;
                _mapSelectionWindow.ChangeCurrentWorldInfo(_worldsList.CurrentWorldData);
            }
        }
    }
}
