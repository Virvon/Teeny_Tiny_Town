using Assets.Sources.Gameplay.World.RepresentationOfWorld.ActionHandler;
using Assets.Sources.Gameplay.World.StateMachine;
using Assets.Sources.Gameplay.World.StateMachine.States;
using Assets.Sources.Gameplay.World.WorldInfrastructure.NextBuildingForPlacing;
using Assets.Sources.Gameplay.World.WorldInfrastructure.WorldChangers;
using Assets.Sources.Infrastructure.Factories.GameplayFactory;
using Assets.Sources.Infrastructure.Factories.WorldFactory;
using Assets.Sources.Sandbox.ActionHandler;
using Assets.Sources.Services.PersistentProgress;
using Assets.Sources.Services.StateMachine;
using Cysharp.Threading.Tasks;

namespace Assets.Sources.Gameplay.World.Root
{
    public class CurrencyWorldBootstrapper : WorldBootstrapper
    {
        private readonly IGameplayFactory _gameplayFactory;

        public CurrencyWorldBootstrapper(
            IWorldChanger worldChanger,
            IWorldFactory worldFactory,
            WorldStateMachine worldStateMachine,
            StatesFactory statesFactory,
            World world,
            ActionHandlerStateMachine actionHandlerStateMachine,
            ActionHandlerStatesFactory actionHandlerStatesFactory,
            NextBuildingForPlacingCreator nextBuildingForPlacingCreator,
            IPersistentProgressService persistentProgressService,
            IGameplayFactory gameplayFactory)
            : base(
                  worldChanger,
                  worldFactory,
                  worldStateMachine,
                  statesFactory,
                  world,
                  actionHandlerStateMachine,
                  actionHandlerStatesFactory,
                  nextBuildingForPlacingCreator,
                  persistentProgressService)
        {
            _gameplayFactory = gameplayFactory;
        }

        public override void Initialize()
        {
            base.Initialize();
            _gameplayFactory.CreateWorldWalletSoundPlayer();
        }

        protected override void RegisterStates(bool needRegisterWaitinState)
        {
            WorldStateMachine.RegisterState(StatesFactory.Create<WorldStartState>());
            WorldStateMachine.RegisterState(StatesFactory.Create<WorldChangingState>());
            WorldStateMachine.RegisterState(StatesFactory.Create<ExitWorldState>());
            WorldStateMachine.RegisterState(StatesFactory.Create<ResultState>());
            WorldStateMachine.RegisterState(StatesFactory.Create<RewardState>());
            WorldStateMachine.RegisterState(StatesFactory.Create<QuestsState>());
            WorldStateMachine.RegisterState(StatesFactory.Create<SafeGameplayState>());
            WorldStateMachine.RegisterState(StatesFactory.Create<StoreState>());
            WorldStateMachine.RegisterState(StatesFactory.Create<GainBuyingState>());

            if (needRegisterWaitinState)
                WorldStateMachine.RegisterState(StatesFactory.Create<WaitingState>());
        }

        protected override void OnWorldEntered() =>
            WorldStateMachine.Enter<WorldStartState>().Forget();
    }
}
