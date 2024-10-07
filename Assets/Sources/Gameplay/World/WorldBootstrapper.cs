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

        public WorldBootstrapper(
            WorldChanger worldChanger,
            IWorldFactory worldFactory,
            WorldStateMachine worldStateMachine,
            StatesFactory statesFactory,
            ActionHandlerStateMachine actionHandlerStateMachine,
            ActionHandlerStatesFactory actionHandlerStatesFactory)
        {
            _worldChanger = worldChanger;
            _worldFactory = worldFactory;
            _worldStateMachine = worldStateMachine;
            _statesFactory = statesFactory;
            _actionHandlerStateMachine = actionHandlerStateMachine;
            _actionHandlerStatesFactory = actionHandlerStatesFactory;
        }

        public async void Initialize()
        {
            await _worldFactory.CreateSelectFrame();
            await _worldFactory.CreateBuildingMarker();
            WorldGenerator worldGenerator = await _worldFactory.CreateWorldGenerator(null);

            RegisterActionHandlerStates();

            _actionHandlerStateMachine.Enter<NewBuildingPlacePositionHandler>();

            await _worldChanger.Generate(worldGenerator);
            worldGenerator.TestInspect(_worldChanger);

            _worldStateMachine.RegisterState(_statesFactory.Create<ChangeWorldState>());
            _worldStateMachine.RegisterState(_statesFactory.Create<ExitWorldState>());
        }

        private void RegisterActionHandlerStates()
        {
            _actionHandlerStateMachine.RegisterState(_actionHandlerStatesFactory.CreateHandlerState<NewBuildingPlacePositionHandler>());
            _actionHandlerStateMachine.RegisterState(_actionHandlerStatesFactory.CreateHandlerState<RemovedBuildingPositionHandler>());
            _actionHandlerStateMachine.RegisterState(_actionHandlerStatesFactory.CreateHandlerState<ReplacedBuildingPositionHandler>());
        }
    }
}
