using Assets.Sources.Gameplay.StateMachine;
using Assets.Sources.Gameplay.StateMachine.States;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Assets.Sources.UI.Windows.MapSelection
{
    public class RestartMapSelectionPanel : MapSelectionPanel
    {
        [SerializeField] private Button _continueButton;

        private GameplayStateMachine _gameplayStateMachine;

        [Inject]
        private void Construct(GameplayStateMachine gameplayStateMachine)
        {
            _gameplayStateMachine = gameplayStateMachine;

            _continueButton.onClick.AddListener(OnContinueButtonClicked);
        }

        private void OnDestroy() =>
            _continueButton.onClick.RemoveListener(OnContinueButtonClicked);

        private void OnContinueButtonClicked() =>
           _gameplayStateMachine.Enter<GameplayLoopState, bool>(true).Forget();
    }
}
