using Assets.Sources.Gameplay.StateMachine;
using Assets.Sources.Gameplay.StateMachine.States;
using Assets.Sources.Gameplay.World;
using Assets.Sources.Services.PersistentProgress;
using Cysharp.Threading.Tasks;
using System;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Assets.Sources.UI.Windows.MapSelection
{
    public class ContinueMapSelectionPanel : MapSelectionPanel
    {
        [SerializeField] private Button _continueButton;
        [SerializeField] private Button _restartButton;

        private GameplayStateMachine _gameplayStateMachine;
        private WorldsList _worldsList;

        [Inject]
        private void Construct(GameplayStateMachine gameplayStateMachine, WorldsList worldsList)
        {
            _gameplayStateMachine = gameplayStateMachine;
            _worldsList = worldsList;

            _continueButton.onClick.AddListener(OnContinueButtonClicked);
            _restartButton.onClick.AddListener(OnRestartButtonClicked);
        }

        private void OnDestroy()
        {
            _continueButton.onClick.RemoveListener(OnContinueButtonClicked);
            _restartButton.onClick.RemoveListener(OnRestartButtonClicked);
        }

        private async void OnRestartButtonClicked()
        {
            _worldsList.CleanCurrentWorld();
            await _worldsList.StartLastPlayedWorld();
        }

        private void OnContinueButtonClicked() =>
           _gameplayStateMachine.Enter<GameplayLoopState, bool>(true).Forget();
    }
}
