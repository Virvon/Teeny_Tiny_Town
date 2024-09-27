using Assets.Sources.Gameplay.StateMachine;
using Assets.Sources.Gameplay.StateMachine.States;
using Assets.Sources.Gameplay.World;
using Assets.Sources.Gameplay.World.RepresentationOfWorld.ActionHandler;
using Assets.Sources.Infrastructure.Factories.GameplayFactory;
using Assets.Sources.Services.StateMachine;
using Assets.Sources.UI.Windows;
using Zenject;

namespace Assets.Sources.Gameplay
{
    public class GameplayBootstrapper : IInitializable
    {
        private readonly GameplayStateMachine _gameplayStateMachine;
        private readonly StatesFactory _statesFactory;
        
        private readonly WindowsSwitcher _windowsSwitcher;
        private readonly IGameplayFactory _gameplayFactory;

        public GameplayBootstrapper(
            GameplayStateMachine gameplayStateMachine,
            StatesFactory statesFactory,
            
            WindowsSwitcher windowsSwitcher,
            IGameplayFactory gameplayFactory)
        {
            _gameplayStateMachine = gameplayStateMachine;
            _statesFactory = statesFactory;
            
            _windowsSwitcher = windowsSwitcher;
            _gameplayFactory = gameplayFactory;
        }

        public async void Initialize()
        {          
            WorldsList worldsList = await _gameplayFactory.CreateWorldsList();

            RegisterGameplayStates();

            await _windowsSwitcher.CreateWindows();
            await worldsList.Create();

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
