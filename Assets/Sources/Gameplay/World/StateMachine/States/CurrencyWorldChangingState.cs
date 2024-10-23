using Assets.Sources.Data.WorldDatas;
using Assets.Sources.Gameplay.Cameras;
using Assets.Sources.Gameplay.World.RepresentationOfWorld.ActionHandler;
using Assets.Sources.Infrastructure.Factories.UiFactory;
using Assets.Sources.Services.Input;
using Assets.Sources.Services.StaticDataService.Configs.Windows;
using Assets.Sources.UI.Windows;

namespace Assets.Sources.Gameplay.World.StateMachine.States
{
    public class CurrencyWorldChangingState : WorldChangingState
    {
        public CurrencyWorldChangingState(
            IInputService inputService,
            WindowsSwitcher windowsSwitcher,
            ActionHandlerStateMachine actionHandlerStateMachine,
            IUiFactory uiFactory,
            GameplayCamera gameplayCamera,
            IWorldData worldData)
            : base(inputService, windowsSwitcher, actionHandlerStateMachine, uiFactory, gameplayCamera, worldData)
        {
        }

        protected override WindowType WindowType => WindowType.CurrencyGameplayWindow;
    }
}
