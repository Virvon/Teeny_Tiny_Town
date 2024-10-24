using Assets.Sources.Gameplay.StateMachine;
using Assets.Sources.Services.StateMachine;
using Assets.Sources.Services.StaticDataService.Configs.Windows;
using Assets.Sources.UI;
using Cysharp.Threading.Tasks;
using System;

namespace Assets.Sources.Gameplay.World.StateMachine.States
{
    public class ExitWorldState : IPayloadState<Action>
    {
        private readonly GameplayStateMachine _gameplayStateMachine;
        private readonly WindowsSwitcher _windowsSwitcher;

        public ExitWorldState(GameplayStateMachine gameplayStateMachine, WindowsSwitcher windowsSwitcher)
        {
            _gameplayStateMachine = gameplayStateMachine;
            _windowsSwitcher = windowsSwitcher;
        }

        public UniTask Enter(Action callbakc)
        {
            callbakc?.Invoke();

            return default;
        }

        public UniTask Exit()
        {
            //_windowsSwitcher.Remove(WindowType.GameplayWindow);
            return default;
        }
    }
}
