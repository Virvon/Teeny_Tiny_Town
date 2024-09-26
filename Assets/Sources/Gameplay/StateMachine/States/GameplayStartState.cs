﻿using Assets.Sources.Services.StateMachine;
using Assets.Sources.Services.StaticDataService.Configs.Windows;
using Assets.Sources.UI.Windows;
using Cysharp.Threading.Tasks;

namespace Assets.Sources.Gameplay.StateMachine.States
{
    public class GameplayStartState : IState
    {
        private readonly WindowsSwitcher _windowsSwitcher;

        public GameplayStartState(WindowsSwitcher windowsSwitcher)
        {
            _windowsSwitcher = windowsSwitcher;
        }

        public UniTask Enter()
        {
            _windowsSwitcher.Switch(WindowType.StartWindow);

            return default;
        }

        public UniTask Exit()
        {
            return default;
        }
    }
}
