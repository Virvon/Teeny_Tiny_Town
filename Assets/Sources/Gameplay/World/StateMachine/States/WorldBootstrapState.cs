using Assets.Sources.Gameplay.World.RepresentationOfWorld.ActionHandler;
using Assets.Sources.Gameplay.World.WorldInfrastructure.WorldChangers;
using Assets.Sources.Infrastructure.Factories.WorldFactory;
using Assets.Sources.Services.StateMachine;
using Cysharp.Threading.Tasks;

namespace Assets.Sources.Gameplay.World.StateMachine.States
{
    public class WorldBootstrapState : IState
    {
        private readonly IWorldFactory _worldFactory;
        private readonly ActionHandlerStateMachine _actionHandlerStateMachine;
        private readonly ActionHandlerStatesFactory _actionHandlerStatesFactory;
        private readonly IWorldChanger _worldChanger;

        protected readonly WorldStateMachine WorldStateMachine;

        public WorldBootstrapState(
            IWorldFactory worldFactory,
            ActionHandlerStateMachine actionHandlerStateMachine,
            ActionHandlerStatesFactory actionHandlerStatesFactory,
            IWorldChanger worldChanger,
            WorldStateMachine worldStateMachine)
        {
            _worldFactory = worldFactory;
            _actionHandlerStateMachine = actionHandlerStateMachine;
            _actionHandlerStatesFactory = actionHandlerStatesFactory;
            _worldChanger = worldChanger;
            WorldStateMachine = worldStateMachine;
        }

        public async UniTask Enter()
        {
            await _worldFactory.CreateSelectFrame();
            await _worldFactory.CreateBuildingMarker();
            await _worldFactory.CreateActionHandlerSwitcher();

            RegisterActionHandlerStates();

            _actionHandlerStateMachine.Enter<NewBuildingPlacePositionHandler>();

            _worldChanger.Start();
            EnterNextState();
        }

        public UniTask Exit()
        {
            return default;
        }

        protected virtual void EnterNextState() =>
            WorldStateMachine.Enter<WorldChangingState>().Forget();

        private void RegisterActionHandlerStates()
        {
            _actionHandlerStateMachine.RegisterState(_actionHandlerStatesFactory.CreateHandlerState<NewBuildingPlacePositionHandler>());
            _actionHandlerStateMachine.RegisterState(_actionHandlerStatesFactory.CreateHandlerState<RemovedBuildingPositionHandler>());
            _actionHandlerStateMachine.RegisterState(_actionHandlerStatesFactory.CreateHandlerState<ReplacedBuildingPositionHandler>());
        }
    }
}
