using System;
using Assets.Sources.Services.Input;
using Assets.Sources.Services.SaveLoadProgress;
using Assets.Sources.Services.SceneManagment;
using Assets.Sources.Services.StateMachine;
using Cysharp.Threading.Tasks;

namespace Assets.Sources.Infrastructure.GameStateMachine.States
{
    public class SandboxState : IState
    {
        private readonly ISceneLoader _sceneLoader;
        private readonly IInputService _inputService;
        private readonly ISaveLoadService _saveLoadService;

        public SandboxState(ISceneLoader sceneLoader, IInputService inputService, ISaveLoadService saveLoadService)
        {
            _sceneLoader = sceneLoader;
            _inputService = inputService;
            _saveLoadService = saveLoadService;
        }

        public UniTask Enter()
        {
            _sceneLoader.Load(InfrastructureAssetPath.SandboxScene);
            _inputService.SetEnabled(true);

            return default;
        }

        public UniTask Exit()
        {
            _inputService.SetEnabled(false);
            _saveLoadService.SaveProgress();
            return default;
        }
    }
}
