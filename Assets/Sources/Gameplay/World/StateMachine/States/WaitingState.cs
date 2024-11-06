using Assets.Sources.Services.StateMachine;
using Assets.Sources.UI;
using Assets.Sources.UI.Windows.World;
using Cysharp.Threading.Tasks;
using System;

namespace Assets.Sources.Gameplay.World.StateMachine.States
{
    public class WaitingState : IState
    {
        private readonly WindowsSwitcher _windowsSwintcher;

        public WaitingState(WindowsSwitcher windowsSwintcher)
        {
            _windowsSwintcher = windowsSwintcher;
        }

        public async UniTask Enter()
        {
            await _windowsSwintcher.Switch<WaitingWindow>();
        }

        public UniTask Exit()
        {
            return default;
        }
    }
}
