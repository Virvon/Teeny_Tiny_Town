using Assets.Sources.Gameplay.StateMachine;
using Assets.Sources.Gameplay.StateMachine.States;
using Assets.Sources.Infrastructure.GameplayFactory;
using Assets.Sources.Services.StateMachine;
using Zenject;

namespace Assets.Sources.Gameplay
{
    public class GameplayBootstrapper : IInitializable
    {
        private readonly GameplayStateMachine _gameplayStateMachine;
        private readonly StatesFactory _statesFactory;
        private readonly IGameplayFactory _gameplayFactory;

        public GameplayBootstrapper(GameplayStateMachine gameplayStateMachine, StatesFactory statesFactory, IGameplayFactory gameplayFactory)
        {
            _gameplayStateMachine = gameplayStateMachine;
            _statesFactory = statesFactory;
            _gameplayFactory = gameplayFactory;
        }

        public async void Initialize()
        {
            await _gameplayFactory.CreateWorldGenerator();

            _gameplayStateMachine.RegisterState(_statesFactory.Create<GameplayLoopState>());

            await _gameplayStateMachine.Enter<GameplayLoopState>();
        }
    }
}
