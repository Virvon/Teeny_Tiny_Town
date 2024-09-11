using Assets.Sources.Infrastructure.GameStateMachine;
using Assets.Sources.Infrastructure.GameStateMachine.States;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace Assets.Sources.Infrastructure
{
    public class GameBootstrapper : MonoBehaviour
    {
        private GameStateMachine.GameStateMachine _gameStateMachine;
        private StatesFactory _statesFactory;

        [Inject]
        private void Construct(GameStateMachine.GameStateMachine gameStateMachine, StatesFactory statesFactory)
        {
            _gameStateMachine = gameStateMachine;
            _statesFactory = statesFactory;
        }

        private void Start()
        {
            _gameStateMachine.RegisterState(_statesFactory.Create<BootstrapState>());
            _gameStateMachine.RegisterState(_statesFactory.Create<GameLoopState>());

            _gameStateMachine.Enter<BootstrapState>().Forget();

            DontDestroyOnLoad(this);
        }
    }
}
