using Assets.Sources.Gameplay.StateMachine;
using Assets.Sources.Gameplay.StateMachine.States;
using Assets.Sources.Gameplay.World.StateMachine;
using Assets.Sources.Gameplay.World.StateMachine.States;
using Cysharp.Threading.Tasks;
using System;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Assets.Sources.UI.Windows.World
{
    public class AdditionalBonusOfferWindow : BluredBackgroundWindow
    {
        [SerializeField] private Button _startButton;
        [SerializeField] private Button _hideWindowButton;

        private WorldStateMachine _worldStateMachine;
        private GameplayStateMachine _gameplayStateMachine;

        [Inject]
        private void Construct(WorldStateMachine worldStateMachine, GameplayStateMachine gameplayStateMachine)
        {
            _worldStateMachine = worldStateMachine;
            _gameplayStateMachine = gameplayStateMachine;

            _startButton.onClick.AddListener(OnStartButtonClicked);
            _hideWindowButton.onClick.AddListener(OnHideWindowButtonClicked);
        }

        private void OnDisable()
        {
            _startButton.onClick.RemoveListener(OnStartButtonClicked);
            _hideWindowButton.onClick.RemoveListener(OnHideWindowButtonClicked);
        }

        private void OnStartButtonClicked() =>
            _worldStateMachine.Enter<WorldChangingState>().Forget();

        private void OnHideWindowButtonClicked() =>
            _worldStateMachine.Enter<ExitWorldState, Action>(() => _gameplayStateMachine.Enter<MapSelectionState>().Forget()).Forget();
    }
}
