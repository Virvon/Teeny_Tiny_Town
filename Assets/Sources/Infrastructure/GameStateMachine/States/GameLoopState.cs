using Assets.Sources.Services.PersistentProgress;
using Assets.Sources.Services.SceneManagment;
using Assets.Sources.Services.StateMachine;
using Assets.Sources.Services.StaticDataService;
using Cysharp.Threading.Tasks;

namespace Assets.Sources.Infrastructure.GameStateMachine.States
{
    public class GameLoopState : IState
    {
        private readonly ISceneLoader _sceneLoader;
        private readonly IPersistentProgressService _persistentProgressService;
        private readonly IStaticDataService _staticDataService;

        public GameLoopState(ISceneLoader sceneLoader, IPersistentProgressService persistentProgressService, IStaticDataService staticDataService)
        {
            _sceneLoader = sceneLoader;
            _persistentProgressService = persistentProgressService;
            _staticDataService = staticDataService;
        }

        public UniTask Enter()
        {
            uint availableMovesCount = _staticDataService.WorldsConfig.AvailableMovesCount;

            if (_persistentProgressService.Progress.GameplayMovesCounter.RemainingMovesCount > availableMovesCount)
                _persistentProgressService.Progress.GameplayMovesCounter.SetCount(availableMovesCount);

            _sceneLoader.Load(InfrastructureAssetPath.GameplayScene);

            return default;
        }

        public UniTask Exit() =>
            default;
    }
}