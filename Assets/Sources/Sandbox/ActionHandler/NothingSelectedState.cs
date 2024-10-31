using Assets.Sources.Gameplay.World.RepresentationOfWorld.ActionHandler;
using Assets.Sources.Gameplay.World.RepresentationOfWorld.Markers;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Assets.Sources.Sandbox.ActionHandler
{
    public class NothingSelectedState : ActionHandlerState
    {
        private readonly ActionHandlerStateMachine _actionHandlerStateMachine;

        public NothingSelectedState(SelectFrame selectFrame, LayerMask layerMask, ActionHandlerStateMachine actionHandlerStateMachine)
            : base(selectFrame, layerMask)
        {
            _actionHandlerStateMachine = actionHandlerStateMachine;
        }

        public override UniTask Enter()
        {
            _actionHandlerStateMachine.SetActive(false);
            return default;
        }

        public override UniTask Exit()
        {
            _actionHandlerStateMachine.SetActive(true);
            return default;
        }

        public override void OnHandleMoved(Vector2 handlePosition)
        {
        }

        public override void OnPressed(Vector2 handlePosition)
        {
        }
    }
}
