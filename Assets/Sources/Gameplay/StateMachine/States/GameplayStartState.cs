using Assets.Sources.Infrastructure.Factories.UiFactory;
using Assets.Sources.Services.StateMachine;
using Assets.Sources.Services.StaticDataService.Configs.Windows;
using Assets.Sources.UI.Windows;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Assets.Sources.Gameplay.StateMachine.States
{
    public class GameplayStartState : IState
    {
        private readonly WindowsSwitcher _windowsSwitcher;
        private readonly IUiFactory _uiFactory;

        public GameplayStartState(WindowsSwitcher windowsSwitcher, IUiFactory uiFactory)
        {
            _windowsSwitcher = windowsSwitcher;
            _uiFactory = uiFactory;
        }

        public async UniTask Enter()
        {
            if(_windowsSwitcher.Contains(WindowType.StartWindow) == false)
            {
                Window window = await _uiFactory.CreateWindow(WindowType.StartWindow);
                _windowsSwitcher.RegisterWindow(WindowType.StartWindow, window);
            }

            _windowsSwitcher.Switch(WindowType.StartWindow);
        }

        public UniTask Exit()
        {
            return default;
        }
    }
}
