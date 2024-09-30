using Assets.Sources.Gameplay.World.StateMachine;
using Assets.Sources.Gameplay.World.StateMachine.States;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Assets.Sources.UI.Windows
{
    public class GameplayWindow : Window
    {
        [SerializeField] private Button _exitButton;

        private WorldStateMachine _worldStateMachine;

        [Inject]
        private void Construct(WorldStateMachine worldStateMachine)
        {
            _worldStateMachine = worldStateMachine;
        }

        private void OnEnable()
        {
            _exitButton.onClick.AddListener(OnExitButtonClicked);
        }

        private void OnDisable()
        {
            _exitButton.onClick.RemoveListener(OnExitButtonClicked);
        }

        private void OnExitButtonClicked()
        {
            _worldStateMachine.Enter<ExitWorldState>().Forget();
        }
    }
}
