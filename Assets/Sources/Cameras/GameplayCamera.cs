using Assets.Sources.Services.PersistentProgress;
using Assets.Sources.Services.StaticDataService;
using Assets.Sources.Services.StaticDataService.Configs;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using Zenject;

namespace Assets.Sources.Gameplay.Cameras
{
    public class GameplayCamera : MonoBehaviour
    {
        private const int OrthographicSize = 20;

        [SerializeField] private Camera _mainCamera;

        private AnimationsConfig _animationsConfig;
        private IPersistentProgressService _persistentProgressService;

        private Tween _mover;

        [Inject]
        private void Construct(IStaticDataService staticDataService, IPersistentProgressService persistentProgressService)
        {
            _animationsConfig = staticDataService.AnimationsConfig;
            _persistentProgressService = persistentProgressService;

            _mainCamera.orthographicSize = OrthographicSize;

            ChangeOrthographic();

            _persistentProgressService.Progress.SettingsData.OrthographicChanged += ChangeOrthographic;
        }

        public Camera MainCamera => _mainCamera;

        protected virtual void OnDestroy()
        {
            _persistentProgressService.Progress.SettingsData.OrthographicChanged -= ChangeOrthographic;
            _mover?.Kill();
        }

        public void MoveTo(Vector3 position, TweenCallback callback = null)
        {
            _mover = transform.DOMove(position, _animationsConfig.CameraMoveDuration);
            _mover.onComplete += callback;
        }

        private void ChangeOrthographic() =>
            _mainCamera.orthographic = _persistentProgressService.Progress.SettingsData.IsOrthographicCamera;

        public class Factory : PlaceholderFactory<string, Vector3, UniTask<GameplayCamera>>
        {
        }
    }
}
