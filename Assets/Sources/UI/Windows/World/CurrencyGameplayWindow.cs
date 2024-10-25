using Assets.Sources.Gameplay.World.StateMachine;
using Assets.Sources.Gameplay.World.StateMachine.States;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Sources.UI.Windows.World
{
    public class CurrencyGameplayWindow : GameplayWindow
    {
        [SerializeField] private Button _storeButton;

        protected override void Subscrube()
        {
            base.Subscrube();
            _storeButton.onClick.AddListener(OnStoreButtonClicked);
        }

        protected override void Unsubscruby()
        {
            base.Unsubscruby();
            _storeButton.onClick.RemoveListener(OnStoreButtonClicked);
        }

        private void OnStoreButtonClicked() =>
           WorldStateMachine.Enter<StoreState>().Forget();
    }
}
