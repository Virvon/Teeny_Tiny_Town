using Assets.Sources.Gameplay.World.RepresentationOfWorld.ActionHandler;
using Assets.Sources.Gameplay.World.RepresentationOfWorld.Tiles;
using Assets.Sources.Gameplay.World.WorldInfrastructure.WorldChangers;
using Assets.Sources.Services.StaticDataService.Configs.Building;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Assets.Sources.Gameplay.Inventory
{
    public class InventoryCell : MonoBehaviour
    {
        [SerializeField] private Button _button;
        [SerializeField] private TMP_Text _buildingName;

        private ActionHandlerStateMachine _actionHandlerStateMachine;
        private BuildingMarker _buildingMarker;
        private IWorldChanger _worldChanger;

        private BuildingType _buildingType;

        [Inject]
        private void Construct(ActionHandlerStateMachine actionHandlerStateMachine, BuildingMarker buildingMarker, IWorldChanger worldChanger)
        {
            _actionHandlerStateMachine = actionHandlerStateMachine;
            _buildingMarker = buildingMarker;
            _worldChanger = worldChanger;
        }

        private void OnEnable()
        {
            _button.onClick.AddListener(OnButtonClicked);
        }

        private void OnDisable()
        {
            _button.onClick.RemoveListener(OnButtonClicked);
        }

        private async void OnButtonClicked()
        {
            if (_actionHandlerStateMachine.CurrentState is not NewBuildingPlacePositionHandler || _buildingMarker.IsCreatedBuilding)
                return;
            else if(_buildingType != BuildingType.Undefined)
                _buildingType = await _buildingMarker.SwapBuilding(_buildingType);
            else
                _buildingType = await _buildingMarker.SwapBuilding(_worldChanger.UpdateBuildingForPlacingType());

            _buildingName.text = _buildingType.ToString();
        }
    }
}
