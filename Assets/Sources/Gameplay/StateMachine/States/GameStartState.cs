using Assets.Sources.Gameplay.Cameras;
using Assets.Sources.Infrastructure.Factories.UiFactory;
using Assets.Sources.Services.StateMachine;
using Assets.Sources.Services.StaticDataService.Configs.Windows;
using Assets.Sources.UI;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Assets.Sources.Gameplay.StateMachine.States
{
    public class GameStartState : IState
    {
        private readonly WindowsSwitcher _windowsSwitcher;
        private readonly IUiFactory _uiFactory;
        private readonly GameplayCamera _camera;

        public GameStartState(WindowsSwitcher windowsSwitcher, IUiFactory uiFactory, GameplayCamera camera)
        {
            _windowsSwitcher = windowsSwitcher;
            _uiFactory = uiFactory;
            _camera = camera;
        }

        public async UniTask Enter()
        {
            if(_windowsSwitcher.Contains(WindowType.StartWindow) == false)
            {
                Window window = await _uiFactory.CreateWindow(WindowType.StartWindow);
                _windowsSwitcher.RegisterWindow(WindowType.StartWindow, window);
            }

            _windowsSwitcher.Switch(WindowType.StartWindow);
            _camera.MoveTo(new Vector3(67.3f, 93.1f, -67.3f));
        }

        public UniTask Exit()
        {
            return default;
        }
    }
}
