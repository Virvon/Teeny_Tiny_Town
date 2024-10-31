using Assets.Sources.Services.Input;
using Assets.Sources.Services.SceneManagment;
using Assets.Sources.Services.StateMachine;
using Cysharp.Threading.Tasks;

namespace Assets.Sources.Infrastructure.GameStateMachine.States
{
    public class SandboxState : IState
    {
        private readonly ISceneLoader _sceneLoader;
        private readonly IInputService _inputService;

        public SandboxState(ISceneLoader sceneLoader, IInputService inputService)
        {
            _sceneLoader = sceneLoader;
            _inputService = inputService;
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
            return default;
        }
    }
}
