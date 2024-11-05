using Assets.Sources.Services.StateMachine;
using Assets.Sources.UI;
using Assets.Sources.UI.Windows.World;
using Cysharp.Threading.Tasks;

namespace Assets.Sources.Gameplay.World.StateMachine.States
{
    public class StoreState : IState
    {
        private readonly WindowsSwitcher _windowsSwitcher;

        public StoreState(WindowsSwitcher windowsSwitcher)
        {
            _windowsSwitcher = windowsSwitcher;
        }

        public UniTask Enter()
        {
            _windowsSwitcher.Switch<StoreWindow>();
            return default;
        }

        public UniTask Exit()
        {
            return default;
        }
    }
}
