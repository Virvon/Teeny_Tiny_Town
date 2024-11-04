using Assets.Sources.Gameplay.World.RepresentationOfWorld.ActionHandler;
using Assets.Sources.Gameplay.World.RepresentationOfWorld.Markers;
using Assets.Sources.Gameplay.World.RepresentationOfWorld.Tiles;
using Cysharp.Threading.Tasks;
using System;
using System.Threading.Tasks;
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

        public event Action Entered;
        public event Action Exited;

        public override UniTask Enter()
        {
            _isPressed = false;
            Entered?.Invoke();
            return default;
        }

        public override UniTask Exit()
        {
            Exited?.Invoke();
            return default;
        }

        public override async void OnHandleMoved(Vector2 handlePosition)
        {
            if (_isPressed)
                await ClearTile(handlePosition);
        }

        public override void OnPressed(Vector2 handlePosition)
        {
            SelectFrame.Hide();
            _isPressed = false;
            _clearedTile = null;
        }

        public override async void OnHandlePressedMoveStarted(Vector2 handlePosition)
        {
            await ClearTile(handlePosition);

            _isPressed = true;
        }

        private async UniTask ClearTile(Vector2 handlePosition)
        {
            if (CheckTileIntersection(handlePosition, out TileRepresentation tile) && tile != _clearedTile)
            {
                SelectFrame.Select(tile);
                SelectFrame.Show();
                _clearedTile = tile;
                await _sandboxChanger.ClearTile(tile.GridPosition);
            }
        }
    }
}
