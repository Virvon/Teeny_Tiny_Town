using Assets.Sources.Gameplay.World.StateMachine;
using Assets.Sources.Gameplay.World.StateMachine.States;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Sources.UI.Windows
{
    public class CurrencyGameplayWindow : GameplayWindow
    {
        [SerializeField] private Button _storeButton;

        protected override void OnEnable()
        {
            base.OnEnable();
            _storeButton.onClick.AddListener(OnStoreButtonClicked);
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            _storeButton.onClick.RemoveListener(OnStoreButtonClicked);
        }

        private void OnStoreButtonClicked() =>
           WorldStateMachine.Enter<StoreState>().Forget();
    }
}
