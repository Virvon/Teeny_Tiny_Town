using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Assets.Sources.Infrastructure.GameStateMachine.States
{
    public class BootstrapState : IState
    {
        private readonly GameStateMachine _gameStateMachine;

        public BootstrapState(GameStateMachine gameStateMachine)
        {
            _gameStateMachine = gameStateMachine;
        }

        public UniTask Enter()
        {
            _gameStateMachine.Enter<GameLoopState>().Forget();

            return default;
        }

        public UniTask Exit() =>
            default;
    }
}
