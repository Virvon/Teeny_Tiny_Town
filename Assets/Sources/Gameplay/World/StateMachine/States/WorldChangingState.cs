using Assets.Sources.Data.World;
using Assets.Sources.Gameplay.Cameras;
using Assets.Sources.Gameplay.World.RepresentationOfWorld.ActionHandler;
using Assets.Sources.Gameplay.World.RepresentationOfWorld.Markers;
using Assets.Sources.Gameplay.World.WorldInfrastructure.NextBuildingForPlacing;
using Assets.Sources.Gameplay.World.WorldInfrastructure.WorldChangers;
using Assets.Sources.Sandbox.ActionHandler;
using Assets.Sources.Services.Input;
using Assets.Sources.Services.PersistentProgress;
using Assets.Sources.Services.StateMachine;
using Assets.Sources.UI;
using Assets.Sources.UI.Windows.World;
using Cysharp.Threading.Tasks;
using System;
using UnityEngine;

namespace Assets.Sources.Gameplay.World.StateMachine.States
{
    public class WorldChangingState : IState, IDisposable
    {
        private readonly IInputService _inputService;
        private readonly WindowsSwitcher _windowsSwitcher;
        private readonly ActionHandlerStateMachine _actionHandlerStateMachine;
        private readonly GameplayCamera _camera;
        private readonly IWorldData _worldData;
        private readonly WorldStateMachine _worldStateMachine;
        private readonly IPersistentProgressService _persistentProgressService;
        private readonly MarkersVisibility _markersVisibility;
        private readonly NextBuildingForPlacingCreator _nextBuildingForPlacingCreator;
        private readonly IWorldChanger _worldChanger;

        public WorldChangingState(
            IInputService inputService,
            WindowsSwitcher windowsSwitcher,
            ActionHandlerStateMachine actionHandlerStateMachine,
            GameplayCamera gameplayCamera,
            IWorldData worldData,
            WorldStateMachine worldStateMachine,
            IPersistentProgressService persistentProgressService,
            MarkersVisibility markersVisibility,
            IWorldChanger worldChanger,
            NextBuildingForPlacingCreator nextBuildingForPlacingCreator)
        {
            _inputService = inputService;
            _windowsSwitcher = windowsSwitcher;
            _actionHandlerStateMachine = actionHandlerStateMachine;
            _camera = gameplayCamera;
            _worldData = worldData;
            _worldStateMachine = worldStateMachine;
            _persistentProgressService = persistentProgressService;
            _markersVisibility = markersVisibility;
            _worldChanger = worldChanger;
            _nextBuildingForPlacingCreator = nextBuildingForPlacingCreator;

            _worldData.PointsData.GoalAchieved += OnGoalAchived;
        }

        public void Dispose()
        {
            Debug.Log("destruct world changing state");

            _worldData.PointsData.GoalAchieved -= OnGoalAchived;
            _persistentProgressService.Progress.GameplayMovesCounter.MovesOvered -= OnMovesOvered;                
        }

        public UniTask Enter()
        {
            _worldData.IsChangingStarted = true;
            _persistentProgressService.Progress.LastPlayedWorldDataId = _worldData.Id;

            _windowsSwitcher.HideCurrentWindow();

            _camera.MoveTo(new Vector3(55.1f, 78.8f, -55.1f), callback: async () =>
            {
                if(_nextBuildingForPlacingCreator.CheckFreeTiles(_worldChanger.Tiles))
                {
                    _actionHandlerStateMachine.SetActive(true);
                    _inputService.SetEnabled(true);
                    _markersVisibility.ChangeAllowedVisibility(true);

                    await _windowsSwitcher.Switch<GameplayWindow>();
                }
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
