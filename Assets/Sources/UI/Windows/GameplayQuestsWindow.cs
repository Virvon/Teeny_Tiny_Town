using Assets.Sources.Gameplay.StateMachine;
using Assets.Sources.Gameplay.StateMachine.States;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Assets.Sources.UI.Windows
{
    public class GameplayQuestsWindow : BluredBackgroundWindow
    {
        [SerializeField] private Button _hideButton;

        private GameplayStateMachine _gameplayStateMachine;

        [Inject]
        private void Construct(GameplayStateMachine gameplayStateMachine)
        {
            _gameplayStateMachine = gameplayStateMachine;

            _hideButton.onClick.AddListener(OnHideButtonClicked);
        }

        private void OnDestroy() =>
            _hideButton.onClick.RemoveListener(OnHideButtonClicked);

        private void OnHideButtonClicked() =>
            _gameplayStateMachine.Enter<GameStartState>().Forget();
    }
}
