using Assets.Sources.Gameplay.World.RepresentationOfWorld.Markers;
using Assets.Sources.Services.StateMachine;
using Assets.Sources.UI;
using Assets.Sources.UI.Windows.World;
using Cysharp.Threading.Tasks;

namespace Assets.Sources.Gameplay.World.StateMachine.States
{
    public class SaveGameplayState : IState
    {
        private readonly WindowsSwitcher _windowsSwitcher;
        private readonly MarkersVisibility _markersVisibility;

        public SaveGameplayState(WindowsSwitcher windowsSwitcher, MarkersVisibility markersVisibility)
        {
            _windowsSwitcher = windowsSwitcher;
            _markersVisibility = markersVisibility;
        }

        public UniTask Enter()
        {
            _markersVisibility.ChangeAllowedVisibility(false);
            _windowsSwitcher.Switch<SaveGameplayWindow>();

            return default;
        }

        public UniTask Exit() =>
            default;
    }
}
