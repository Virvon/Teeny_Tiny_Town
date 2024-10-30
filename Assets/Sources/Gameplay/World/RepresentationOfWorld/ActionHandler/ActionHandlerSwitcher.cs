using Assets.Sources.Data.WorldDatas;
using Assets.Sources.Services.Input;

namespace Assets.Sources.Gameplay.World.RepresentationOfWorld.ActionHandler
{
    public class ActionHandlerSwitcher : IActionHandlerSwitcher
    {
        private readonly WorldRepresentationChanger _worldRepresentationChanger;
        private readonly ActionHandlerStateMachine _handlerStateMachine;
        private readonly IInputService _inputService;
        private readonly IWorldData WorldData;

        public ActionHandlerSwitcher(
            ActionHandlerStateMachine handlerStateMachine,
            WorldRepresentationChanger worldRepresentationChanger,
            IInputService inputService,
            IWorldData worldData)
        {
            _handlerStateMachine = handlerStateMachine;
            _worldRepresentationChanger = worldRepresentationChanger;
            _inputService = inputService;
            WorldData = worldData;

            _inputService.RemoveBuildingButtonPressed += OnRemoveBuildingButtonClicked;
            _inputService.ReplaceBuildingButtonPressed += OnReplaceBuildingButtonClicked;
            _worldRepresentationChanger.GameplayMoved += EnterToDefaultState;
        }

        ~ActionHandlerSwitcher()
        {
            _inputService.RemoveBuildingButtonPressed -= OnRemoveBuildingButtonClicked;
            _inputService.ReplaceBuildingButtonPressed -= OnReplaceBuildingButtonClicked;
            _worldRepresentationChanger.GameplayMoved -= EnterToDefaultState;
        }

        public void EnterToDefaultState()
        {
            if (_handlerStateMachine.CurrentState is not NewBuildingPlacePositionHandler)
                _handlerStateMachine.Enter<NewBuildingPlacePositionHandler>();
        }

        protected virtual bool CheckBulldozerItemsCount() =>
            WorldData.BulldozerItems.Count != 0;

        protected virtual bool CheckReplaceItemsCount() =>
            WorldData.ReplaceItems.Count != 0;

        private void OnReplaceBuildingButtonClicked()
        {
            if (CheckReplaceItemsCount() == false)
                return;

            if (_handlerStateMachine.CurrentState is not ReplacedBuildingPositionHandler)
                _handlerStateMachine.Enter<ReplacedBuildingPositionHandler>();
            else
                _handlerStateMachine.Enter<NewBuildingPlacePositionHandler>();
        }

        private void OnRemoveBuildingButtonClicked()
        {
            if (CheckBulldozerItemsCount() == false)
                return;

            if (_handlerStateMachine.CurrentState is not RemovedBuildingPositionHandler)
                _handlerStateMachine.Enter<RemovedBuildingPositionHandler>();
            else
                _handlerStateMachine.Enter<NewBuildingPlacePositionHandler>();
        }


    }
}
