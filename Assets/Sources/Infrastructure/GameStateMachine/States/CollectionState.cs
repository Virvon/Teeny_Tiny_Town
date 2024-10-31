using Assets.Sources.Services.SceneManagment;
using Assets.Sources.Services.StateMachine;
using Cysharp.Threading.Tasks;

namespace Assets.Sources.Infrastructure.GameStateMachine.States
{
    public class CollectionState : IState
    {
        private readonly ISceneLoader _sceneLoader;

        public CollectionState(ISceneLoader sceneLoader)
        {
            _sceneLoader = sceneLoader;
        }

        public UniTask Enter()
        {
            _sceneLoader.Load(InfrastructureAssetPath.CollectionScene);

            return default;
        }

        public UniTask Exit()
        {
            return default;
        }
    }
}
