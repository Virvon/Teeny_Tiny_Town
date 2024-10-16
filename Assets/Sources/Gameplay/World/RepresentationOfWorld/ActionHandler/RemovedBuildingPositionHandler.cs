﻿using Assets.Sources.Gameplay.GameplayMover;
using Assets.Sources.Gameplay.World.RepresentationOfWorld.Tiles;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Assets.Sources.Gameplay.World.RepresentationOfWorld.ActionHandler
{
    public class RemovedBuildingPositionHandler : ActionHandlerState
    {
        public RemovedBuildingPositionHandler(
            SelectFrame selectFrame,
            LayerMask layerMask,
            IGameplayMover gameplayMover)
            : base(selectFrame, layerMask, gameplayMover)
        {
        }

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

                GameplayMover.RemoveBuilding(tile.GridPosition);
            }
        }
    }
}
