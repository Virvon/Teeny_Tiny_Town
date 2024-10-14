using Assets.Sources.Data;
using Assets.Sources.Gameplay.World.RepresentationOfWorld;
using Assets.Sources.Gameplay.World.StateMachine;
using Assets.Sources.Gameplay.World.StateMachine.States;
using Assets.Sources.Gameplay.World.WorldInfrastructure.WorldChangers;
using Assets.Sources.Infrastructure.Factories.WorldFactory;
using Assets.Sources.Services.StateMachine;
using Zenject;

namespace Assets.Sources.Gameplay.World.Root
{
    public class WorldBootstrapper : IInitializable
    {
        private readonly IWorldChanger _worldChanger;
        private readonly IWorldFactory _worldFactory;
        private readonly WorldStateMachine _worldStateMachine;
        private readonly StatesFactory _statesFactory;
        private readonly IWorldData _worldData;

        public WorldBootstrapper(
            IWorldChanger worldChanger,
            IWorldFactory worldFactory,
            WorldStateMachine worldStateMachine,
            StatesFactory statesFactory,
            IWorldData worldData)
        {
            _worldChanger = worldChanger;
            _worldFactory = worldFactory;
            _worldStateMachine = worldStateMachine;
            _statesFactory = statesFactory;
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
    }
}
