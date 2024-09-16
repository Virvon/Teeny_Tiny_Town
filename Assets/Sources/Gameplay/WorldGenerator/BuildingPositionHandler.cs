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
        private Tile.Tile _handlePressedMoveStartTile;
        private bool _isBuildingPressed;
        private Camera _camera;


        [Inject]
        private void Construct(IInputService inputService)
        {
            _inputService = inputService;

            _selectFrame = Instantiate(_selectFramePrefab);

            _camera = Camera.main;

            _inputService.HandleMoved += OnHandleMoved;
            _inputService.Pressed += OnPressed;
            _inputService.HandlePressedMoveStarted += OnHandlePressedMoveStarted;
            _inputService.HandlePressedMovePerformed += OnHandlePressedMovePerformed;
        }

        public event Action<Vector2Int, BuildingType> BuildingCreated;

        public void Set(Building building, Tile.Tile buildingTile)
        {
            _building = building;
            buildingTile.Select(_selectFrame, _selectFramePositionOffset);

            _selectFrame.gameObject.SetActive(true);
        }

        private void OnHandleMoved(Vector2 handlePosition)
        {
            if (_building == null)
            {
                return;
            }

            if(CheckTileIntersection(handlePosition, out Tile.Tile tile))
            {
                tile.Select(_selectFrame, _selectFramePositionOffset);
                _selectFrame.gameObject.SetActive(true);

                if (_isBuildingPressed == false)
                    tile.PutBuilding(_building);
            }
            else if (_isBuildingPressed)
            {
                _selectFrame.gameObject.SetActive(false);
            }

            if (_isBuildingPressed)
            {
                Ray ray = GetRay(handlePosition);
                Plane groundPlane = new Plane(Vector3.up, new Vector3(0, _buildingPressedMoveHeight, 0));

                if (groundPlane.Raycast(ray, out float distanceToPlane))
                {
                    Vector3 worldPosition = ray.GetPoint(distanceToPlane);

                    _building.transform.position = new Vector3(worldPosition.x, _buildingPressedMoveHeight, worldPosition.z);
                }
            }
        }

        private void OnPressed(Vector2 handlePosition)
        {
            if (_building == null)
                return;

            if(CheckTileIntersection(handlePosition, out Tile.Tile tile))
            {
                _selectFrame.gameObject.SetActive(false);
                tile.PutBuilding(_building);
                tile.SetBuilding(_building);
                
                BuildingCreated?.Invoke(tile.GridPosition, _building.Type);
            }
            else if (_isBuildingPressed)
            {
                _handlePressedMoveStartTile.PutBuilding(_building);
                _handlePressedMoveStartTile.Select(_selectFrame, _selectFramePositionOffset);
                _selectFrame.gameObject.SetActive(true);
            }

            _isBuildingPressed = false;
        }

        private void OnHandlePressedMovePerformed(Vector2 handlePosition)
        {
            if(CheckTileIntersection(handlePosition, out Tile.Tile tile) && tile == _handlePressedMoveStartTile)
            {
                _isBuildingPressed = true;
            }
        }

        private void OnHandlePressedMoveStarted(Vector2 handlePosition)
        {
            if (CheckTileIntersection(handlePosition, out Tile.Tile tile))
                _handlePressedMoveStartTile = tile;
        }

        private bool CheckTileIntersection(Vector2 handlePosition, out Tile.Tile tile)
        {
            tile = null;

            if(Physics.Raycast(GetRay(handlePosition), out RaycastHit hitInfo, _raycastDistance, _layerMask, QueryTriggerInteraction.Ignore)
                && hitInfo.transform.TryGetComponent(out GroundCollider groundCollider))
            {
                tile = groundCollider.Tile;

                return true;
            }

            return false;
        }

        private Ray GetRay(Vector2 handlePosition) =>
            _camera.ScreenPointToRay(new Vector3(handlePosition.x, handlePosition.y, 1));
    }
}
