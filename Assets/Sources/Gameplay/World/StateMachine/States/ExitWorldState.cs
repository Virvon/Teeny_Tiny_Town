using Assets.Sources.Gameplay.StateMachine;
using Assets.Sources.Gameplay.StateMachine.States;
using Assets.Sources.Services.StateMachine;
using Assets.Sources.Services.StaticDataService.Configs.Windows;
using Assets.Sources.UI.Windows;
using Cysharp.Threading.Tasks;

namespace Assets.Sources.Gameplay.World.StateMachine.States
{
    public class ExitWorldState : IState
    {
        private readonly GameplayStateMachine _gameplayStateMachine;
        private readonly WindowsSwitcher _windowsSwitcher;

        public ExitWorldState(GameplayStateMachine gameplayStateMachine, WindowsSwitcher windowsSwitcher)
        {
            _gameplayStateMachine = gameplayStateMachine;
            _windowsSwitcher = windowsSwitcher;
        }

        public UniTask Enter()
        {
            _gameplayStateMachine.Enter<MapSelectionState>().Forget();
            return default;
        }

        public UniTask Exit()
        {
            _windowsSwitcher.Remove(WindowType.GameplayWindow);
            return default;
        }
    }
}
