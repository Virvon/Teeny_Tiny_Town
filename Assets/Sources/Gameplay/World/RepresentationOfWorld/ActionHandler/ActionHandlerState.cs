using Assets.Sources.Gameplay.World.RepresentationOfWorld.Tiles;
using Assets.Sources.Services.StateMachine;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Assets.Sources.Gameplay.World.RepresentationOfWorld.ActionHandler
{
    public abstract class ActionHandlerState : IState
    {
        private const float RaycastDistance = 300;

        protected readonly SelectFrame SelectFrame;
        protected readonly GameplayMover.GameplayMover GameplayMover;

        private readonly LayerMask _layerMask;
        private readonly Camera _camera;

        public ActionHandlerState(SelectFrame selectFrame, LayerMask layerMask, GameplayMover.GameplayMover gameplayMover)
        {
            SelectFrame = selectFrame;
            _layerMask = layerMask;

            _camera = Camera.main;
            GameplayMover = gameplayMover;
        }

        public abstract UniTask Enter();

        public abstract UniTask Exit();

        public abstract void OnHandleMoved(Vector2 handlePosition);

        public abstract void OnPressed(Vector2 handlePosition);

        public virtual void OnHandlePressedMovePerformed(Vector2 handlePosition)
        {
        }

        public virtual void OnHandlePressedMoveStarted(Vector2 handlePosition)
        {
        }

        protected bool CheckTileIntersection(Vector2 handlePosition, out TileRepresentation tile)
        {
            tile = null;

            if (Physics.Raycast(GetRay(handlePosition), out RaycastHit hitInfo, RaycastDistance, _layerMask, QueryTriggerInteraction.Ignore)
                && hitInfo.transform.TryGetComponent(out GroundCollider groundCollider))
            {
                tile = groundCollider.Tile;

                return true;
            }

            return false;
        }

        protected Ray GetRay(Vector2 handlePosition) =>
            _camera.ScreenPointToRay(new Vector3(handlePosition.x, handlePosition.y, 1));
    }
}
