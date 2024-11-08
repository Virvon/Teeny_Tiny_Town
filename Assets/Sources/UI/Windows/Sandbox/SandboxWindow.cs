using Assets.Sources.Infrastructure.GameStateMachine;
using Assets.Sources.Infrastructure.GameStateMachine.States;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Assets.Sources.UI.Windows.Sandbox
{
    public class SandboxWindow : Window
    {
        [SerializeField] private Button _hideButton;

        private GameStateMachine _gameStateMachine;

        [Inject]
        private void Construct(GameStateMachine gameStateMachine)
        {
            _gameStateMachine = gameStateMachine;

            _hideButton.onClick.AddListener(OnHideButtonClicked);
        }

        private void OnDestroy() =>
            _hideButton.onClick.RemoveListener(OnHideButtonClicked);

        private void OnHideButtonClicked() =>
            _gameStateMachine.Enter<GameLoopState>().Forget();
    }
}
