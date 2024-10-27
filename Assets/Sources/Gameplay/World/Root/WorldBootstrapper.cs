using Assets.Sources.Data.WorldDatas;
using Assets.Sources.Gameplay.World.RepresentationOfWorld;
using Assets.Sources.Gameplay.World.RepresentationOfWorld.ActionHandler;
using Assets.Sources.Gameplay.World.StateMachine;
using Assets.Sources.Gameplay.World.StateMachine.States;
using Assets.Sources.Gameplay.World.WorldInfrastructure.NextBuildingForPlacing;
using Assets.Sources.Gameplay.World.WorldInfrastructure.WorldChangers;
using Assets.Sources.Infrastructure.Factories.WorldFactory;
using Assets.Sources.Services.PersistentProgress;
using Assets.Sources.Services.StateMachine;
using Cysharp.Threading.Tasks;
using Zenject;

namespace Assets.Sources.Gameplay.World.Root
{
    public class WorldBootstrapper : IInitializable
    {
        private readonly IWorldChanger _worldChanger;
        private readonly IWorldFactory _worldFactory;
        private readonly IWorldData _worldData;
        private readonly World _world;
        private readonly ActionHandlerStateMachine _actionHandlerStateMachine;
        private readonly ActionHandlerStatesFactory _actionHandlerStatesFactory;
        private readonly NextBuildingForPlacingCreator _nextBuildingForPlacingCreator;
        private readonly IPersistentProgressService _persistentProgressService;

        protected readonly WorldStateMachine WorldStateMachine;
        protected readonly StatesFactory StatesFactory;

        public WorldBootstrapper(
            IWorldChanger worldChanger,
            IWorldFactory worldFactory,
            WorldStateMachine worldStateMachine,
            StatesFactory statesFactory,
            IWorldData worldData,
            World world,
            ActionHandlerStateMachine actionHandlerStateMachine,
            ActionHandlerStatesFactory actionHandlerStatesFactory,
            NextBuildingForPlacingCreator nextBuildingForPlacingCreator,
            IPersistentProgressService persistentProgressService,
            WorldWindows worldWindows)
        {
            _worldChanger = worldChanger;
            _worldFactory = worldFactory;
            WorldStateMachine = worldStateMachine;
            StatesFactory = statesFactory;
            _worldData = worldData;
            _world = world;

            _world.Entered += OnWorldEntered;
            _actionHandlerStateMachine = actionHandlerStateMachine;
            _actionHandlerStatesFactory = actionHandlerStatesFactory;
            _nextBuildingForPlacingCreator = nextBuildingForPlacingCreator;
            _persistentProgressService = persistentProgressService;
        }

        ~WorldBootstrapper() =>
            _world.Entered -= OnWorldEntered;

        public async void Initialize()
        {
            WorldGenerator worldGenerator = await _worldFactory.CreateWorldGenerator();

            worldGenerator.PlaceToCenter(_worldData.Size);
            await _worldChanger.Generate(worldGenerator);


            await _worldFactory.CreateSelectFrame();
            await _worldFactory.CreateBuildingMarker();
            await _worldFactory.CreateActionHandlerSwitcher();

            RegisterActionHandlerStates();
            RegisterStates();

            _actionHandlerStateMachine.Enter<NewBuildingPlacePositionHandler>();

            _nextBuildingForPlacingCreator.CreateData(_worldChanger.Tiles);

            _worldChanger.Start();
        }

        protected virtual void RegisterStates()
        {
            WorldStateMachine.RegisterState(StatesFactory.Create<WorldStartState>());
            WorldStateMachine.RegisterState(StatesFactory.Create<WorldChangingState>());
            WorldStateMachine.RegisterState(StatesFactory.Create<ExitWorldState>());
            WorldStateMachine.RegisterState(StatesFactory.Create<ResultState>());
            WorldStateMachine.RegisterState(StatesFactory.Create<RewardState>());
            WorldStateMachine.RegisterState(StatesFactory.Create<QuestsState>());
            WorldStateMachine.RegisterState(StatesFactory.Create<SaveGameplayState>());
            
            if(_persistentProgressService.Progress.StoreData.IsInfinityMovesUnlocked == false)
                WorldStateMachine.RegisterState(StatesFactory.Create<WaitingState>());
        }

        protected virtual void OnWorldEntered() =>
            WorldStateMachine.Enter<WorldStartState>().Forget();

        private void RegisterActionHandlerStates()
        {
            _actionHandlerStateMachine.RegisterState(_actionHandlerStatesFactory.CreateHandlerState<NewBuildingPlacePositionHandler>());
            _actionHandlerStateMachine.RegisterState(_actionHandlerStatesFactory.CreateHandlerState<RemovedBuildingPositionHandler>());
            _actionHandlerStateMachine.RegisterState(_actionHandlerStatesFactory.CreateHandlerState<ReplacedBuildingPositionHandler>());
        }
    }
}
