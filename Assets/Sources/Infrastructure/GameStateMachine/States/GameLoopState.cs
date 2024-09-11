using Assets.Sources.Services.SceneManagment;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Assets.Sources.Infrastructure.GameStateMachine.States
{
    public class GameLoopState : IState
    {
        private readonly ISceneLoader _sceneLoader;

        public GameLoopState(ISceneLoader sceneLoader)
        {
            _sceneLoader = sceneLoader;
        }

        public UniTask Enter()
        {
            _sceneLoader.Load(InfrastructureAssetPath.GameplayScene);

            return default;
        }

        public UniTask Exit()
        {
            return default;
        }
    }
}
