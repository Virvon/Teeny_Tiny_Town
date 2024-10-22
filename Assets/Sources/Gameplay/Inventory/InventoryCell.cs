using Assets.Sources.Gameplay.World.RepresentationOfWorld.ActionHandler;
using Assets.Sources.Gameplay.World.RepresentationOfWorld.Tiles;
using Assets.Sources.Gameplay.World.WorldInfrastructure.NextBuildingForPlacing;
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
        private NextBuildingForPlacingCreator _nextBuildingForPlacingCreator;

        private BuildingType _buildingType;

        [Inject]
        private void Construct(ActionHandlerStateMachine actionHandlerStateMachine, BuildingMarker buildingMarker, NextBuildingForPlacingCreator nextBuildingForPlacingCreator)
        {
            _actionHandlerStateMachine = actionHandlerStateMachine;
            _buildingMarker = buildingMarker;
            _nextBuildingForPlacingCreator = nextBuildingForPlacingCreator;
        }

        private void OnEnable()
        {
            _button.onClick.AddListener(OnButtonClicked);
        }

        private void OnDisable()
        {
            _button.onClick.RemoveListener(OnButtonClicked);
        }

        private void OnButtonClicked()
        {
            if (_actionHandlerStateMachine.CurrentState is not NewBuildingPlacePositionHandler || _buildingMarker.IsCreatedBuilding)
            {
                return;
            }
            else if(_buildingType != BuildingType.Undefined)
            {
                BuildingType buildingType = _buildingMarker.BuildingType;
                _nextBuildingForPlacingCreator.ChangeCurrentBuildingForPlacing(_buildingType);
                _buildingType = buildingType;
            }
            else
            {
                _buildingType = _buildingMarker.BuildingType;
                _nextBuildingForPlacingCreator.MoveToNextBuilding();
            }

            _buildingName.text = _buildingType.ToString();
        }
    }
}
