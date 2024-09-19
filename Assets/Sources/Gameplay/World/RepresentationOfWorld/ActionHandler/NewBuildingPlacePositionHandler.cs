using Assets.Sources.Gameplay.World.WorldInfrastructure;
using Cysharp.Threading.Tasks;
using System;
using UnityEngine;
using Tile = Assets.Sources.Gameplay.World.RepresentationOfWorld.Tiles.Tile;
using Building = Assets.Sources.Gameplay.World.RepresentationOfWorld.Tiles.Building;

namespace Assets.Sources.Gameplay.World.RepresentationOfWorld.ActionHandler
{
    public class NewBuildingPlacePositionHandler : ActionHandlerState
    {
        private const float PressedBuildingHeight = 5;

        private Building _buildingForPlacing;
        private Tile _handlePressedMoveStartTile;
        private bool _isBuildingPressed;
        private Tile _lastSelectedTile;

        public NewBuildingPlacePositionHandler(SelectFrame selectFrame, LayerMask layerMask) : base(selectFrame, layerMask)
        {
        }

        public event Action<Vector2Int, BuildingType> Placed;

        public override UniTask Enter()
        {
            if (_buildingForPlacing != null)
                _buildingForPlacing.gameObject.SetActive(true);

            if (_lastSelectedTile != null)
                Select(_lastSelectedTile);

            return default;
        }

        public override UniTask Exit()
        {
            SelectFrame.Hide();
            _buildingForPlacing.gameObject.SetActive(false);

            return default;
        }

        public void SetNewBuilding(Building building, Tile buildingTile)
        {
            if (_buildingForPlacing != null)
                _buildingForPlacing.Destroy();

            _buildingForPlacing = building;
            Select(buildingTile);

            SelectFrame.gameObject.SetActive(true);
        }

        public override void OnHandleMoved(Vector2 handlePosition)
        {
            if (_buildingForPlacing == null)
                return;

            if (CheckTileIntersection(handlePosition, out Tile tile) && tile.IsEmpty)
            {
                Select(tile);

                if (_isBuildingPressed == false)
                    tile.Replace(_buildingForPlacing);
            }
            else if (_isBuildingPressed)
            {
                SelectFrame.Hide();
            }

            if (_isBuildingPressed)
            {
                Ray ray = GetRay(handlePosition);
                Plane plane = new Plane(Vector3.up, new Vector3(0, PressedBuildingHeight, 0));

                if (plane.Raycast(ray, out float distanceToPlane))
                {
                    Vector3 worldPosition = ray.GetPoint(distanceToPlane);

                    _buildingForPlacing.transform.position = new Vector3(worldPosition.x, PressedBuildingHeight, worldPosition.z);
                }
            }
        }

        public override void OnPressed(Vector2 handlePosition)
        {
            if (_buildingForPlacing == null)
                return;

            if (CheckTileIntersection(handlePosition, out Tile tile) && tile.IsEmpty)
            {
                tile.PlaceBuilding(_buildingForPlacing);
                SelectFrame.Hide();

                Placed?.Invoke(tile.GridPosition, _buildingForPlacing.Type);
            }
            else if (_isBuildingPressed)
            {
                _handlePressedMoveStartTile.Replace(_buildingForPlacing);
                Select(_handlePressedMoveStartTile);
            }

            _handlePressedMoveStartTile = null;
            _isBuildingPressed = false;
        }

        public override void OnHandlePressedMovePerformed(Vector2 handlePosition)
        {
            if (CheckTileIntersection(handlePosition, out Tile tile) && tile == _handlePressedMoveStartTile)
                _isBuildingPressed = true;
        }

        public override void OnHandlePressedMoveStarted(Vector2 handlePosition)
        {
            if (CheckTileIntersection(handlePosition, out Tile tile))
                _handlePressedMoveStartTile = tile;
        }

        private void Select(Tile tile)
        {
            SelectFrame.Select(tile);
            _lastSelectedTile = tile;
        }
    }
}