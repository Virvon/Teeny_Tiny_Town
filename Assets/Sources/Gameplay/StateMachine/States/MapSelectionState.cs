﻿using Assets.Sources.Gameplay.Cameras;
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

        public async UniTask Enter()
        {
            await _windowsSwitcher.Switch<MapSelectionWindow>();
            _camera.MoveTo(new Vector3(60.9f, 93.1f, -60.9f));
        }

        public UniTask Exit()
        {
            return default;
        }
    }
}
