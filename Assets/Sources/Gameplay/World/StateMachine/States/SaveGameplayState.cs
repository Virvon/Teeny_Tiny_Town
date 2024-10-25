using Assets.Sources.Services.StateMachine;
using Assets.Sources.UI;
using Assets.Sources.UI.Windows.World;
using Cysharp.Threading.Tasks;

namespace Assets.Sources.Gameplay.World.StateMachine.States
{
    public class SaveGameplayState : IState
    {
        private readonly WindowsSwitcher _windowsSwitcher;

        public SaveGameplayState(WindowsSwitcher windowsSwitcher) =>
            _windowsSwitcher = windowsSwitcher;

        public UniTask Enter()
        {
            _windowsSwitcher.Switch<SaveGameplayWindow>();

            return default;
        }

        public UniTask Exit() =>
            default;
    }
}
