using Assets.Sources.Gameplay.World.WorldInfrastructure.NextBuildingForPlacing;
using Assets.Sources.Services.AssetManagement;
using Assets.Sources.Services.StaticDataService;
using Assets.Sources.Services.StaticDataService.Configs.Building;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Assets.Sources.UI.Windows.World.Panels
{
    public class NextBuildingPanel : MonoBehaviour
    {
        [SerializeField] private Image _icon;

        private NextBuildingForPlacingCreator _nextBuildingForPlacingCreator;
        private IStaticDataService _staticDataService;
        private IAssetProvider _assetProvider;

        [Inject]
        private void Construct(NextBuildingForPlacingCreator nextBuildingForPlacingCreator, IStaticDataService staticDataService, IAssetProvider assetProvider)
        {
            _nextBuildingForPlacingCreator = nextBuildingForPlacingCreator;
            _staticDataService = staticDataService;
            _assetProvider = assetProvider;

            OnBuildingForPlacingDataChanged(_nextBuildingForPlacingCreator.BuildingsForPlacingData);

            _nextBuildingForPlacingCreator.DataChanged += OnBuildingForPlacingDataChanged;
        }

        private void OnDestroy() =>
            _nextBuildingForPlacingCreator.DataChanged -= OnBuildingForPlacingDataChanged;

        private async void OnBuildingForPlacingDataChanged(BuildingsForPlacingData data)
        {
            BuildingConfig buildingConfig = _staticDataService.GetBuilding<BuildingConfig>(data.NextBuildingType);
            _icon.sprite = await _assetProvider.Load<Sprite>(buildingConfig.IconAssetReference);
            _icon.SetNativeSize();
        }
    }
}
