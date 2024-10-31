using Assets.Sources.Gameplay.World.RepresentationOfWorld.ActionHandler;
using Assets.Sources.Gameplay.World.RepresentationOfWorld.Markers;
using Assets.Sources.Gameplay.World.RepresentationOfWorld.Tiles;
using Assets.Sources.Services.StaticDataService.Configs.World;
using Cysharp.Threading.Tasks;
using System;
using UnityEngine;

namespace Assets.Sources.Sandbox.ActionHandler
{
    public class GroundPositionHandler : ActionHandlerState
    {
        private readonly SandboxChanger _sandboxChanger;

        private bool _isPressed;
        private SandboxGroundType _groundType;
        public TileRepresentation _placedTile;

        public GroundPositionHandler(SelectFrame selectFrame, LayerMask layerMask, SandboxChanger sandboxChanger)
            : base(selectFrame, layerMask)
        {
            _sandboxChanger = sandboxChanger;
        }

        public override UniTask Enter()
        {
            _isPressed = false;
            return default;
        }

        public override UniTask Exit()
        {
            return default;
        }

        public void SetGround(SandboxGroundType type) =>
            _groundType = type;

        public override async void OnHandleMoved(Vector2 handlePosition)
        {
            if (CheckTileIntersection(handlePosition, out TileRepresentation tile) && _isPressed && tile != _placedTile)
            {
                SelectFrame.Select(tile);
                SelectFrame.Show();
                _placedTile = tile;
                await _sandboxChanger.PutGround(tile.GridPosition, _groundType);
            }
        }

        public override void OnPressed(Vector2 handlePosition)
        {
            SelectFrame.Hide();
            _isPressed = false;
            _placedTile = null;
        }

        public override async void OnHandlePressedMoveStarted(Vector2 handlePosition)
        {
            if(CheckTileIntersection(handlePosition, out TileRepresentation tile) && tile != _placedTile)
            {
                SelectFrame.Select(tile);
                SelectFrame.Show();
                _placedTile = tile;
                await _sandboxChanger.PutGround(tile.GridPosition, _groundType);
            }

            _isPressed = true;
        }
    }
}
