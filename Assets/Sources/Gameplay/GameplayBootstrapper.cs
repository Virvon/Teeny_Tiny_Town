using Assets.Sources.Gameplay.Cameras;
using Assets.Sources.Gameplay.StateMachine;
using Assets.Sources.Gameplay.StateMachine.States;
using Assets.Sources.Gameplay.World;
using Assets.Sources.Infrastructure.Factories.GameplayFactory;
using Assets.Sources.Services.StateMachine;
using Zenject;

namespace Assets.Sources.Gameplay
{
    public class GameplayBootstrapper : IInitializable
    {
        private readonly GameplayStateMachine _gameplayStateMachine;
        private readonly StatesFactory _statesFactory;
        private readonly IGameplayFactory _gameplayFactory;

        public GameplayBootstrapper(
            GameplayStateMachine gameplayStateMachine,
            StatesFactory statesFactory,
            IGameplayFactory gameplayFactory)
        {
            _gameplayStateMachine = gameplayStateMachine;
            _statesFactory = statesFactory;
            _gameplayFactory = gameplayFactory;
        }

        public async void Initialize()
        {
            await _gameplayFactory.CreateCamera();
            WorldsList worldsList = await _gameplayFactory.CreateWorldsList();

            RegisterGameplayStates();

            await worldsList.CreateCurrentWorld();

            await _gameplayStateMachine.Enter<GameplayStartState>();
        }

        private void RegisterGameplayStates()
        {
            _gameplayStateMachine.RegisterState(_statesFactory.Create<GameplayLoopState>());
            _gameplayStateMachine.RegisterState(_statesFactory.Create<GameplayStartState>());
            _gameplayStateMachine.RegisterState(_statesFactory.Create<MapSelectionState>());
        }  
    }
}
