using Assets.Sources.Gameplay.World.StateMachine;
using Assets.Sources.Gameplay.World.StateMachine.States;
using Cysharp.Threading.Tasks;
using System;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Assets.Sources.UI.Windows
{
    public class GameplayWindow : Window
    {
        [SerializeField] private Button _exitButton;
        [SerializeField] private Button _storeButton;

        private WorldStateMachine _worldStateMachine;

        [Inject]
        private void Construct(WorldStateMachine worldStateMachine) =>
            _worldStateMachine = worldStateMachine;

        private void OnEnable()
        {
            _exitButton.onClick.AddListener(OnExitButtonClicked);
            _storeButton.onClick.AddListener(OnStoreButtonClicked);
        }

        private void OnDisable()
        {
            _exitButton.onClick.RemoveListener(OnExitButtonClicked);
            _storeButton.onClick.RemoveListener(OnStoreButtonClicked);
        }

        private void OnExitButtonClicked() =>
            _worldStateMachine.Enter<ExitWorldState>().Forget();

        private void OnStoreButtonClicked() =>
            _worldStateMachine.Enter<StoreState>().Forget();
    }
}
