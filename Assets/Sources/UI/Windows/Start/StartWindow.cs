﻿using Assets.Sources.Gameplay.StateMachine;
using Assets.Sources.Gameplay.StateMachine.States;
using Assets.Sources.Infrastructure.GameStateMachine;
using Assets.Sources.Infrastructure.GameStateMachine.States;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Assets.Sources.UI.Windows.Start
{
    public class StartWindow : Window
    {
        [SerializeField] private Button _mapSelectionButton;
        [SerializeField] private Button _continueButton;
        [SerializeField] private Button _sandboxButton;
        [SerializeField] private Button _collectionButton;
        [SerializeField] private Button _questsButton;

        private GameplayStateMachine _gameplayStateMachine;
        private GameStateMachine _gameStateMachine;

        [Inject]
        private void Construct(GameplayStateMachine gameplayStateMachine, GameStateMachine gameStateMachine)
        {
            _gameplayStateMachine = gameplayStateMachine;
            _gameStateMachine = gameStateMachine;

            _mapSelectionButton.onClick.AddListener(OnMapSelectionButtonClicked);
            _continueButton.onClick.AddListener(OnContinueButtonClicked);
            _sandboxButton.onClick.AddListener(OnSandboxButtonClicked);
            _collectionButton.onClick.AddListener(OnCollectionButtonClicked);
            _questsButton.onClick.AddListener(OnQuestsButtonClicked);
        }

        private void OnDestroy()
        {
            _mapSelectionButton.onClick.RemoveListener(OnMapSelectionButtonClicked);
            _continueButton.onClick.RemoveListener(OnContinueButtonClicked);
            _sandboxButton.onClick.AddListener(OnSandboxButtonClicked);
            _collectionButton.onClick.RemoveListener(OnCollectionButtonClicked);
            _questsButton.onClick.RemoveListener(OnQuestsButtonClicked);
        }

        private void OnMapSelectionButtonClicked() =>
            _gameplayStateMachine.Enter<MapSelectionState>().Forget();

        private void OnContinueButtonClicked() =>
            _gameplayStateMachine.Enter<GameplayLoopState, bool>(false).Forget();

        private void OnSandboxButtonClicked() =>
            _gameStateMachine.Enter<SandboxState>().Forget();

        private void OnCollectionButtonClicked() =>
            _gameStateMachine.Enter<CollectionState>().Forget();

        private void OnQuestsButtonClicked() =>
            _gameplayStateMachine.Enter<ShowQuestsState>().Forget();
    }
}
