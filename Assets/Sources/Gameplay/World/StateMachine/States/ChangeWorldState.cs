using Assets.Sources.Gameplay.World.RepresentationOfWorld.ActionHandler;
using Assets.Sources.Gameplay.World.WorldInfrastructure;
using Assets.Sources.Infrastructure.Factories.UiFactory;
using Assets.Sources.Infrastructure.Factories.WorldFactory;
using Assets.Sources.Services.Input;
using Assets.Sources.Services.StateMachine;
using Assets.Sources.Services.StaticDataService.Configs.Windows;
using Assets.Sources.UI.Windows;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Assets.Sources.Gameplay.World.StateMachine.States
{
    public class ChangeWorldState : IState
    {
        private readonly IInputService _inputService;
        private readonly WindowsSwitcher _windowsSwitcher;
        private readonly ActionHandlerStateMachine _actionHandlerStateMachine;
        private readonly IUiFactory _uiFactory;
        private readonly ActionHandlerStatesFactory _actionHandlerStatesFactory;
        private readonly IWorldFactory _worldFactory;
        private readonly WorldChanger _worldChanger;

        public ChangeWorldState(
            IInputService inputService,
            WindowsSwitcher windowsSwitcher,
            ActionHandlerStateMachine actionHandlerStateMachine,
            IUiFactory uiFactory,
            ActionHandlerStatesFactory actionHandlerStatesFactory,
            IWorldFactory worldFactory,
            WorldChanger worldChanger)
        {
            _inputService = inputService;
            _windowsSwitcher = windowsSwitcher;
            _actionHandlerStateMachine = actionHandlerStateMachine;
            _uiFactory = uiFactory;
            _actionHandlerStatesFactory = actionHandlerStatesFactory;
            _worldFactory = worldFactory;
            _worldChanger = worldChanger;
        }

        public async UniTask Enter()
        {
            if(_windowsSwitcher.Contains(WindowType.GameplayWindow) == false)
            {
                Window window = await _uiFactory.CreateWindow(WindowType.GameplayWindow);
                _windowsSwitcher.RegisterWindow(WindowType.GameplayWindow, window);
            }

            _windowsSwitcher.Switch(WindowType.GameplayWindow);

            await _worldFactory.CreateSelectFrame();
            await _worldFactory.CreateBuildingMarker();
            await _worldFactory.CreateActionHandlerSwitcher();

            RegisterActionHandlerStates();

            _actionHandlerStateMachine.Enter<NewBuildingPlacePositionHandler>();

            _actionHandlerStateMachine.SetActive(true);
            _inputService.SetEnabled(true);

            _worldChanger.Start();
        }

        public UniTask Exit()
        {
            _actionHandlerStateMachine.SetActive(false);
            _inputService.SetEnabled(false);
            return default;
        }

        private void RegisterActionHandlerStates()
        {
            _actionHandlerStateMachine.RegisterState(_actionHandlerStatesFactory.CreateHandlerState<NewBuildingPlacePositionHandler>());
            _actionHandlerStateMachine.RegisterState(_actionHandlerStatesFactory.CreateHandlerState<RemovedBuildingPositionHandler>());
            _actionHandlerStateMachine.RegisterState(_actionHandlerStatesFactory.CreateHandlerState<ReplacedBuildingPositionHandler>());
        }
    }
}
