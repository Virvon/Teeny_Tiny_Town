using System;
using Assets.Sources.Gameplay.World.RepresentationOfWorld.ActionHandler;
using Assets.Sources.Services.Input;

namespace Assets.Sources.Sandbox.ActionHandler
{
    public class SandboxActionHandlerSwitcher : IDisposable
    {
        private readonly ActionHandlerStateMachine _handlerStateMachine;
        private readonly IInputService _inputService;

        public SandboxActionHandlerSwitcher(
            ActionHandlerStateMachine handlerStateMachine,
            IInputService inputService)
        {
            _handlerStateMachine = handlerStateMachine;
            _inputService = inputService;

            _inputService.ClearTilesButtonPressed += OnClearTilesButtonPressed;
            _inputService.BuildingsButtonPressed += OnBuildingsButtonPressed;
            _inputService.GroundsButtonPressed += OnGroundsButtonPressed;
        }

        public void Dispose()
        {
            _inputService.ClearTilesButtonPressed -= OnClearTilesButtonPressed;
            _inputService.BuildingsButtonPressed -= OnBuildingsButtonPressed;
            _inputService.GroundsButtonPressed -= OnGroundsButtonPressed;
        }

        public void EnterToDefaultState()
        {
            if (_handlerStateMachine.CurrentState is not NothingSelectedState)
                _handlerStateMachine.Enter<NothingSelectedState>();
        }

        private void OnGroundsButtonPressed()
        {
            if (_handlerStateMachine.CurrentState is not GroundPositionHandler)
                _handlerStateMachine.Enter<GroundPositionHandler>();
            else
                EnterToDefaultState();
        }

        private void OnBuildingsButtonPressed()
        {
            if (_handlerStateMachine.CurrentState is not BuildingPositionHandler)
                _handlerStateMachine.Enter<BuildingPositionHandler>();
            else
                EnterToDefaultState();
        }

        private void OnClearTilesButtonPressed()
        {
            if (_handlerStateMachine.CurrentState is not ClearTilePositionHandler)
                _handlerStateMachine.Enter<ClearTilePositionHandler>();
            else
                EnterToDefaultState();
        }
    }
}
