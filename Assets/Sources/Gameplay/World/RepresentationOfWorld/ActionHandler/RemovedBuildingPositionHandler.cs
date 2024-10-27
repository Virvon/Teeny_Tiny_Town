using Assets.Sources.Gameplay.GameplayMover;
using Assets.Sources.Gameplay.World.RepresentationOfWorld.Markers;
using Assets.Sources.Gameplay.World.RepresentationOfWorld.Tiles;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Assets.Sources.Gameplay.World.RepresentationOfWorld.ActionHandler
{
    public class RemovedBuildingPositionHandler : ActionHandlerState
    {
        private readonly MarkersVisibility _markersVisibility;

        private TileRepresentation _selectedTile;

        public RemovedBuildingPositionHandler(
            SelectFrame selectFrame,
            LayerMask layerMask,
            IGameplayMover gameplayMover,
            MarkersVisibility markersVisibility)
            : base(selectFrame, layerMask, gameplayMover)
        {
            _markersVisibility = markersVisibility;
        }

        public override UniTask Enter() =>
            default;

        public override UniTask Exit()
        {
            _selectedTile = null;
            return default;
        }

        public override void OnHandleMoved(Vector2 handlePosition)
        {
            if (CheckTileIntersection(handlePosition, out TileRepresentation tile) && tile.IsEmpty == false )
            {
                if(_selectedTile != tile)
                {
                    SelectFrame.Select(tile);
                    _markersVisibility.SetSelectFrameShowed(true);
                    tile.ShakeBuilding();
                    ChangeSelectedTile(tile);
                }
            }
            else
            {
                SelectFrame.Hide();
                ChangeSelectedTile(null);
            }
        }

        public override void OnPressed(Vector2 handlePosition)
        {
            if (CheckTileIntersection(handlePosition, out TileRepresentation tile) && tile.IsEmpty == false)
            {
                SelectFrame.Hide();

                GameplayMover.RemoveBuilding(tile.GridPosition);
            }
        }

        private void ChangeSelectedTile(TileRepresentation tile)
        {
            _selectedTile?.StopBuildingShaking();
            _selectedTile = tile;
            _selectedTile?.ShakeBuilding();
        }
    }
}
