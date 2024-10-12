using Assets.Sources.Data;
using Assets.Sources.Gameplay.World.RepresentationOfWorld;
using Assets.Sources.Gameplay.World.RepresentationOfWorld.ActionHandler;
using Assets.Sources.Gameplay.World.StateMachine;
using Assets.Sources.Gameplay.World.StateMachine.States;
using Assets.Sources.Gameplay.World.WorldInfrastructure;
using Assets.Sources.Infrastructure.Factories.WorldFactory;
using Assets.Sources.Services.StateMachine;
using Zenject;

namespace Assets.Sources.Gameplay.World
{
    public class WorldBootstrapper : IInitializable
    {
        private readonly WorldChanger _worldChanger;
        private readonly IWorldFactory _worldFactory;
        private readonly WorldStateMachine _worldStateMachine;
        private readonly StatesFactory _statesFactory;
        private readonly ActionHandlerStateMachine _actionHandlerStateMachine;
        private readonly ActionHandlerStatesFactory _actionHandlerStatesFactory;
        private readonly WorldData _worldData;

        public WorldBootstrapper(
            WorldChanger worldChanger,
            IWorldFactory worldFactory,
            WorldStateMachine worldStateMachine,
            StatesFactory statesFactory,
            ActionHandlerStateMachine actionHandlerStateMachine,
            ActionHandlerStatesFactory actionHandlerStatesFactory,
            WorldData worldData)
        {
            _worldChanger = worldChanger;
            _worldFactory = worldFactory;
            _worldStateMachine = worldStateMachine;
            _statesFactory = statesFactory;
            _actionHandlerStateMachine = actionHandlerStateMachine;
            _actionHandlerStatesFactory = actionHandlerStatesFactory;
            _worldData = worldData;
        }

        public async void Initialize()
        {
            WorldGenerator worldGenerator = await _worldFactory.CreateWorldGenerator();

            worldGenerator.PlaceToCenter(_worldData.Length, _worldData.Width);
            await _worldChanger.Generate(worldGenerator);

            _worldStateMachine.RegisterState(_statesFactory.Create<WorldChangingState>());
            _worldStateMachine.RegisterState(_statesFactory.Create<ExitWorldState>());
            _worldStateMachine.RegisterState(_statesFactory.Create<StoreState>());
            _worldStateMachine.RegisterState(_statesFactory.Create<WorldBootsrappState>());
        }

        private void RegisterActionHandlerStates()
        {
            _actionHandlerStateMachine.RegisterState(_actionHandlerStatesFactory.CreateHandlerState<NewBuildingPlacePositionHandler>());
            _actionHandlerStateMachine.RegisterState(_actionHandlerStatesFactory.CreateHandlerState<RemovedBuildingPositionHandler>());
            _actionHandlerStateMachine.RegisterState(_actionHandlerStatesFactory.CreateHandlerState<ReplacedBuildingPositionHandler>());
        }
    }
}
