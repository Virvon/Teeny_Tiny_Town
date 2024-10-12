using Assets.Sources.Gameplay.World.RepresentationOfWorld.ActionHandler;
using Assets.Sources.Infrastructure.Factories.UiFactory;
using Assets.Sources.Services.Input;
using Assets.Sources.Services.StateMachine;
using Assets.Sources.Services.StaticDataService.Configs.Windows;
using Assets.Sources.UI.Windows;
using Cysharp.Threading.Tasks;

namespace Assets.Sources.Gameplay.World.StateMachine.States
{
    public class WorldChangingState : IState
    {
        private readonly IInputService _inputService;
        private readonly WindowsSwitcher _windowsSwitcher;
        private readonly ActionHandlerStateMachine _actionHandlerStateMachine;
        private readonly IUiFactory _uiFactory;

        public WorldChangingState(
            IInputService inputService,
            WindowsSwitcher windowsSwitcher,
            ActionHandlerStateMachine actionHandlerStateMachine,
            IUiFactory uiFactory)
        {
            _inputService = inputService;
            _windowsSwitcher = windowsSwitcher;
            _actionHandlerStateMachine = actionHandlerStateMachine;
            _uiFactory = uiFactory;
        }

        public async UniTask Enter()
        {
            if(_windowsSwitcher.Contains(WindowType.GameplayWindow) == false)
            {
                Window window = await _uiFactory.CreateWindow(WindowType.GameplayWindow);
                _windowsSwitcher.RegisterWindow(WindowType.GameplayWindow, window);
            }

            _windowsSwitcher.Switch(WindowType.GameplayWindow);

            _actionHandlerStateMachine.SetActive(true);
            _inputService.SetEnabled(true);
        }

        public UniTask Exit()
        {
            _actionHandlerStateMachine.SetActive(false);
            _inputService.SetEnabled(false);
            return default;
        }
    }
}
