using Assets.Sources.Gameplay.World.RepresentationOfWorld.ActionHandler;
using Assets.Sources.Gameplay.World.RepresentationOfWorld.Markers;
using Assets.Sources.Gameplay.World.Windows;
using Assets.Sources.Services.StateMachine;
using Cysharp.Threading.Tasks;
using System;

namespace Assets.Sources.Gameplay.World.StateMachine.States
{
    public class ExitWorldState : IPayloadState<Action>
    {
        private readonly IWorldWindows _worldWindows;
        private readonly MarkersVisibility _markersVisibility;
        private readonly ActionHandlerSwitcher _actionHandlerSwitcher;

        public ExitWorldState(IWorldWindows worldWindows, MarkersVisibility markersVisibility, ActionHandlerSwitcher actionHandlerSwitcher)
        {
            _worldWindows = worldWindows;
            _markersVisibility = markersVisibility;
            _actionHandlerSwitcher = actionHandlerSwitcher;
        }

        public UniTask Enter(Action callbakc)
        {
            _markersVisibility.ChangeAllowedVisibility(false);
            _actionHandlerSwitcher.EnterToDefaultState();
            _worldWindows.Remove();
            callbakc?.Invoke();

            return default;
        }

        public UniTask Exit() =>
            default;
    }
}
