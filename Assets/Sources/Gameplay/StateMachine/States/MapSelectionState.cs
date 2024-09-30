using Assets.Sources.Infrastructure.Factories.UiFactory;
using Assets.Sources.Services.StateMachine;
using Assets.Sources.Services.StaticDataService.Configs.Windows;
using Assets.Sources.UI.Windows;
using Cysharp.Threading.Tasks;

namespace Assets.Sources.Gameplay.StateMachine.States
{
    public class MapSelectionState : IState
    {
        private readonly WindowsSwitcher _windowsSwitcher;
        private readonly IUiFactory _uiFactory;

        public MapSelectionState(WindowsSwitcher windowsSwitcher, IUiFactory uiFactory)
        {
            _windowsSwitcher = windowsSwitcher;
            _uiFactory = uiFactory;
        }

        public async UniTask Enter()
        {
            if(_windowsSwitcher.Contains(WindowType.MapSelectionWindow) == false)
            {
                Window window = await _uiFactory.CreateWindow(WindowType.MapSelectionWindow);
                _windowsSwitcher.RegisterWindow(WindowType.MapSelectionWindow, window);
            }

            _windowsSwitcher.Switch(WindowType.MapSelectionWindow);
        }

        public UniTask Exit()
        {
            return default;
        }
    }
}
