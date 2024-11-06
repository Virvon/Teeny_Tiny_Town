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

        public async UniTask Enter()
        {
            await _windowsSwitcher.Switch<StoreWindow>();
        }

        public UniTask Exit()
        {
            return default;
        }
    }
}
