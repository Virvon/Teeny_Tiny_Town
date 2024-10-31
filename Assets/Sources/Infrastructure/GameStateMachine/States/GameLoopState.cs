using Assets.Sources.Services.SceneManagment;
using Assets.Sources.Services.StateMachine;
using Cysharp.Threading.Tasks;

namespace Assets.Sources.Infrastructure.GameStateMachine.States
{
    public class GameLoopState : IState
    {
        private readonly ISceneLoader _sceneLoader;

        public GameLoopState(ISceneLoader sceneLoader) =>
            _sceneLoader = sceneLoader;

        public UniTask Enter()
        {
            _sceneLoader.Load(InfrastructureAssetPath.GameplayScene);

            return default;
        }

        public UniTask Exit() =>
            default;
    }
    public class SandboxState : IState
    {
        private readonly ISceneLoader _sceneLoader;

        public SandboxState(ISceneLoader sceneLoader) =>
            _sceneLoader = sceneLoader;

        public UniTask Enter()
        {
            _sceneLoader.Load(InfrastructureAssetPath.SandboxScene);

            return default;
        }

        public UniTask Exit() =>
            default;
    }
}
