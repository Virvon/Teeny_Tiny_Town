using Assets.Sources.Collection;
using Assets.Sources.Services.PersistentProgress;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Assets.Sources.UI.Windows
{
    public class CollectionWindow : Window
    {
        [SerializeField] private Button _showNextItemButton;
        [SerializeField] private Button _showPreviousItemButton;
        [SerializeField] private TMP_Text _unlockedBuildingsQuentityValue;
        [SerializeField] private CanvasGroup _placedBuildingsQuantityPanel;
        [SerializeField] private TMP_Text _placedBuildingsQuantityValue;

        private CollectionItemCreator _collectionItemCreator;
        private IPersistentProgressService _persistentProgressService;

        [Inject]
        private void Construct(CollectionItemCreator collectionItemCreator, IPersistentProgressService persistentProgressService)
        {
            _collectionItemCreator = collectionItemCreator;
            _persistentProgressService = persistentProgressService;

            int unlockedBuildingsCount = _persistentProgressService.Progress.BuildingDatas.Count(data => data.IsUnlocked);
            int buildingsCount = _persistentProgressService.Progress.BuildingDatas.Length;

            _unlockedBuildingsQuentityValue.text = $"{unlockedBuildingsCount}/{buildingsCount}";

            OnItemChanged(_persistentProgressService.Progress.BuildingDatas[_collectionItemCreator.CollectionItemIndex].Count);

            _showNextItemButton.onClick.AddListener(OnShowNextItemButtonClicked);
            _showPreviousItemButton.onClick.AddListener(OnShowPreviousItemButtonClicked);
            _collectionItemCreator.ItemChanged += OnItemChanged;
        }

        private void OnDestroy()
        {
            _showNextItemButton.onClick.RemoveListener(OnShowNextItemButtonClicked);
            _showPreviousItemButton.onClick.RemoveListener(OnShowPreviousItemButtonClicked);
            _collectionItemCreator.ItemChanged -= OnItemChanged;
        }

        private async void OnShowPreviousItemButtonClicked() =>
            await _collectionItemCreator.ShowPreviousBuilding();

        private async void OnShowNextItemButtonClicked() =>
            await _collectionItemCreator.ShowNextBuilding();

        private void OnItemChanged(uint placedBuildingsCount)
        {
            _placedBuildingsQuantityValue.text = placedBuildingsCount.ToString();
            _placedBuildingsQuantityPanel.alpha = placedBuildingsCount > 0? 1 : 0;
        }
    }
}
