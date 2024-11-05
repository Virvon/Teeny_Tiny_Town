using Assets.Sources.Services.StateMachine;
using Assets.Sources.UI;
using Assets.Sources.UI.Windows;
using Cysharp.Threading.Tasks;

namespace Assets.Sources.Gameplay.StateMachine.States
{
    public class ShowQuestsState : IState
    {
        private readonly WindowsSwitcher _windowsSwitcher;

        public ShowQuestsState(WindowsSwitcher windowsSwitcher) =>
            _windowsSwitcher = windowsSwitcher;

        public UniTask Enter()
        {
            _windowsSwitcher.Switch<GameplayQuestsWindow>();
            return default;
        }

        public UniTask Exit() =>
            default;
    }
}
