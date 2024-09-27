using Cysharp.Threading.Tasks;
using System;
using UnityEngine;
using Assets.Sources.Gameplay.World.RepresentationOfWorld.Tiles;

namespace Assets.Sources.Gameplay.World.RepresentationOfWorld.ActionHandler
{
    public class NewBuildingPlacePositionHandler : ActionHandlerState
    {
        private const float PressedBuildingHeight = 5;

        private readonly BuildingMarker _buildingMarker;
        private readonly GameplayMover.GameplayMover _gameplayMover;
        private readonly WorldRepresentationChanger _worldRepresentationChanger;

        private TileRepresentation _handlePressedMoveStartTile;
        private bool _isBuildingPressed;

        public NewBuildingPlacePositionHandler(
            SelectFrame selectFrame,
            LayerMask layerMask,
            BuildingMarker buildingMarker,
            GameplayMover.GameplayMover gameplayMover,
            WorldRepresentationChanger worldRepresentationChanger)
            : base(selectFrame, layerMask)
        {
            _buildingMarker = buildingMarker;
            _gameplayMover = gameplayMover;
            _worldRepresentationChanger = worldRepresentationChanger;

            _worldRepresentationChanger.GameplayMoved += StartPlacing;
        }

        ~NewBuildingPlacePositionHandler()
        {
            _worldRepresentationChanger.GameplayMoved -= StartPlacing;
        }

        public event Action<Vector2Int> Placed;

        public override UniTask Enter()
        {
            _buildingMarker.Show();
            SelectFrame.SelectLast();

            return default;
        }

        public override UniTask Exit()
        {
            SelectFrame.Hide();
            _buildingMarker.Hide();

            return default;
        }

        public void StartPlacing()
        {
            TileRepresentation startTile = _worldRepresentationChanger.StartTile;

            _buildingMarker.Mark(startTile);
            SelectFrame.Select(startTile);
        }

        public override void OnHandleMoved(Vector2 handlePosition)
        {
            if (CheckTileIntersection(handlePosition, out TileRepresentation tile) && tile.IsEmpty)
            {
                SelectFrame.Select(tile);

                if (_isBuildingPressed == false)
                    _buildingMarker.Mark(tile);
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

                    _buildingMarker.Replace(new Vector3(worldPosition.x, PressedBuildingHeight, worldPosition.z));
                }
            }
        }

        public override void OnPressed(Vector2 handlePosition)
        {
            if (CheckTileIntersection(handlePosition, out TileRepresentation tile) && tile.IsEmpty)
            {
                _buildingMarker.Mark(tile);
                SelectFrame.Hide();

                Placed?.Invoke(tile.GridPosition);
                _gameplayMover.PlaceNewBuilding(tile.GridPosition);
            }
            else if (_isBuildingPressed)
            {
                _buildingMarker.Mark(_handlePressedMoveStartTile);
                SelectFrame.Select(_handlePressedMoveStartTile);
            }

            _handlePressedMoveStartTile = null;
            _isBuildingPressed = false;
        }

        public override void OnHandlePressedMovePerformed(Vector2 handlePosition)
        {
            if (CheckTileIntersection(handlePosition, out TileRepresentation tile) && tile == _handlePressedMoveStartTile)
                _isBuildingPressed = true;
        }

        public override void OnHandlePressedMoveStarted(Vector2 handlePosition)
        {
            if (CheckTileIntersection(handlePosition, out TileRepresentation tile))
                _handlePressedMoveStartTile = tile;
        }
    }
}