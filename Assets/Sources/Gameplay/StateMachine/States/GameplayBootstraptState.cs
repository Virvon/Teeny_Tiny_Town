using Assets.Sources.Services.StateMachine;
using Assets.Sources.UI.Windows;
using Cysharp.Threading.Tasks;

namespace Assets.Sources.Gameplay.StateMachine.States
{
    public class GameplayBootstraptState : IState
    {
        private readonly GameplayStateMachine _gameplayStateMachine;
        private readonly WindowsSwitcher _windowsSwitcher;

        public GameplayBootstraptState(GameplayStateMachine gameplayStateMachine, WindowsSwitcher windowsSwitcher)
        {
            _gameplayStateMachine = gameplayStateMachine;
            _windowsSwitcher = windowsSwitcher;
        }

        public async UniTask Enter()
        {
            await _windowsSwitcher.CreateWindows();

            _gameplayStateMachine.Enter<GameplayStartState>().Forget();
        }

        public UniTask Exit()
        {
            return default;
        }
    }
}
