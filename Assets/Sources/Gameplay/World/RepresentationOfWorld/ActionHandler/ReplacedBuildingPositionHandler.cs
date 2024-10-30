using Assets.Sources.Gameplay.GameplayMover;
using Assets.Sources.Gameplay.World.RepresentationOfWorld.Markers;
using Assets.Sources.Gameplay.World.RepresentationOfWorld.Tiles;
using Assets.Sources.Infrastructure.Factories.WorldFactory;
using Cysharp.Threading.Tasks;
using System;
using UnityEngine;

namespace Assets.Sources.Gameplay.World.RepresentationOfWorld.ActionHandler
{
    public class ReplacedBuildingPositionHandler : ActionHandlerState
    {
        private readonly SelectFrame _choosedBuildingSelectFrame;
        private readonly SelectFrame.Factory _selectFrameFactory;
        private readonly MarkersVisibility _markersVisibility;
        private readonly Transform _selectFrameParent;

        private readonly Vector3 _choosedBuildingPositionOffset = new (0, 2, 0);

        private SelectFrame _choosedPlaceSelectFrame;
        private bool _isBuildingChoosed;
        private TileRepresentation _choosedToReplacingTile;

        public ReplacedBuildingPositionHandler(
            SelectFrame selectFrame,
            LayerMask layerMask,
            SelectFrame.Factory selectFrameFactory,
            IGameplayMover gameplayMover,
            MarkersVisibility markersVisibility,
            WorldGenerator worldGenerator)
            : base(selectFrame, layerMask, gameplayMover)
        {
            _selectFrameFactory = selectFrameFactory;
            _choosedBuildingSelectFrame = SelectFrame;
            _markersVisibility = markersVisibility;
            _selectFrameParent = worldGenerator.transform;
        }

        public event Action Entered;
        public event Action Exited;

        public override async UniTask Enter()
        {
            Entered?.Invoke();

            if (_choosedPlaceSelectFrame == null)
                _choosedPlaceSelectFrame = await _selectFrameFactory.Create(WorldFactoryAssets.SelectFrame, _selectFrameParent);

            _markersVisibility.SetSelectFrameShowed(false);
            _choosedPlaceSelectFrame.Hide();
        }

        public override UniTask Exit()
        {
            if (_choosedToReplacingTile != null)
                _choosedToReplacingTile.LowerBuilding();

            _markersVisibility.SetSelectFrameShowed(false);
            _choosedPlaceSelectFrame.Hide();

            _isBuildingChoosed = false;

            Exited?.Invoke();

            return default;
        }

        public override void OnHandleMoved(Vector2 handlePosition)
        {
            if (CheckTileIntersection(handlePosition, out TileRepresentation tile))
            {
                if (_isBuildingChoosed
                    && _choosedToReplacingTile != tile
                    && CheckBuildingAndTileCompatibility(_choosedToReplacingTile.BuildingType, tile.Type))
                {
                    _choosedPlaceSelectFrame.Select(tile);
                    _choosedPlaceSelectFrame.Show();
                }
                else if (_isBuildingChoosed == false && tile.IsEmpty == false)
                {
                    _choosedBuildingSelectFrame.Select(tile);
                    _markersVisibility.SetSelectFrameShowed(true);
                }
                else
                {
                    if (_isBuildingChoosed)
                        _choosedPlaceSelectFrame.Hide();
                    else
                        _markersVisibility.SetSelectFrameShowed(false);
                }
            }
            else if (_isBuildingChoosed)
            {
                _choosedPlaceSelectFrame.Hide();
            }
            else
            {
                _markersVisibility.SetSelectFrameShowed(false);
            }
        }

        public override void OnPressed(Vector2 handlePosition)
        {
            if (CheckTileIntersection(handlePosition, out TileRepresentation tile))
            {
                if (_isBuildingChoosed)
                {
                    if (_choosedToReplacingTile == tile)
                    {
                        _choosedToReplacingTile.LowerBuilding();
                        _choosedToReplacingTile = null;
                        _markersVisibility.SetSelectFrameShowed(false);
                        _isBuildingChoosed = false;
                    }
                    else if (CheckBuildingAndTileCompatibility(_choosedToReplacingTile.BuildingType, tile.Type))
                    {
                        _markersVisibility.SetSelectFrameShowed(false);
                        _choosedPlaceSelectFrame.Hide();

                        GameplayMover.ReplaceBuilding(_choosedToReplacingTile.GridPosition, _choosedToReplacingTile.BuildingType, tile.GridPosition, tile.BuildingType);
                    }
                }
                else if (tile.IsEmpty == false)
                {
                    _choosedToReplacingTile = tile;
                    _choosedBuildingSelectFrame.Select(_choosedToReplacingTile);
                    _markersVisibility.SetSelectFrameShowed(true);
                    _choosedToReplacingTile.RaiseBuilding(_choosedBuildingPositionOffset);
                    _isBuildingChoosed = true;
                }
            }
        }
    }
}
