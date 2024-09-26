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
        private readonly IGameplayFactory _gameplayFactory;
        private readonly ActionHandlerStateMachine _actionHandlerStateMachine;
        private readonly ActionHandlerStatesFactory _actionHandlerStatesFactory;
        private readonly WindowsSwitcher _windowsSwitcher;

        public GameplayBootstrapper(GameplayStateMachine gameplayStateMachine, StatesFactory statesFactory, IGameplayFactory gameplayFactory, ActionHandlerStateMachine actionHandlerStateMachine, ActionHandlerStatesFactory actionHandlerStatesFactory, WindowsSwitcher windowsSwitcher)
        {
            _gameplayStateMachine = gameplayStateMachine;
            _statesFactory = statesFactory;
            _gameplayFactory = gameplayFactory;
            _actionHandlerStateMachine = actionHandlerStateMachine;
            _actionHandlerStatesFactory = actionHandlerStatesFactory;
            _windowsSwitcher = windowsSwitcher;
        }

        public async void Initialize()
        {
            await _gameplayFactory.CreateSelectFrame();
            await _gameplayFactory.CreateBuildingMarker();
            WorldsList worldsList = await _gameplayFactory.CreateWorldsList();

            _actionHandlerStateMachine.RegisterState(_actionHandlerStatesFactory.CreateHandlerState<NewBuildingPlacePositionHandler>());
            _actionHandlerStateMachine.RegisterState(_actionHandlerStatesFactory.CreateHandlerState<RemovedBuildingPositionHandler>());
            _actionHandlerStateMachine.RegisterState(_actionHandlerStatesFactory.CreateHandlerState<ReplacedBuildingPositionHandler>());

            _actionHandlerStateMachine.Enter<NewBuildingPlacePositionHandler>();

            _gameplayStateMachine.RegisterState(_statesFactory.Create<GameplayLoopState>());
            _gameplayStateMachine.RegisterState(_statesFactory.Create<GameplayStartState>());
            _gameplayStateMachine.RegisterState(_statesFactory.Create<MapSelectionState>());

            await _windowsSwitcher.CreateWindows();
            await worldsList.Create();

            await _gameplayStateMachine.Enter<GameplayStartState>();
        }
    }
}
