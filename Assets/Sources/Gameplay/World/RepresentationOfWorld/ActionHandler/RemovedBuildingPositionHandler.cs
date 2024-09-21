using Assets.Sources.Gameplay.World.RepresentationOfWorld.Tiles;
using Cysharp.Threading.Tasks;
using System;
using UnityEngine;

namespace Assets.Sources.Gameplay.World.RepresentationOfWorld.ActionHandler
{
    public class RemovedBuildingPositionHandler : ActionHandlerState
    {
        public RemovedBuildingPositionHandler(SelectFrame selectFrame, LayerMask layerMask) : base(selectFrame, layerMask)
        {
        }

        public event Action<Vector2Int> Removed;

        public override UniTask Enter() =>
            default;

        public override UniTask Exit() =>
            default;

        public override void OnHandleMoved(Vector2 handlePosition)
        {
            if (CheckTileIntersection(handlePosition, out TileRepresentation tile) && tile.IsEmpty == false)
                SelectFrame.Select(tile);
            else
                SelectFrame.Hide();
        }

        public override void OnPressed(Vector2 handlePosition)
        {
            if (CheckTileIntersection(handlePosition, out TileRepresentation tile) && tile.IsEmpty == false)
            {
                SelectFrame.Hide();

                Removed?.Invoke(tile.GridPosition);
            }
        }
    }
}
