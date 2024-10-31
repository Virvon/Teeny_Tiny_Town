using Assets.Sources.Services.StateMachine;
using Assets.Sources.UI;
using Assets.Sources.UI.Windows.World;
using Cysharp.Threading.Tasks;

namespace Assets.Sources.Gameplay.World.StateMachine.States
{
    public class QuestsState : IState
    {
        private readonly WindowsSwitcher _windowsSwitcher;

        public QuestsState(WindowsSwitcher windowsSwitcher)
        {
            _windowsSwitcher = windowsSwitcher;
        }

        public UniTask Enter()
        {
            _windowsSwitcher.Switch<QuestsWindow>("questa");
            return default;
        }

        public UniTask Exit() =>
            default;
    }
}
