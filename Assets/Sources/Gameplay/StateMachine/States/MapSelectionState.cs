using Assets.Sources.Gameplay.Cameras;
using Assets.Sources.Infrastructure.Factories.UiFactory;
using Assets.Sources.Services.StateMachine;
using Assets.Sources.Services.StaticDataService.Configs.Windows;
using Assets.Sources.UI.Windows;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Assets.Sources.Gameplay.StateMachine.States
{
    public class MapSelectionState : IState
    {
        private readonly WindowsSwitcher _windowsSwitcher;
        private readonly IUiFactory _uiFactory;
        private readonly GameplayCamera _camera;

        public MapSelectionState(WindowsSwitcher windowsSwitcher, IUiFactory uiFactory, GameplayCamera camera)
        {
            _windowsSwitcher = windowsSwitcher;
            _uiFactory = uiFactory;
            _camera = camera;
        }

        public async UniTask Enter()
        {
            if(_windowsSwitcher.Contains(WindowType.MapSelectionWindow) == false)
            {
                Window window = await _uiFactory.CreateWindow(WindowType.MapSelectionWindow);
                _windowsSwitcher.RegisterWindow(WindowType.MapSelectionWindow, window);
            }

            _windowsSwitcher.Switch(WindowType.MapSelectionWindow);
            _camera.MoveTo(new Vector3(60.9f, 93.1f, -60.9f));
        }

        public UniTask Exit()
        {
            return default;
        }
    }
}
