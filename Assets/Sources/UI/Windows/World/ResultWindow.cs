using System;
using Assets.Sources.Gameplay.StateMachine;
using Assets.Sources.Gameplay.StateMachine.States;
using Assets.Sources.Gameplay.World.StateMachine;
using Assets.Sources.Gameplay.World.StateMachine.States;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Assets.Sources.UI.Windows.World
{
    public class ResultWindow : Window
    {
        [SerializeField] private Button _restartButton;
        [SerializeField] private Button _startWindowButton;
        [SerializeField] private Button _mapSelectionButton;

        private WorldStateMachine _worldStateMachine;
        private GameplayStateMachine _gameplayStateMachine;

        [Inject]
        private void Construct(WorldStateMachine worldStateMachine, GameplayStateMachine gameplayStateMachine)
        {
            _worldStateMachine = worldStateMachine;
            _gameplayStateMachine = gameplayStateMachine;

            _restartButton.onClick.AddListener(OnRestartButtonClicked);
            _startWindowButton.onClick.AddListener(OnStartWindowButtonClicked);
            _mapSelectionButton.onClick.AddListener(OnMapSelectionButtonClicked);
        }

        private void OnDisable()
        {
            _restartButton.onClick.RemoveListener(OnRestartButtonClicked);
            _startWindowButton.onClick.RemoveListener(OnStartWindowButtonClicked);
            _mapSelectionButton.onClick.RemoveListener(OnMapSelectionButtonClicked);
        }

        private void OnRestartButtonClicked() =>
            _worldStateMachine.Enter<WorldStartState>().Forget();

        private void OnStartWindowButtonClicked() =>
            _worldStateMachine.Enter<ExitWorldState, Action>(() => _gameplayStateMachine.Enter<GameStartState>().Forget()).Forget();

        private void OnMapSelectionButtonClicked() =>
            _worldStateMachine.Enter<ExitWorldState, Action>(() => _gameplayStateMachine.Enter<MapSelectionState>().Forget()).Forget();
    }
}
