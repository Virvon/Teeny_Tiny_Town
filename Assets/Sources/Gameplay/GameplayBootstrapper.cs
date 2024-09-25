using Assets.Sources.Gameplay.StateMachine;
using Assets.Sources.Gameplay.StateMachine.States;
using Assets.Sources.Gameplay.World.RepresentationOfWorld.ActionHandler;
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
        private readonly ActionHandlerStateMachine _actionHandlerStateMachine;
        private readonly ActionHandlerStatesFactory _actionHandlerStatesFactory;

        public GameplayBootstrapper(GameplayStateMachine gameplayStateMachine, StatesFactory statesFactory, IGameplayFactory gameplayFactory, ActionHandlerStateMachine actionHandlerStateMachine, ActionHandlerStatesFactory actionHandlerStatesFactory)
        {
            _gameplayStateMachine = gameplayStateMachine;
            _statesFactory = statesFactory;
            _gameplayFactory = gameplayFactory;
            _actionHandlerStateMachine = actionHandlerStateMachine;
            _actionHandlerStatesFactory = actionHandlerStatesFactory;
        }

        public async void Initialize()
        {
            await _gameplayFactory.CreateSelectFrame();
            await _gameplayFactory.CreateBuildingMarker();

            _actionHandlerStateMachine.RegisterState(_actionHandlerStatesFactory.CreateHandlerState<NewBuildingPlacePositionHandler>());
            _actionHandlerStateMachine.RegisterState(_actionHandlerStatesFactory.CreateHandlerState<RemovedBuildingPositionHandler>());
            _actionHandlerStateMachine.RegisterState(_actionHandlerStatesFactory.CreateHandlerState<ReplacedBuildingPositionHandler>());

            _actionHandlerStateMachine.Enter<NewBuildingPlacePositionHandler>();

            await _gameplayFactory.CreateWorldGenerator();

            _gameplayStateMachine.RegisterState(_statesFactory.Create<GameplayLoopState>());

            await _gameplayStateMachine.Enter<GameplayLoopState>();
        }
    }
}
