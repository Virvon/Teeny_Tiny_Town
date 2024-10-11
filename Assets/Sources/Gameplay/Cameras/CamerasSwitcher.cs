using Assets.Sources.Infrastructure.Factories.GameplayFactory;
using Assets.Sources.Services.StaticDataService;
using Assets.Sources.Services.StaticDataService.Configs.Camera;
using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Sources.Gameplay.Cameras
{
    public class CamerasSwitcher
    {
        private const int SelectedCameraPriorityValue = 1;
        private const int UnSelectedCameraPriorityValue = 0;
        private const GameplayCameraType SwitchedAtStartCameraType = GameplayCameraType.BootstrapCamera;

        private readonly IStaticDataService _staticDataService;
        private readonly IGameplayFactory _gameplayFactory;

        private Dictionary<GameplayCameraType, GameplayCamera> _cameras;
        private GameplayCamera _currentCamera;

        public CamerasSwitcher(IStaticDataService staticDataService, IGameplayFactory gameplayFactory)
        {
            _staticDataService = staticDataService;
            _gameplayFactory = gameplayFactory;

            _cameras = new();
        }

        public async UniTask CreateCameras()
        {
            foreach(GameplayCameraConfig config in _staticDataService.CameraConfigs)
                _cameras.Add(config.Type, await _gameplayFactory.CreateCamera(config.Type));

            Switch(SwitchedAtStartCameraType);
        }

        public void Switch(GameplayCameraType type)
        {
            _currentCamera?.SetPriority(UnSelectedCameraPriorityValue);
            _currentCamera = _cameras[type];
            _currentCamera.SetPriority(SelectedCameraPriorityValue);
        }
    }
}
