using Assets.Sources.Data.WorldDatas;
using Assets.Sources.Gameplay.Cameras;
using Assets.Sources.Gameplay.World.RepresentationOfWorld.ActionHandler;
using Assets.Sources.Infrastructure.Factories.UiFactory;
using Assets.Sources.Services.Input;
using Assets.Sources.Services.StateMachine;
using Assets.Sources.Services.StaticDataService.Configs.Windows;
using Assets.Sources.UI;
using Cysharp.Threading.Tasks;
using System;
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
        private readonly IWorldData _worldData;
        private readonly WorldStateMachine _worldStateMachine;

        public WorldChangingState(
            IInputService inputService,
            WindowsSwitcher windowsSwitcher,
            ActionHandlerStateMachine actionHandlerStateMachine,
            IUiFactory uiFactory,
            GameplayCamera gameplayCamera,
            IWorldData worldData,
            WorldStateMachine worldStateMachine)
        {
            _inputService = inputService;
            _windowsSwitcher = windowsSwitcher;
            _actionHandlerStateMachine = actionHandlerStateMachine;
            _uiFactory = uiFactory;
            _camera = gameplayCamera;
            _worldData = worldData;
            _worldStateMachine = worldStateMachine;

            _worldData.PointsData.GoalAchieved += OnGoalAchived;
        }

        ~WorldChangingState()
        {
            _worldData.PointsData.GoalAchieved -= OnGoalAchived;
        }

        protected virtual WindowType WindowType => WindowType.GameplayWindow;

        public async UniTask Enter()
        {
            _worldData.IsChangingStarted = true;

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

        private void OnGoalAchived()
        {
            _worldStateMachine.Enter<RewardState>().Forget();
        }
    }
}
