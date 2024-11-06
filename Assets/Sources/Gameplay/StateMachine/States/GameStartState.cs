using Assets.Sources.Gameplay.Cameras;
using Assets.Sources.Services.AssetManagement;
using Assets.Sources.Services.PersistentProgress;
using Assets.Sources.Services.StateMachine;
using Assets.Sources.UI;
using Assets.Sources.UI.Windows.Start;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Assets.Sources.Gameplay.StateMachine.States
{
    public class GameStartState : IState
    {
        private readonly WindowsSwitcher _windowsSwitcher;
        private readonly GameplayCamera _camera;
        private readonly IAssetProvider _assetProvider;
        private readonly IPersistentProgressService _persistentProgressService;
        private readonly GameplayStateMachine _gameplayStateMachine;

        public GameStartState(WindowsSwitcher windowsSwitcher, GameplayCamera camera, IAssetProvider assetProvider, IPersistentProgressService persistentProgressService, GameplayStateMachine gameplayStateMachine)
        {
            _windowsSwitcher = windowsSwitcher;
            _camera = camera;
            _assetProvider = assetProvider;
            _persistentProgressService = persistentProgressService;
            _gameplayStateMachine = gameplayStateMachine;
        }

        public async UniTask Enter()
        {
            if(_persistentProgressService.Progress.IsEducationCompleted)
            {
                await _assetProvider.WarmupAssetsByLable(AssetLabels.Gameplay);
                await _windowsSwitcher.Switch<StartWindow>();
                _camera.MoveTo(new Vector3(67.3f, 93.1f, -67.3f));
            }
            else
            {
                _gameplayStateMachine.Enter<GameplayLoopState, bool>(true).Forget();
            }
        }

        public UniTask Exit()
        {
            return default;
        }
    }
}
