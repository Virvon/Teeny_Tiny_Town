using Assets.Sources.Gameplay.World.RepresentationOfWorld.ActionHandler;
using Assets.Sources.Gameplay.World.WorldInfrastructure.NextBuildingForPlacing;
using Assets.Sources.Infrastructure.Factories.UiFactory;
using Assets.Sources.Services.Input;
using Assets.Sources.Services.StateMachine;
using Assets.Sources.Services.StaticDataService.Configs.Windows;
using Assets.Sources.UI.Windows;
using Cysharp.Threading.Tasks;
using System.Threading.Tasks;

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
            IUiFactory uiFactory,
            NextBuildingForPlacingCreator nextBuildingForPlacingCreator)
        {
            _inputService = inputService;
            _windowsSwitcher = windowsSwitcher;
            _actionHandlerStateMachine = actionHandlerStateMachine;
            _uiFactory = uiFactory;
        }

        protected virtual WindowType WindowType => WindowType.GameplayWindow;

        public async UniTask Enter()
        {
            if (_windowsSwitcher.Contains(WindowType) == false)
            {
                Window window = await _uiFactory.CreateWindow(WindowType);
                _windowsSwitcher.RegisterWindow(WindowType, window);
            }

            _windowsSwitcher.Switch(WindowType);

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
