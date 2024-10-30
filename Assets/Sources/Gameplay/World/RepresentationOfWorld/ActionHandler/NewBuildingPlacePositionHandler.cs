using Cysharp.Threading.Tasks;
using UnityEngine;
using Assets.Sources.Gameplay.World.RepresentationOfWorld.Tiles;
using Assets.Sources.Gameplay.GameplayMover;
using Assets.Sources.Gameplay.World.RepresentationOfWorld.Markers;

namespace Assets.Sources.Gameplay.World.RepresentationOfWorld.ActionHandler
{
    public class NewBuildingPlacePositionHandler : ActionHandlerState
    {
        private const float PressedBuildingHeight = 5;

        private readonly BuildingMarker _buildingMarker;
        private readonly WorldRepresentationChanger _worldRepresentationChanger;
        private readonly MarkersVisibility _markersVisibility;

        private TileRepresentation _handlePressedMoveStartTile;
        private bool _isBuildingPressed;
        private TileRepresentation _lastSelectedTile;

        public NewBuildingPlacePositionHandler(
            SelectFrame selectFrame,
            LayerMask layerMask,
            IGameplayMover gameplayMover,
            WorldRepresentationChanger worldRepresentationChanger,
            MarkersVisibility markersVisibility,
            BuildingMarker buildingMarker)
            : base(selectFrame, layerMask, gameplayMover)
        {
            _worldRepresentationChanger = worldRepresentationChanger;
            _markersVisibility = markersVisibility;
            _buildingMarker = buildingMarker;

            _worldRepresentationChanger.GameplayMoved += StartPlacing;
        }

        ~NewBuildingPlacePositionHandler() =>
            _worldRepresentationChanger.GameplayMoved -= StartPlacing;

        public override UniTask Enter()
        {
            if(_lastSelectedTile != null)
            {
                SelectFrame.Select(_lastSelectedTile);
                _markersVisibility.SetSelectFrameShowed(true);
            }

            _markersVisibility.SetBuildingShowed(true);
            

            return default;
        }

        public override UniTask Exit()
        {
            _markersVisibility.SetBuildingShowed(false);
            _markersVisibility.SetSelectFrameShowed(false);

            return default;
        }

        public void StartPlacing()
        {
            TileRepresentation startTile = _worldRepresentationChanger.StartTile;

            _lastSelectedTile = startTile;

            _buildingMarker.Mark(startTile);
            SelectFrame.Select(startTile);
            _markersVisibility.SetBuildingShowed(true);
            _markersVisibility.SetSelectFrameShowed(true);
        }

        public override void OnHandleMoved(Vector2 handlePosition)
        {
            if (CheckTileIntersection(handlePosition, out TileRepresentation tile)
                && tile.IsEmpty
                && CheckBuildingAndTileCompatibility(_buildingMarker.BuildingType, tile.Type))
            {
                SelectFrame.Select(tile);
                _markersVisibility.SetSelectFrameShowed(true);
                _lastSelectedTile = tile;

                if (_isBuildingPressed == false)
                    _buildingMarker.Mark(tile);
            }
            else if (_isBuildingPressed)
            {
                _markersVisibility.SetSelectFrameShowed(false);
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
            if (CheckTileIntersection(handlePosition, out TileRepresentation tile)
                && tile.IsEmpty
                && CheckBuildingAndTileCompatibility(_buildingMarker.BuildingType, tile.Type))
            {
                _markersVisibility.SetBuildingShowed(false);
                _markersVisibility.SetSelectFrameShowed(false);

                _lastSelectedTile = null;

                GameplayMover.PlaceNewBuilding(tile.GridPosition, _buildingMarker.BuildingType);
            }
            else if (_isBuildingPressed)
            {
                _buildingMarker.Mark(_handlePressedMoveStartTile);
                SelectFrame.Select(_handlePressedMoveStartTile);
                _markersVisibility.SetSelectFrameShowed(true);

                _lastSelectedTile = _handlePressedMoveStartTile;
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