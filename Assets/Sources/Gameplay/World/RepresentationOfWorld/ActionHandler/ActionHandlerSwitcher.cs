using Assets.Sources.UI;
using UnityEngine;
using Zenject;

namespace Assets.Sources.Gameplay.World.RepresentationOfWorld.ActionHandler
{
    public class ActionHandlerSwitcher : MonoBehaviour
    {
        private WorldRepresentationChanger _worldRepresentationChanger;

        private ActionHandlerStateMachine _handlerStateMachine;
        private DestroyBuildingButton _destroyBuildingButton;
        private ReplaceButton _replaceButton;

        [Inject]
        private void Construct(ActionHandlerStateMachine handlerStateMachine, DestroyBuildingButton destroyBuildingButton, ReplaceButton replaceButton, WorldRepresentationChanger worldRepresentationChanger)
        {
            _handlerStateMachine = handlerStateMachine;
            _destroyBuildingButton = destroyBuildingButton;
            _replaceButton = replaceButton;
            _worldRepresentationChanger = worldRepresentationChanger;

            _destroyBuildingButton.Clicked += OnDestroyButtonClicked;
            _replaceButton.Clicked += OnReplaceButtonClicked;
            _worldRepresentationChanger.GameplayMoved += OnGameplayMoved;
        }

        private void OnDestroy()
        {
            _destroyBuildingButton.Clicked -= OnDestroyButtonClicked;
            _replaceButton.Clicked -= OnReplaceButtonClicked;
            _worldRepresentationChanger.GameplayMoved -= OnGameplayMoved;
        }

        private void OnReplaceButtonClicked()
        {
            if (_handlerStateMachine.CurrentState is not ReplacedBuildingPositionHandler)
                _handlerStateMachine.Enter<ReplacedBuildingPositionHandler>();
            else
                _handlerStateMachine.Enter<NewBuildingPlacePositionHandler>();
        }

        private void OnDestroyButtonClicked()
        {
            if (_handlerStateMachine.CurrentState is not RemovedBuildingPositionHandler)
                _handlerStateMachine.Enter<RemovedBuildingPositionHandler>();
            else
                _handlerStateMachine.Enter<NewBuildingPlacePositionHandler>();
        }

        private void OnGameplayMoved()
        {
            if (_handlerStateMachine.CurrentState is not NewBuildingPlacePositionHandler)
                _handlerStateMachine.Enter<NewBuildingPlacePositionHandler>();
        }
    }
}
