using Assets.Sources.Gameplay.Cameras;
using Assets.Sources.Gameplay.StateMachine;
using Assets.Sources.Gameplay.StateMachine.States;
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
        [SerializeField] private Canvas _canvas;

        private GameplayStateMachine _gameplayStateMachine;

        [Inject]
        private void Construct(GameplayStateMachine gameplayStateMachine, GameplayCamera gameplayCamera)
        {
            _gameplayStateMachine = gameplayStateMachine;
            _canvas.worldCamera = gameplayCamera.Camera;
        }

        private void OnEnable()
        {
            _mapSelectionButton.onClick.AddListener(OnMapSelectionButtonClicked);
            _continueButton.onClick.AddListener(OnContinueButtonClicked);
        }

        private void OnDisable()
        {
            _mapSelectionButton.onClick.RemoveListener(OnMapSelectionButtonClicked);
            _continueButton.onClick.RemoveListener(OnContinueButtonClicked);
        }

        private void OnMapSelectionButtonClicked()
        {
            _gameplayStateMachine.Enter<MapSelectionState>().Forget();
        }

        private void OnContinueButtonClicked()
        {
            _gameplayStateMachine.Enter<GameplayLoopState>().Forget();
        }
    }
}
