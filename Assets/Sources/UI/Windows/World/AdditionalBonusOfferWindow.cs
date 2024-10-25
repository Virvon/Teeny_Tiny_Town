using Assets.Sources.Gameplay.World.StateMachine;
using Assets.Sources.Gameplay.World.StateMachine.States;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Assets.Sources.UI.Windows.World
{
    public class AdditionalBonusOfferWindow : Window
    {
        [SerializeField] private Button _startButton;

        private WorldStateMachine _worldStateMachine;

        [Inject]
        private void Construct(WorldStateMachine worldStateMachine)
        {
            _worldStateMachine = worldStateMachine;

            _startButton.onClick.AddListener(OnStartButtonClicked);
        }

        private void OnDisable() =>
            _startButton.onClick.RemoveListener(OnStartButtonClicked);

        private void OnStartButtonClicked() =>
            _worldStateMachine.Enter<WorldChangingState>().Forget();
    }
}
