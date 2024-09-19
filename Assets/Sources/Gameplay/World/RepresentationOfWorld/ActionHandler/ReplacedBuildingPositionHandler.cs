using Assets.Sources.Gameplay.World.WorldInfrastructure;
using Assets.Sources.Infrastructure.GameplayFactory;
using Cysharp.Threading.Tasks;
using System;
using UnityEngine;
using Tile = Assets.Sources.Gameplay.World.RepresentationOfWorld.Tiles.Tile;

namespace Assets.Sources.Gameplay.World.RepresentationOfWorld.ActionHandler
{
    public class ReplacedBuildingPositionHandler : ActionHandlerState
    {
        private readonly SelectFrame _choosedBuildingSelectFrame;
        private readonly SelectFrame.Factory _selectFrameFactory;

        private readonly Vector3 _choosedBuildingPositionOffset = new Vector3(0, 2, 0);

        private SelectFrame _choosedPlaceSelectFrame;
        private bool _isBuildingChoosed;
        private Tile _choosedToReplacingTile;

        public ReplacedBuildingPositionHandler(SelectFrame selectFrame, LayerMask layerMask, SelectFrame.Factory selectFrameFactory) : base(selectFrame, layerMask)
        {
            _selectFrameFactory = selectFrameFactory;
            _choosedBuildingSelectFrame = SelectFrame;
        }

        public event Action<Vector2Int, BuildingType, Vector2Int, BuildingType> Replaced;

        public override async UniTask Enter()
        {
            if(_choosedPlaceSelectFrame == null)
                _choosedPlaceSelectFrame = await _selectFrameFactory.Create(GameplayFactoryAssets.SelectFrame);

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
            if (CheckTileIntersection(handlePosition, out Tile tile))
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
            if (CheckTileIntersection(handlePosition, out Tile tile))
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
                        Tiles.Building choosedToRaplaceBuilding = _choosedToReplacingTile.TakeBuilding();
                        Tiles.Building targetPlacedBuilding = tile.TakeBuilding();

                        tile.PlaceBuilding(choosedToRaplaceBuilding);

                        if (targetPlacedBuilding != null)
                            _choosedToReplacingTile.PlaceBuilding(targetPlacedBuilding);

                        _choosedBuildingSelectFrame.Hide();
                        _choosedPlaceSelectFrame.Hide();

                        Replaced?.Invoke(_choosedToReplacingTile.GridPosition, _choosedToReplacingTile.BuildingType, tile.GridPosition, tile.BuildingType);
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
