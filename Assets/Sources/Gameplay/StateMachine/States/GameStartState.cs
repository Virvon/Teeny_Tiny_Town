using Assets.Sources.Gameplay.Cameras;
using Assets.Sources.Services.AssetManagement;
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

        public GameStartState(WindowsSwitcher windowsSwitcher, GameplayCamera camera, IAssetProvider assetProvider)
        {
            _windowsSwitcher = windowsSwitcher;
            _camera = camera;
            _assetProvider = assetProvider;
        }

        public UniTask Enter()
        {
            _assetProvider.WarmupAssetsByLable(AssetLabels.Gameplay);
            _windowsSwitcher.Switch<StartWindow>();
            _camera.MoveTo(new Vector3(67.3f, 93.1f, -67.3f));

            return default;
        }

        public UniTask Exit()
        {
            return default;
        }
    }
}
