using Assets.Sources.Gameplay.World.RepresentationOfWorld.ActionHandler;
using Assets.Sources.Gameplay.World.RepresentationOfWorld.Markers;
using Assets.Sources.Gameplay.World.RepresentationOfWorld.Tiles;
using Assets.Sources.Services.StaticDataService.Configs.Building;
using Cysharp.Threading.Tasks;
using System;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Sources.Sandbox.ActionHandler
{
    public class BuildingPositionHandler : ActionHandlerState
    {
        private readonly SandboxChanger _sandboxChanger;
        private bool _isPressed;
        private BuildingType _buildingType;
        private TileRepresentation _placedTile;

        public BuildingPositionHandler(SelectFrame selectFrame, LayerMask layerMask, SandboxChanger sandboxChanger)
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

        public void SetBuilding(BuildingType type) =>
            _buildingType = type;

        public async override void OnHandleMoved(Vector2 handlePosition)
        {
            if (_isPressed)
                await TryCreateBuilding(handlePosition);
        }

        public override void OnPressed(Vector2 handlePosition)
        {
            SelectFrame.Hide();
            _isPressed = false;
            _placedTile = null;
        }

        public override async void OnHandlePressedMoveStarted(Vector2 handlePosition)
        {
            await TryCreateBuilding(handlePosition);

            _isPressed = true;
        }

        private async UniTask TryCreateBuilding(Vector2 handlePosition)
        {
            if (CheckTileIntersection(handlePosition, out TileRepresentation tile) && tile != _placedTile && tile.IsEmpty)
            {
                SelectFrame.Select(tile);
                SelectFrame.Show();
                _placedTile = tile;
                await _sandboxChanger.PutBuilding(tile.GridPosition, _buildingType);
            }
        }
    }
}
