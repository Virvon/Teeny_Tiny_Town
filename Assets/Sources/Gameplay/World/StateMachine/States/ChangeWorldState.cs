using Assets.Sources.Gameplay.World.RepresentationOfWorld.ActionHandler;
using Assets.Sources.Services.Input;
using Assets.Sources.Services.StateMachine;
using Assets.Sources.Services.StaticDataService.Configs.Windows;
using Assets.Sources.UI.Windows;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Assets.Sources.Gameplay.World.StateMachine.States
{
    public class ChangeWorldState : IState
    {
        private readonly IInputService _inputService;
        private readonly WindowsSwitcher _windowsSwitcher;
        private readonly ActionHandlerStateMachine _actionHandlerStateMachine;

        public ChangeWorldState(IInputService inputService, WindowsSwitcher windowsSwitcher, ActionHandlerStateMachine actionHandlerStateMachine)
        {
            _inputService = inputService;
            _windowsSwitcher = windowsSwitcher;
            _actionHandlerStateMachine = actionHandlerStateMachine;
        }

        public UniTask Enter()
        {
            _actionHandlerStateMachine.SetActive(true);
            _windowsSwitcher.Switch(WindowType.GameplayWindow);
            _inputService.SetEnabled(true);
            return default;
        }

        public UniTask Exit()
        {
            _inputService.SetEnabled(false);
            return default;
        }
    }
}
