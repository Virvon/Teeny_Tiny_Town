using Assets.Sources.Data.WorldDatas;
using Assets.Sources.Gameplay.Cameras;
using Assets.Sources.Gameplay.World.RepresentationOfWorld.ActionHandler;
using Assets.Sources.Infrastructure.Factories.UiFactory;
using Assets.Sources.Services.Input;
using Assets.Sources.Services.StateMachine;
using Assets.Sources.Services.StaticDataService.Configs.Windows;
using Assets.Sources.UI.Windows;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Assets.Sources.Gameplay.World.StateMachine.States
{
    public class WorldChangingState : IState
    {
        private readonly IInputService _inputService;
        private readonly WindowsSwitcher _windowsSwitcher;
        private readonly ActionHandlerStateMachine _actionHandlerStateMachine;
        private readonly IUiFactory _uiFactory;
        private readonly GameplayCamera _camera;

        public WorldChangingState(
            IInputService inputService,
            WindowsSwitcher windowsSwitcher,
            ActionHandlerStateMachine actionHandlerStateMachine,
            IUiFactory uiFactory,
            GameplayCamera gameplayCamera)
        {
            _inputService = inputService;
            _windowsSwitcher = windowsSwitcher;
            _actionHandlerStateMachine = actionHandlerStateMachine;
            _uiFactory = uiFactory;
            _camera = gameplayCamera;
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

            _camera.MoveTo(new Vector3(55.1f, 78.8f, -55.1f), callback: () =>
            {
                _actionHandlerStateMachine.SetActive(true);
                _inputService.SetEnabled(true);
            });
        }

        public UniTask Exit()
        {
            _actionHandlerStateMachine.SetActive(false);
            _inputService.SetEnabled(false);
            return default;
        }
    }
    public class StartWorldState : IState
    {
        private readonly IWorldData _worldData;
        private readonly WindowsSwitcher _windowsSwitcher;
        private readonly IUiFactory _uiFactory;
        private readonly WorldStateMachine _worldStateMachine;

        public StartWorldState(IWorldData worldData, WindowsSwitcher windowsSwitcher, IUiFactory uiFactory, WorldStateMachine worldStateMachine)
        {
            _worldData = worldData;
            _windowsSwitcher = windowsSwitcher;
            _uiFactory = uiFactory;
            _worldStateMachine = worldStateMachine;
        }

        public async UniTask Enter()
        {
            if (_worldData.IsChangingStarted)
                _worldStateMachine.Enter<WorldBootstrapState>().Forget();
            else
                await ShowAdditionalBonusOffer();
        }

        public UniTask Exit() =>
            default;

        private async UniTask ShowAdditionalBonusOffer()
        {
            if (_windowsSwitcher.Contains(WindowType.AdditionalBonusOfferWindow) == false)
            {
                Window window = await _uiFactory.CreateWindow(WindowType.AdditionalBonusOfferWindow);
                _windowsSwitcher.RegisterWindow(WindowType.AdditionalBonusOfferWindow, window);
            }

            _windowsSwitcher.Switch(WindowType.AdditionalBonusOfferWindow);
        }
    }
}
