using Assets.Sources.Gameplay.World.RepresentationOfWorld.Markers;
using Assets.Sources.Services.StateMachine;
using Assets.Sources.UI;
using Assets.Sources.UI.Windows.World;
using Cysharp.Threading.Tasks;

namespace Assets.Sources.Gameplay.World.StateMachine.States
{
    public class SafeGameplayState : IState
    {
        private readonly WindowsSwitcher _windowsSwitcher;
        private readonly MarkersVisibility _markersVisibility;

        public SafeGameplayState(WindowsSwitcher windowsSwitcher, MarkersVisibility markersVisibility)
        {
            _windowsSwitcher = windowsSwitcher;
            _markersVisibility = markersVisibility;
        }

        public async UniTask Enter()
        {
            _markersVisibility.ChangeAllowedVisibility(false);
            await _windowsSwitcher.Switch<SaveGameplayWindow>();
        }

        public UniTask Exit() =>
            default;
    }
}
