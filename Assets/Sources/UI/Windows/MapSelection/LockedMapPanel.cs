using Assets.Sources.Gameplay.World;
using Assets.Sources.Services.PersistentProgress;
using Assets.Sources.Services.SaveLoadProgress;
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
        private ISaveLoadService _saveLoadService;

        private uint _currentWorldCost;

        [Inject]
        private void Construct(
            IPersistentProgressService persistentProgressService,
            IStaticDataService staticDataService,
            WorldsList worldsList,
            ISaveLoadService saveLoadService)
        {
            _persistentProgressService = persistentProgressService;
            _staticDataServcie = staticDataService;
            _worldsList = worldsList;
            _saveLoadService = saveLoadService;

            _buyButton.onClick.AddListener(OnBuyButtonClicked);
        }

        private void OnDestroy() =>
            _buyButton.onClick.RemoveListener(OnBuyButtonClicked);

        public override void Open()
        {
            base.Open();

            WorldConfig worldConfig = _staticDataServcie.GetWorld<WorldConfig>(_worldsList.CurrentWorldDataId);
            _currentWorldCost = worldConfig.Cost;
            _costValue.text = _currentWorldCost.ToString();
        }

        private void OnBuyButtonClicked()
        {
            if (_persistentProgressService.Progress.Wallet.TryGet(_currentWorldCost))
            {
                _persistentProgressService.Progress.GetWorldData(_worldsList.CurrentWorldDataId).IsUnlocked = true;
                _mapSelectionWindow.ChangeCurrentWorldInfo(_worldsList.CurrentWorldDataId);
                _saveLoadService.SaveProgress();
            }
        }
    }
}
