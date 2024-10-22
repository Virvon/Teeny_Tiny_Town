using Assets.Sources.Gameplay.World.RepresentationOfWorld.ActionHandler;
using Assets.Sources.Gameplay.World.WorldInfrastructure.NextBuildingForPlacing;
using Assets.Sources.Gameplay.World.WorldInfrastructure.WorldChangers;
using Assets.Sources.Infrastructure.Factories.WorldFactory;
using Cysharp.Threading.Tasks;

namespace Assets.Sources.Gameplay.World.StateMachine.States
{
    public class CurrencyWorldBootstrapState : WorldBootstrapState
    {
        public CurrencyWorldBootstrapState(
            IWorldFactory worldFactory,
            ActionHandlerStateMachine actionHandlerStateMachine,
            ActionHandlerStatesFactory actionHandlerStatesFactory,
            IWorldChanger worldChanger,
            WorldStateMachine worldStateMachine,
            NextBuildingForPlacingCreator nextBuildingForPlacingCreator)
            : base(
                  worldFactory,
                  actionHandlerStateMachine,
                  actionHandlerStatesFactory,
                  worldChanger,
                  worldStateMachine,
                  nextBuildingForPlacingCreator)
        {
        }

        protected override void EnterNextState() =>
            WorldStateMachine.Enter<CurrencyWorldChangingState>().Forget();
    }
}
