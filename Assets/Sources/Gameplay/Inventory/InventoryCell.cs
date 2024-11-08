using Assets.Sources.Data.World;
using Assets.Sources.Gameplay.World.RepresentationOfWorld.ActionHandler;
using Assets.Sources.Gameplay.World.RepresentationOfWorld.Markers;
using Assets.Sources.Gameplay.World.WorldInfrastructure.NextBuildingForPlacing;
using Assets.Sources.Services.AssetManagement;
using Assets.Sources.Services.StaticDataService;
using Assets.Sources.Services.StaticDataService.Configs.Building;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Assets.Sources.Gameplay.Inventory
{
    public class InventoryCell : MonoBehaviour
    {
        [SerializeField] private Button _button;
        [SerializeField] private Image _icon;
        [SerializeField] private CanvasGroup _iconCanvasGroup;
        [SerializeField] private int _serialNumber;

        private ActionHandlerStateMachine _actionHandlerStateMachine;
        private BuildingMarker _buildingMarker;
        private NextBuildingForPlacingCreator _nextBuildingForPlacingCreator;
        private IStaticDataService _staticDataService;
        private IAssetProvider _assetProvider;
        private IWorldData _worldData;

        [Inject]
        private async void Construct(
            ActionHandlerStateMachine actionHandlerStateMachine,
            BuildingMarker buildingMarker,
            NextBuildingForPlacingCreator nextBuildingForPlacingCreator,
            IStaticDataService staticDataService,
            IAssetProvider assetProvider,
            IWorldData worldData)
        {
            _actionHandlerStateMachine = actionHandlerStateMachine;
            _buildingMarker = buildingMarker;
            _nextBuildingForPlacingCreator = nextBuildingForPlacingCreator;
            _staticDataService = staticDataService;
            _assetProvider = assetProvider;
            _worldData = worldData;

            await ChangeIcon();

            _button.onClick.AddListener(OnButtonClicked);
        }

        private BuildingType BuildingType => _worldData.Inventory[_serialNumber];

        private void OnDestroy() =>
            _button.onClick.RemoveListener(OnButtonClicked);

        private async void OnButtonClicked()
        {
            if (_actionHandlerStateMachine.CurrentState is not NewBuildingPlacePositionHandler || _buildingMarker.IsCreatedBuilding)
            {
                return;
            }
            else if (BuildingType != BuildingType.Undefined)
            {
                BuildingType buildingType = _buildingMarker.BuildingType;
                _nextBuildingForPlacingCreator.ChangeCurrentBuildingForPlacing(BuildingType);
                _worldData.Inventory[_serialNumber] = buildingType;
            }
            else
            {
                _worldData.Inventory[_serialNumber] = _buildingMarker.BuildingType;
                _nextBuildingForPlacingCreator.MoveToNextBuilding();
            }

            await ChangeIcon();
        }

        private async UniTask ChangeIcon()
        {
            if (BuildingType == BuildingType.Undefined)
            {
                _iconCanvasGroup.alpha = 0;

                return;
            }

            BuildingConfig buildingConfig = _staticDataService.GetBuilding<BuildingConfig>(BuildingType);
            _icon.sprite = await _assetProvider.Load<Sprite>(buildingConfig.IconAssetReference);
            _icon.SetNativeSize();
            _iconCanvasGroup.alpha = 1;
        }
    }
}
