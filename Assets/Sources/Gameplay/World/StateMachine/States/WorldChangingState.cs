using Assets.Sources.Data.WorldDatas;
using Assets.Sources.Gameplay.Cameras;
using Assets.Sources.Gameplay.World.RepresentationOfWorld.ActionHandler;
using Assets.Sources.Services.Input;
using Assets.Sources.Services.PersistentProgress;
using Assets.Sources.Services.StateMachine;
using Assets.Sources.Services.StaticDataService.Configs.Windows;
using Assets.Sources.UI;
using Assets.Sources.UI.Windows.World;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Assets.Sources.Gameplay.World.StateMachine.States
{
    public class WorldChangingState : IState
    {
        private readonly IInputService _inputService;
        private readonly WindowsSwitcher _windowsSwitcher;
        private readonly ActionHandlerStateMachine _actionHandlerStateMachine;
        private readonly GameplayCamera _camera;
        private readonly IWorldData _worldData;
        private readonly WorldStateMachine _worldStateMachine;
        private readonly IPersistentProgressService _persistentProgressService;

        public WorldChangingState(
            IInputService inputService,
            WindowsSwitcher windowsSwitcher,
            ActionHandlerStateMachine actionHandlerStateMachine,
            GameplayCamera gameplayCamera,
            IWorldData worldData,
            WorldStateMachine worldStateMachine,
            IPersistentProgressService persistentProgressService)
        {
            _inputService = inputService;
            _windowsSwitcher = windowsSwitcher;
            _actionHandlerStateMachine = actionHandlerStateMachine;
            _camera = gameplayCamera;
            _worldData = worldData;
            _worldStateMachine = worldStateMachine;
            _persistentProgressService = persistentProgressService;

            _worldData.PointsData.GoalAchieved += OnGoalAchived;
        }

        ~WorldChangingState()
        {
            _worldData.PointsData.GoalAchieved -= OnGoalAchived;
            _persistentProgressService.Progress.GameplayMovesCounter.MovesOvered -= OnMovesOvered;
        }

        protected virtual WindowType WindowType => WindowType.GameplayWindow;

        public UniTask Enter()
        {
            _worldData.IsChangingStarted = true;

            _windowsSwitcher.Switch<GameplayWindow>();

            _camera.MoveTo(new Vector3(55.1f, 78.8f, -55.1f), callback: () =>
            {
                _actionHandlerStateMachine.SetActive(true);
                _inputService.SetEnabled(true);
            });

            _persistentProgressService.Progress.GameplayMovesCounter.MovesOvered += OnMovesOvered;

            return default;
        }

        public UniTask Exit()
        {
            _actionHandlerStateMachine.SetActive(false);
            _inputService.SetEnabled(false);

            _persistentProgressService.Progress.GameplayMovesCounter.MovesOvered -= OnMovesOvered;

            return default;
        }

        private void OnMovesOvered()
        {
            _worldStateMachine.Enter<WaitingState>().Forget();
        }

        private void OnGoalAchived()
        {
            _worldStateMachine.Enter<RewardState>().Forget();
        }
    }
}
