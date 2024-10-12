using Assets.Sources.Gameplay.StateMachine;
using Assets.Sources.Gameplay.StateMachine.States;
using Assets.Sources.Gameplay.World;
using Cysharp.Threading.Tasks;
using System;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Assets.Sources.UI.Windows
{
    public class MapSelectionWindow : Window
    {
        [SerializeField] private Button _nextMapButton;
        [SerializeField] private Button _previousMapButton;
        [SerializeField] private Button _continueButton;

        private WorldsList _worldsList;
        private GameplayStateMachine _gameplayStateMachine;

        [Inject]
        private void Construct(WorldsList worldsList, GameplayStateMachine gameplayStateMachine)
        {
            _worldsList = worldsList;
            _gameplayStateMachine = gameplayStateMachine;
        }

        private void OnEnable()
        {
            _nextMapButton.onClick.AddListener(OnNextMapButtonClicked);
            _previousMapButton.onClick.AddListener(OnPreviousMapButtonClicked);
            _continueButton.onClick.AddListener(OnContinueButtonClicked);
        }

        private void OnDisable()
        {
            _nextMapButton.onClick.RemoveListener(OnNextMapButtonClicked);
            _previousMapButton.onClick.RemoveListener(OnPreviousMapButtonClicked);
            _continueButton.onClick.RemoveListener(OnContinueButtonClicked);
        }

        private async void OnPreviousMapButtonClicked()
        {
            await _worldsList.ShowPreviousWorld();
        }

        private async void OnNextMapButtonClicked()
        {
            await _worldsList.ShowNextWorld();
        }

        private void OnContinueButtonClicked()
        {
            _gameplayStateMachine.Enter<GameplayLoopState>().Forget();
        }
    }
}
