using Assets.Sources.Services.SceneManagment;
using Assets.Sources.Services.StateMachine;
using Cysharp.Threading.Tasks;
using System;

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

        public UniTask Exit()
        {
            
            return default;
        }
    }
}
