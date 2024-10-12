using Assets.Sources.Gameplay.Cameras;
using Assets.Sources.Infrastructure.Factories.UiFactory;
using Assets.Sources.Services.StateMachine;
using Assets.Sources.Services.StaticDataService.Configs.Camera;
using Assets.Sources.Services.StaticDataService.Configs.Windows;
using Assets.Sources.UI.Windows;
using Cysharp.Threading.Tasks;

namespace Assets.Sources.Gameplay.StateMachine.States
{
    public class MapSelectionState : IState
    {
        private readonly WindowsSwitcher _windowsSwitcher;
        private readonly IUiFactory _uiFactory;
        private readonly CamerasSwitcher _camerasSwtitcher;

        public MapSelectionState(WindowsSwitcher windowsSwitcher, IUiFactory uiFactory, CamerasSwitcher camerasSwtitcher)
        {
            _windowsSwitcher = windowsSwitcher;
            _uiFactory = uiFactory;
            _camerasSwtitcher = camerasSwtitcher;
        }

        public async UniTask Enter()
        {
            if(_windowsSwitcher.Contains(WindowType.MapSelectionWindow) == false)
            {
                Window window = await _uiFactory.CreateWindow(WindowType.MapSelectionWindow);
                _windowsSwitcher.RegisterWindow(WindowType.MapSelectionWindow, window);
            }

            _windowsSwitcher.Switch(WindowType.MapSelectionWindow);
            _camerasSwtitcher.Switch(GameplayCameraType.MapSelectionCamera);
        }

        public UniTask Exit()
        {
            return default;
        }
    }
}
