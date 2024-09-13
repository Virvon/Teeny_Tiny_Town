using Assets.Sources.Gameplay.Tile;
using Assets.Sources.Services.Input;
using System;
using UnityEngine;
using Zenject;

namespace Assets.Sources.Gameplay.WorldGenerator
{
    public class BuildingPositionHandler : MonoBehaviour
    {
        [SerializeField] private float _buildingPressedMoveHeight;
        [SerializeField] private float _sensitivity;
        [SerializeField] private SelectFrame _selectFramePrefab;
        [SerializeField] private Vector3 _selectFramePositionOffset;
        [SerializeField] private float _raycastDistance;
        [SerializeField] private LayerMask _layerMask;

        private IInputService _inputService;
        private SelectFrame _selectFrame;
        private Building _building;
        private Ground _handlePressedMoveStartGround;
        private bool _isBuildingPressed;

        private Vector3 buildingPosition;
        private Vector3 fromCamera;
        private Vector3 result;


        [Inject]
        private void Construct(IInputService inputService)
        {
            _inputService = inputService;

            _selectFrame = Instantiate(_selectFramePrefab);

            _inputService.HandleMoved += OnHandleMoved;
            _inputService.Pressed += OnPressed;
            _inputService.HandlePressedMoveStarted += OnHandlePressedMoveStarted;
            _inputService.HandlePressedMovePerformed += OnHandlePressedMovePerformed;
        }

        public event Action BuildingCreated;

        public void Set(Building building)
        {
            _building = building;
            _selectFrame.transform.position = _building.Ground.BuildingPoint.position + _selectFramePositionOffset;
            _selectFrame.gameObject.SetActive(true);
        }

        private void OnHandleMoved(Vector2 handlePosition)
        {
            if (_building == null)
                return;

            Ray ray = Camera.main.ScreenPointToRay(new Vector3(handlePosition.x, handlePosition.y, 1));

            if (_isBuildingPressed)
            {
                Plane groundPlane = new Plane(Vector3.up, new Vector3(0, _buildingPressedMoveHeight, 0));

                float distanceToPlane;

                if (groundPlane.Raycast(ray, out distanceToPlane))
                {
                    Vector3 worldPosition = ray.GetPoint(distanceToPlane);

                    _building.transform.position = new Vector3(worldPosition.x, _buildingPressedMoveHeight, worldPosition.z);
                }

                if (Physics.Raycast(ray, out RaycastHit hitInfo, _raycastDistance, _layerMask, QueryTriggerInteraction.Ignore) && hitInfo.transform.TryGetComponent(out Ground ground))
                {
                    _selectFrame.transform.position = ground.BuildingPoint.position + _selectFramePositionOffset;
                    _selectFrame.gameObject.SetActive(true);
                }
                else
                    _selectFrame.gameObject.SetActive(false);
            }
            else
            {
                if (Physics.Raycast(ray, out RaycastHit hitInfo, _raycastDistance, _layerMask, QueryTriggerInteraction.Ignore) && hitInfo.transform.TryGetComponent(out Ground ground))
                {
                    _selectFrame.transform.position = ground.BuildingPoint.position + _selectFramePositionOffset;
                    _selectFrame.gameObject.SetActive(true);
                    _building.transform.position = ground.BuildingPoint.position;
                }
            }
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(Vector3.zero, buildingPosition);
            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(Vector3.zero, fromCamera);
            Gizmos.color = Color.green;
            Gizmos.DrawLine(Vector3.zero, result);

        }

        private void OnPressed(Vector2 handlePosition)
        {
            Ray ray = Camera.main.ScreenPointToRay(new Vector3(handlePosition.x, handlePosition.y, 1));

            if (_isBuildingPressed)
            {
                if (Physics.Raycast(ray, out RaycastHit hitInfo, _raycastDistance, _layerMask, QueryTriggerInteraction.Ignore) && hitInfo.transform.TryGetComponent(out Ground ground))
                {
                    _selectFrame.gameObject.SetActive(false);
                    _building.transform.position = ground.BuildingPoint.position;
                    _building = null;
                    BuildingCreated?.Invoke();
                }
                else
                {
                    _building.ResetPosition();
                    _selectFrame.transform.position = _building.Ground.BuildingPoint.position + _selectFramePositionOffset;
                    _selectFrame.gameObject.SetActive(true);
                }

                _isBuildingPressed = false;
            }
            else
            {
                if (Physics.Raycast(ray, out RaycastHit hitInfo, _raycastDistance, _layerMask, QueryTriggerInteraction.Ignore) && hitInfo.transform.TryGetComponent(out Ground ground))
                {
                    _selectFrame.gameObject.SetActive(false);
                    _building = null;
                    BuildingCreated?.Invoke();
                }
            }
        }

        private void OnHandlePressedMovePerformed(Vector2 handlePosition)
        {
            Ray ray = Camera.main.ScreenPointToRay(new Vector3(handlePosition.x, handlePosition.y, 1));

            if (Physics.Raycast(ray, out RaycastHit hitInfo, _raycastDistance, _layerMask, QueryTriggerInteraction.Ignore) && hitInfo.transform.TryGetComponent(out Ground ground) && ground == _handlePressedMoveStartGround)
            {
                _isBuildingPressed = true;
                _building.Ground = ground;
            }
        }

        private void OnHandlePressedMoveStarted(Vector2 handlePosition)
        {
            Ray ray = Camera.main.ScreenPointToRay(new Vector3(handlePosition.x, handlePosition.y, 1));

            if (Physics.Raycast(ray, out RaycastHit hitInfo, _raycastDistance, _layerMask, QueryTriggerInteraction.Ignore) && hitInfo.transform.TryGetComponent(out Ground ground))
            {
                _handlePressedMoveStartGround = ground;
            }
        }
    }
}
