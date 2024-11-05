using Assets.Sources.Data.World;
using Assets.Sources.Gameplay.World.StateMachine;
using Assets.Sources.Gameplay.World.StateMachine.States;
using Assets.Sources.Services.Input;
using Assets.Sources.Services.StaticDataService.Configs.WorldStore;
using Cysharp.Threading.Tasks;

namespace Assets.Sources.Gameplay.World.RepresentationOfWorld.ActionHandler
{
    public class CurrencyWorldActionHandlerSwitcher : ActionHandlerSwitcher
    {
        private readonly WorldStateMachine _worldStateMachine;

        public CurrencyWorldActionHandlerSwitcher(
            ActionHandlerStateMachine handlerStateMachine,
            WorldRepresentationChanger worldRepresentationChanger,
            IInputService inputService,
            IWorldData worldData,
            WorldStateMachine worldStateMachine)
            : base(handlerStateMachine, worldRepresentationChanger, inputService, worldData)
        {
            _worldStateMachine = worldStateMachine;
        }

        protected override bool CheckBulldozerItemsCount()
        {
            bool isEnoughItems = base.CheckBulldozerItemsCount();

            if (isEnoughItems == false)
                _worldStateMachine.Enter<GainBuyingState, GainStoreItemType>(GainStoreItemType.Bulldozer).Forget();

            return isEnoughItems;
        }

        protected override bool CheckReplaceItemsCount()
        {
            bool isEnoughItems = base.CheckReplaceItemsCount();

            if (isEnoughItems == false)
                _worldStateMachine.Enter<GainBuyingState, GainStoreItemType>(GainStoreItemType.ReplaceItems).Forget();

            return isEnoughItems;
        }
    }
}
