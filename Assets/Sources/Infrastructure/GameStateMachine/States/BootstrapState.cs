using Assets.Sources.Services.AssetManagement;
using Assets.Sources.Services.StateMachine;
using Assets.Sources.Services.StaticDataService;
using Cysharp.Threading.Tasks;

namespace Assets.Sources.Infrastructure.GameStateMachine.States
{
    public class BootstrapState : IState
    {
        private readonly GameStateMachine _gameStateMachine;
        private readonly IAssetProvider _assetProvider;
        private readonly IStaticDataService _staticDataService;

        public BootstrapState(GameStateMachine gameStateMachine, IAssetProvider assetProvider, IStaticDataService staticDataService)
        {
            _gameStateMachine = gameStateMachine;
            _assetProvider = assetProvider;
            _staticDataService = staticDataService;
        }

        public async UniTask Enter()
        {
            await Initialize();

            _gameStateMachine.Enter<GameLoopState>().Forget();
        }

        private async UniTask Initialize()
        {
            await _assetProvider.InitializeAsync();
            await _staticDataService.InitializeAsync();
        }

        public UniTask Exit() =>
            default;
    }
}
