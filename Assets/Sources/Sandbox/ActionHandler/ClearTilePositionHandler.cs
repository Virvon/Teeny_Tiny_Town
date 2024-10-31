using Assets.Sources.Gameplay.World.RepresentationOfWorld.ActionHandler;
using Assets.Sources.Gameplay.World.RepresentationOfWorld.Markers;
using Assets.Sources.Gameplay.World.RepresentationOfWorld.Tiles;
using Cysharp.Threading.Tasks;
using System;
using UnityEngine;

namespace Assets.Sources.Sandbox.ActionHandler
{
    public class ClearTilePositionHandler : ActionHandlerState
    {
        private readonly SandboxChanger _sandboxChanger;

        bool _isPressed;
        public TileRepresentation _clearedTile;

        public ClearTilePositionHandler(SelectFrame selectFrame, LayerMask layerMask, SandboxChanger sandboxChanger)
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

        public override async void OnHandleMoved(Vector2 handlePosition)
        {
            if (CheckTileIntersection(handlePosition, out TileRepresentation tile) && _isPressed && tile != _clearedTile)
            {
                SelectFrame.Select(tile);
                SelectFrame.Show();
                _clearedTile = tile;
                await _sandboxChanger.ClearTile(tile.GridPosition);
            }
        }

        public override void OnPressed(Vector2 handlePosition)
        {
            SelectFrame.Hide();
            _isPressed = false;
            _clearedTile = null;
        }

        public override async void OnHandlePressedMoveStarted(Vector2 handlePosition)
        {
            if (CheckTileIntersection(handlePosition, out TileRepresentation tile) && tile != _clearedTile)
            {
                SelectFrame.Select(tile);
                SelectFrame.Show();
                _clearedTile = tile;
                await _sandboxChanger.ClearTile(tile.GridPosition);
            }

            _isPressed = true;
        }
    }
}
