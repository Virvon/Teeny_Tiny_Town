using Assets.Sources.Gameplay.StateMachine;
using Assets.Sources.Gameplay.StateMachine.States;
using Assets.Sources.Gameplay.World.StateMachine;
using Assets.Sources.Gameplay.World.StateMachine.States;
using Assets.Sources.Services.PersistentProgress;
using Assets.Sources.Services.StaticDataService;
using Cysharp.Threading.Tasks;
using System;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Assets.Sources.UI.Windows.World
{
    public class WaitingWindow : BluredBackgroundWindow
    {
        [SerializeField] private Button _mapSelectionWindowOpenButton;
        [SerializeField] private Button _startWindowOpenButton;
        [SerializeField] private Button _adButton;

        private WorldStateMachine _worldStateMachine;
        private GameplayStateMachine _gameplayStateMachine;
        private IStaticDataService _staticDataService;
        private IPersistentProgressService _persistentProgressService;

        [Inject]
        private void Construct(
            WorldStateMachine worldStateMachine,
            GameplayStateMachine gameplayStateMachine,
            IStaticDataService staticDataService,
            IPersistentProgressService persistentProgressService)
        {
            _worldStateMachine = worldStateMachine;
            _gameplayStateMachine = gameplayStateMachine;
            _staticDataService = staticDataService;
            _persistentProgressService = persistentProgressService;

            _mapSelectionWindowOpenButton.onClick.AddListener(OnMapSelectionWindowOpenButtonClicked);
            _startWindowOpenButton.onClick.AddListener(OnStartWindowOpenButtonClicked);
            _adButton.onClick.AddListener(OnAdButtonClicked);
        }

        private void OnDestroy()
        {
            _mapSelectionWindowOpenButton.onClick.RemoveListener(OnMapSelectionWindowOpenButtonClicked);
            _startWindowOpenButton.onClick.RemoveListener(OnStartWindowOpenButtonClicked);
            _adButton.onClick.RemoveListener(OnAdButtonClicked);
        }

        private void OnAdButtonClicked()
        {
            _persistentProgressService.Progress.GameplayMovesCounter.SetCount(_staticDataService.WorldsConfig.AvailableMovesCount);
            _worldStateMachine.Enter<WorldStartState>().Forget();
        }

        private void OnStartWindowOpenButtonClicked() =>
            _worldStateMachine.Enter<ExitWorldState, Action>(() => _gameplayStateMachine.Enter<GameStartState>().Forget()).Forget();

        private void OnMapSelectionWindowOpenButtonClicked() =>
            _worldStateMachine.Enter<ExitWorldState, Action>(() => _gameplayStateMachine.Enter<MapSelectionState>().Forget()).Forget();
    }
}
