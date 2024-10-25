using Assets.Sources.Services.StateMachine;
using Cysharp.Threading.Tasks;
using System;

namespace Assets.Sources.Gameplay.World.StateMachine.States
{
    public class ExitWorldState : IPayloadState<Action>
    {
        private readonly WorldWindows _worldWindows;

        public ExitWorldState(WorldWindows worldWindows) =>
            _worldWindows = worldWindows;

        public UniTask Enter(Action callbakc)
        {
            _worldWindows.Remove();
            callbakc?.Invoke();

            return default;
        }

        public UniTask Exit() =>
            default;
    }
}
