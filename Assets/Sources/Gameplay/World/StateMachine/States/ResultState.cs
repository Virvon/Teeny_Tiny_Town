using Assets.Sources.Infrastructure.Factories.UiFactory;
using Assets.Sources.Services.StateMachine;
using Assets.Sources.Services.StaticDataService.Configs.Windows;
using Assets.Sources.UI.Windows;
using Cysharp.Threading.Tasks;
using System;

namespace Assets.Sources.Gameplay.World.StateMachine.States
{
    public class ResultState : IState
    {
        private readonly WindowsSwitcher _windowsSwitcher;
        private readonly IUiFactory _uiFactory;
        private readonly World _world;

        public ResultState(WindowsSwitcher windowsSwitcher, IUiFactory uiFactory, World world)
        {
            _windowsSwitcher = windowsSwitcher;
            _uiFactory = uiFactory;
            _world = world;
        }

        public async UniTask Enter()
        {
            if (_windowsSwitcher.Contains(WindowType.ResultWindow) == false)
            {
                Window window = await _uiFactory.CreateWindow(WindowType.ResultWindow);
                _windowsSwitcher.RegisterWindow(WindowType.ResultWindow, window);
            }

            _windowsSwitcher.Switch(WindowType.ResultWindow);
            _world.StartRotating();
        }

        public UniTask Exit()
        {
            throw new NotImplementedException();
        }
    }
}
