using Assets.Sources.Data.WorldDatas;
using Assets.Sources.Gameplay.World.RepresentationOfWorld.ActionHandler;
using Assets.Sources.Gameplay.World.StateMachine;
using Assets.Sources.Gameplay.World.StateMachine.States;
using Assets.Sources.Gameplay.World.WorldInfrastructure.NextBuildingForPlacing;
using Assets.Sources.Gameplay.World.WorldInfrastructure.WorldChangers;
using Assets.Sources.Infrastructure.Factories.UiFactory;
using Assets.Sources.Infrastructure.Factories.WorldFactory;
using Assets.Sources.Services.StateMachine;
using Assets.Sources.UI;
using Cysharp.Threading.Tasks;

namespace Assets.Sources.Gameplay.World.Root
{
    public class CurrencyWorldBootstrapper : WorldBootstrapper
    {
        public CurrencyWorldBootstrapper(
            IWorldChanger worldChanger,
            IWorldFactory worldFactory,
            WorldStateMachine worldStateMachine,
            StatesFactory statesFactory,
            IWorldData worldData,
            World world,
            ActionHandlerStateMachine actionHandlerStateMachine,
            ActionHandlerStatesFactory actionHandlerStatesFactory,
            NextBuildingForPlacingCreator nextBuildingForPlacingCreator,
            WindowsSwitcher windowsSwitcher,
            IUiFactory uiFactory)
            : base(
                  worldChanger,
                  worldFactory,
                  worldStateMachine,
                  statesFactory,
                  worldData,
                  world,
                  actionHandlerStateMachine,
                  actionHandlerStatesFactory,
                  nextBuildingForPlacingCreator,
                  windowsSwitcher,
                  uiFactory)
        {
        }

        protected override void RegisterStates()
        {
            WorldStateMachine.RegisterState(StatesFactory.Create<StoreState>());
            WorldStateMachine.RegisterState(StatesFactory.Create<ExitWorldState>());
            WorldStateMachine.RegisterState(StatesFactory.Create<ResultState>());
        }

        protected override void OnWorldEntered() =>
            WorldStateMachine.Enter<WorldStartState>().Forget();
    }
}
