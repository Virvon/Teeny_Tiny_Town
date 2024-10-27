using Assets.Sources.Gameplay.World.RepresentationOfWorld.Markers;
using Assets.Sources.Services.StateMachine;
using Cysharp.Threading.Tasks;
using System;

namespace Assets.Sources.Gameplay.World.StateMachine.States
{
    public class ExitWorldState : IPayloadState<Action>
    {
        private readonly WorldWindows _worldWindows;
        private readonly MarkersVisibility _markersVisibility;

        public ExitWorldState(WorldWindows worldWindows, MarkersVisibility markersVisibility)
        {
            _worldWindows = worldWindows;
            _markersVisibility = markersVisibility;
        }

        public UniTask Enter(Action callbakc)
        {
            _markersVisibility.ChangeAllowedVisibility(false);
            _worldWindows.Remove();
            callbakc?.Invoke();

            return default;
        }

        public UniTask Exit() =>
            default;
    }
}
