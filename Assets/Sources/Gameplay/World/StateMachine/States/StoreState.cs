using Assets.Sources.Infrastructure.Factories.UiFactory;
using Assets.Sources.Services.StateMachine;
using Assets.Sources.UI;
using Assets.Sources.UI.Windows.World;
using Cysharp.Threading.Tasks;

namespace Assets.Sources.Gameplay.World.StateMachine.States
{
    public class StoreState : IState
    {
        private readonly WindowsSwitcher _windowsSwitcher;
        private readonly IUiFactory _uiFactory;

        public StoreState(WindowsSwitcher windowsSwitcher, IUiFactory uiFactory)
        {
            _windowsSwitcher = windowsSwitcher;
            _uiFactory = uiFactory;
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
