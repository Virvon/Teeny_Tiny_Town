using Assets.Sources.Gameplay.Cameras;
using Assets.Sources.Services.StateMachine;
using Assets.Sources.UI;
using Assets.Sources.UI.Windows.MapSelection;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Assets.Sources.Gameplay.StateMachine.States
{
    public class MapSelectionState : IState
    {
        private readonly WindowsSwitcher _windowsSwitcher;
        private readonly GameplayCamera _camera;

        public MapSelectionState(WindowsSwitcher windowsSwitcher, GameplayCamera camera)
        {
            _windowsSwitcher = windowsSwitcher;
            _camera = camera;
        }

        public UniTask Enter()
        {
            _windowsSwitcher.Switch<MapSelectionWindow>("map selection");
            _camera.MoveTo(new Vector3(60.9f, 93.1f, -60.9f));
            return default;
        }

        public UniTask Exit()
        {
            return default;
        }
    }
}
