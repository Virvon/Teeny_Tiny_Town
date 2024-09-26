using Assets.Sources.Services.StateMachine;
using Assets.Sources.Services.StaticDataService.Configs.Windows;
using Assets.Sources.UI.Windows;
using Cysharp.Threading.Tasks;

namespace Assets.Sources.Gameplay.StateMachine.States
{
    public class MapSelectionState : IState
    {
        private readonly WindowsSwitcher _windowsSwitcher;

        public MapSelectionState(WindowsSwitcher windowsSwitcher)
        {
            _windowsSwitcher = windowsSwitcher;
        }

        public UniTask Enter()
        {
            _windowsSwitcher.Switch(WindowType.MapSelectionWindow);

            return default;
        }

        public UniTask Exit()
        {
            return default;
        }
    }
}
