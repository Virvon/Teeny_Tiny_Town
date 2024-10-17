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
        [SerializeField] private Button _hideButton;
        
        [Inject]
        private void Construct(WorldStateMachine worldStateMachine) =>
            WorldStateMachine = worldStateMachine;

        protected WorldStateMachine WorldStateMachine { get; private set; }

        protected virtual void OnEnable() =>
            _hideButton.onClick.AddListener(OnHideButtonClicked);

        protected virtual void OnDisable() =>
            _hideButton.onClick.RemoveListener(OnHideButtonClicked);

        private void OnHideButtonClicked() =>
            WorldStateMachine.Enter<ExitWorldState>().Forget();
    }
}
