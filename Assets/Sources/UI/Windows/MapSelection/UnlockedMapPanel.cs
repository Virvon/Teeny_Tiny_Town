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
    public class UnlockedMapPanel : MapSelectionPanel
    {
        [SerializeField] private Button _continueButton;
        [SerializeField] private Button _restartButton;
        [SerializeField] private CanvasGroup _restartButtonCanvasGroup;

        private GameplayStateMachine _gameplayStateMachine;
        private WorldsList _worldsList;
        private IPersistentProgressService _persistentProgressService;

        [Inject]
        private void Construct(GameplayStateMachine gameplayStateMachine, WorldsList worldsList, IPersistentProgressService persistentProgressService)
        {
            _gameplayStateMachine = gameplayStateMachine;
            _worldsList = worldsList;
            _persistentProgressService = persistentProgressService;

            ChangeRestartButtonVisibility();

            _continueButton.onClick.AddListener(OnContinueButtonClicked);
            _restartButton.onClick.AddListener(OnRestartButtonClicked);
        }

        private void OnDestroy()
        {
            _continueButton.onClick.RemoveListener(OnContinueButtonClicked);
            _restartButton.onClick.RemoveListener(OnRestartButtonClicked);
        }

        public override void Open()
        {
            base.Open();
            ChangeRestartButtonVisibility();
        }

        private void ChangeRestartButtonVisibility()
        {
            if (_persistentProgressService.Progress.CurrentWorldData.IsChangingStarted)
            {
                _restartButtonCanvasGroup.alpha = 1;
                _restartButtonCanvasGroup.blocksRaycasts = true;
                _restartButtonCanvasGroup.interactable = true;
            }
            else
            {
                _restartButtonCanvasGroup.alpha = 0;
                _restartButtonCanvasGroup.blocksRaycasts = false;
                _restartButtonCanvasGroup.interactable = false;
            }
        }

        private void OnRestartButtonClicked()
        {
            _worldsList.CleanCurrentWorld();
            _worldsList.StartCurrentWorld();
        }

        private void OnContinueButtonClicked() =>
           _gameplayStateMachine.Enter<GameplayLoopState>().Forget();
    }
}
