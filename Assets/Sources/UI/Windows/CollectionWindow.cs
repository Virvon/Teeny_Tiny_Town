using System.Linq;
using Assets.Sources.Collection;
using Assets.Sources.Data;
using Assets.Sources.Infrastructure.GameStateMachine;
using Assets.Sources.Infrastructure.GameStateMachine.States;
using Assets.Sources.Services.PersistentProgress;
using Assets.Sources.Services.StaticDataService;
using Assets.Sources.Services.StaticDataService.Configs.Building;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Assets.Sources.UI.Windows
{
    public class CollectionWindow : Window
    {
        private const string LockedName = "???";
        private const string LockedTitle = "??? ??? ???";

        [SerializeField] private Button _showNextItemButton;
        [SerializeField] private Button _showPreviousItemButton;
        [SerializeField] private TMP_Text _unlockedBuildingsQuentityValue;
        [SerializeField] private CanvasGroup _placedBuildingsQuantityPanel;
        [SerializeField] private TMP_Text _placedBuildingsQuantityValue;
        [SerializeField] private TMP_Text _buildingName;
        [SerializeField] private TMP_Text _buildingTitle;
        [SerializeField] private Button _hideButton;

        private CollectionItemCreator _collectionItemCreator;
        private IPersistentProgressService _persistentProgressService;
        private IStaticDataService _staticDataService;
        private GameStateMachine _gameStateMachine;

        [Inject]
        private void Construct(
            CollectionItemCreator collectionItemCreator,
            IPersistentProgressService persistentProgressService,
            IStaticDataService staticDataService,
            GameStateMachine gameStateMachine)
        {
            _collectionItemCreator = collectionItemCreator;
            _persistentProgressService = persistentProgressService;
            _staticDataService = staticDataService;
            _gameStateMachine = gameStateMachine;

            int unlockedBuildingsCount = _persistentProgressService.Progress.BuildingDatas.Count(data => data.IsUnlocked);
            int buildingsCount = _persistentProgressService.Progress.BuildingDatas.Length;

            _unlockedBuildingsQuentityValue.text = $"{unlockedBuildingsCount}/{buildingsCount}";

            OnItemChanged(_persistentProgressService.Progress.BuildingDatas[_collectionItemCreator.CollectionItemIndex]);

            _showNextItemButton.onClick.AddListener(OnShowNextItemButtonClicked);
            _showPreviousItemButton.onClick.AddListener(OnShowPreviousItemButtonClicked);
            _collectionItemCreator.ItemChanged += OnItemChanged;
            _hideButton.onClick.AddListener(OnHideButtonClicked);
        }

        private void OnDestroy()
        {
            _showNextItemButton.onClick.RemoveListener(OnShowNextItemButtonClicked);
            _showPreviousItemButton.onClick.RemoveListener(OnShowPreviousItemButtonClicked);
            _collectionItemCreator.ItemChanged -= OnItemChanged;
            _hideButton.onClick.RemoveListener(OnHideButtonClicked);
        }

        private async void OnShowPreviousItemButtonClicked() =>
            await _collectionItemCreator.ShowPreviousBuilding();

        private async void OnShowNextItemButtonClicked() =>
            await _collectionItemCreator.ShowNextBuilding();

        private void OnItemChanged(BuildingData buildingData)
        {
            BuildingConfig buildingConfig = _staticDataService.GetBuilding<BuildingConfig>(buildingData.Type);

            _placedBuildingsQuantityValue.text = buildingData.Count.ToString();
            _placedBuildingsQuantityPanel.alpha = buildingData.IsUnlocked ? 1 : 0;

            _buildingName.text = buildingData.IsUnlocked ? buildingConfig.Name : LockedName;
            _buildingTitle.text = buildingData.IsUnlocked ? buildingConfig.Title : LockedTitle;
        }

        private void OnHideButtonClicked() =>
            _gameStateMachine.Enter<GameLoopState>().Forget();
    }
}