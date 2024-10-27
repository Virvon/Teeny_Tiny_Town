using Assets.Sources.Gameplay.GameplayMover;
using Assets.Sources.Gameplay.World.RepresentationOfWorld.Markers;
using Assets.Sources.Gameplay.World.RepresentationOfWorld.Tiles;
using Assets.Sources.Infrastructure.Factories.WorldFactory;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Assets.Sources.Gameplay.World.RepresentationOfWorld.ActionHandler
{
    public class ReplacedBuildingPositionHandler : ActionHandlerState
    {
        private readonly SelectFrame _choosedBuildingSelectFrame;
        private readonly SelectFrame.Factory _selectFrameFactory;

        private readonly Vector3 _choosedBuildingPositionOffset = new (0, 2, 0);

        private SelectFrame _choosedPlaceSelectFrame;
        private bool _isBuildingChoosed;
        private TileRepresentation _choosedToReplacingTile;

        public ReplacedBuildingPositionHandler(
            SelectFrame selectFrame,
            LayerMask layerMask,
            SelectFrame.Factory selectFrameFactory,
            IGameplayMover gameplayMover)
            : base(selectFrame, layerMask, gameplayMover)
        {
            _selectFrameFactory = selectFrameFactory;
            _choosedBuildingSelectFrame = SelectFrame;
        }

        public override async UniTask Enter()
        {
            if(_choosedPlaceSelectFrame == null)
                _choosedPlaceSelectFrame = await _selectFrameFactory.Create(WorldFactoryAssets.SelectFrame);

            _choosedBuildingSelectFrame.Hide();
            _choosedPlaceSelectFrame.Hide();
        }

        public override UniTask Exit()
        {
            _isBuildingChoosed = false;

            return default;
        }

        public override void OnHandleMoved(Vector2 handlePosition)
        {
            if (CheckTileIntersection(handlePosition, out TileRepresentation tile))
            {
                if (_isBuildingChoosed && _choosedToReplacingTile != tile)
                {
                    _choosedPlaceSelectFrame.Select(tile);
                }
                else if (_isBuildingChoosed == false && tile.IsEmpty == false)
                {
                    _choosedBuildingSelectFrame.Select(tile);
                }
                else
                {
                    if (_isBuildingChoosed)
                        _choosedPlaceSelectFrame.Hide();
                    else
                        _choosedBuildingSelectFrame.Hide();
                }
            }
            else if (_isBuildingChoosed)
            {
                _choosedPlaceSelectFrame.Hide();
            }
            else
            {
                _choosedBuildingSelectFrame.Hide();
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
                        _choosedBuildingSelectFrame.Hide();
                        _isBuildingChoosed = false;
                    }
                    else
                    {
                        _choosedBuildingSelectFrame.Hide();
                        _choosedPlaceSelectFrame.Hide();

                        GameplayMover.ReplaceBuilding(_choosedToReplacingTile.GridPosition, _choosedToReplacingTile.BuildingType, tile.GridPosition, tile.BuildingType);
                    }
                }
                else if (tile.IsEmpty == false)
                {
                    _choosedToReplacingTile = tile;
                    _choosedBuildingSelectFrame.Select(_choosedToReplacingTile);
                    _choosedToReplacingTile.RaiseBuilding(_choosedBuildingPositionOffset);
                    _isBuildingChoosed = true;
                }
            }
        }
    }
}
