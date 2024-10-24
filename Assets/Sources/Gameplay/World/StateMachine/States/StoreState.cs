using Assets.Sources.Infrastructure.Factories.UiFactory;
using Assets.Sources.Services.StateMachine;
using Assets.Sources.Services.StaticDataService.Configs.Windows;
using Assets.Sources.UI;
using Cysharp.Threading.Tasks;

namespace Assets.Sources.Gameplay.World.StateMachine.States
{
    public class StoreState : IState
    {
        private readonly WindowsSwitcher _windowsSwitcher;
        private readonly IUiFactory _uiFactory;

        public StoreState(WindowsSwitcher windowsSwitcher, IUiFactory uiFactory)
        {
            _windowsSwitcher = windowsSwitcher;
            _uiFactory = uiFactory;
        }

        public async UniTask Enter()
        {
            if (_windowsSwitcher.Contains(WindowType.StoreWindow) == false)
            {
                Window window = await _uiFactory.CreateWindow(WindowType.StoreWindow);
                _windowsSwitcher.RegisterWindow(WindowType.StoreWindow, window);
            }

            _windowsSwitcher.Switch(WindowType.StoreWindow);
        }

        public UniTask Exit()
        {
            return default;
        }
    }
}
