using Assets.Sources.Gameplay.World.RepresentationOfWorld.ActionHandler;
using Assets.Sources.Gameplay.World.WorldInfrastructure;
using Assets.Sources.Infrastructure.Factories.WorldFactory;
using Assets.Sources.Services.StateMachine;
using Cysharp.Threading.Tasks;

namespace Assets.Sources.Gameplay.World.StateMachine.States
{
    public class WorldBootsrappState : IState
    {
        private readonly IWorldFactory _worldFactory;
        private readonly ActionHandlerStateMachine _actionHandlerStateMachine;
        private readonly ActionHandlerStatesFactory _actionHandlerStatesFactory;
        private readonly WorldChanger _worldChanger;
        private readonly WorldStateMachine _worldStateMachine;

        public WorldBootsrappState(
            IWorldFactory worldFactory,
            ActionHandlerStateMachine actionHandlerStateMachine,
            ActionHandlerStatesFactory actionHandlerStatesFactory,
            WorldChanger worldChanger,
            WorldStateMachine worldStateMachine)
        {
            _worldFactory = worldFactory;
            _actionHandlerStateMachine = actionHandlerStateMachine;
            _actionHandlerStatesFactory = actionHandlerStatesFactory;
            _worldChanger = worldChanger;
            _worldStateMachine = worldStateMachine;
        }

        public async UniTask Enter()
        {
            await _worldFactory.CreateSelectFrame();
            await _worldFactory.CreateBuildingMarker();
            await _worldFactory.CreateActionHandlerSwitcher();

            RegisterActionHandlerStates();

            _actionHandlerStateMachine.Enter<NewBuildingPlacePositionHandler>();

            _worldChanger.Start();

            _worldStateMachine.Enter<WorldChangingState>().Forget();
        }

        public UniTask Exit()
        {
            return default;
        }

        private void RegisterActionHandlerStates()
        {
            _actionHandlerStateMachine.RegisterState(_actionHandlerStatesFactory.CreateHandlerState<NewBuildingPlacePositionHandler>());
            _actionHandlerStateMachine.RegisterState(_actionHandlerStatesFactory.CreateHandlerState<RemovedBuildingPositionHandler>());
            _actionHandlerStateMachine.RegisterState(_actionHandlerStatesFactory.CreateHandlerState<ReplacedBuildingPositionHandler>());
        }
    }
}
