using Assets.Sources.Services.Input;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace Assets.Sources.Gameplay.World.RepresentationOfWorld.ActionHandler
{
    public class ActionHandlerSwitcher : MonoBehaviour
    {
        private WorldRepresentationChanger _worldRepresentationChanger;

        private ActionHandlerStateMachine _handlerStateMachine;
        private IInputService _inputService;

        [Inject]
        private void Construct(
            ActionHandlerStateMachine handlerStateMachine,
            WorldRepresentationChanger worldRepresentationChanger,
            IInputService inputService)
        {
            _handlerStateMachine = handlerStateMachine;
            _worldRepresentationChanger = worldRepresentationChanger;
            _inputService = inputService;

            _inputService.RemoveBuildingButtonPressed += OnRemoveBuildingButtonClicked;
            _inputService.ReplaceBuildingButtonPressed += OnReplaceBuildingButtonClicked;
            _worldRepresentationChanger.GameplayMoved += EnterToDefaultState;
        }

        private void OnDestroy()
        {
            _inputService.RemoveBuildingButtonPressed -= OnRemoveBuildingButtonClicked;
            _inputService.ReplaceBuildingButtonPressed -= OnReplaceBuildingButtonClicked;
            _worldRepresentationChanger.GameplayMoved -= EnterToDefaultState;
        }

        public void EnterToDefaultState()
        {
            if (_handlerStateMachine.CurrentState is not NewBuildingPlacePositionHandler)
                _handlerStateMachine.Enter<NewBuildingPlacePositionHandler>();
        }

        private void OnReplaceBuildingButtonClicked()
        {
            if (_handlerStateMachine.CurrentState is not ReplacedBuildingPositionHandler)
                _handlerStateMachine.Enter<ReplacedBuildingPositionHandler>();
            else
                _handlerStateMachine.Enter<NewBuildingPlacePositionHandler>();
        }

        private void OnRemoveBuildingButtonClicked()
        {
            if (_handlerStateMachine.CurrentState is not RemovedBuildingPositionHandler)
                _handlerStateMachine.Enter<RemovedBuildingPositionHandler>();
            else
                _handlerStateMachine.Enter<NewBuildingPlacePositionHandler>();
        }

        public class Factory : PlaceholderFactory<string, UniTask<ActionHandlerSwitcher>>
        {
        }
    }
}
