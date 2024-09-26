using Assets.Sources.Gameplay.StateMachine;
using Assets.Sources.Gameplay.StateMachine.States;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Assets.Sources.UI.Windows
{
    public class StartWindow : Window
    {
        [SerializeField] private Button _mapSelectionButton;

        private GameplayStateMachine _gameplayStateMachine;

        [Inject]
        private void Construct(GameplayStateMachine gameplayStateMachine)
        {
            _gameplayStateMachine = gameplayStateMachine;
        }

        private void OnEnable()
        {
            _mapSelectionButton.onClick.AddListener(OnMapSelectionButtonClicked);
        }

        private void OnDisable()
        {
            _mapSelectionButton.onClick.RemoveListener(OnMapSelectionButtonClicked);
        }

        private void OnMapSelectionButtonClicked()
        {
            _gameplayStateMachine.Enter<MapSelectionState>().Forget();
        }
    }
}
