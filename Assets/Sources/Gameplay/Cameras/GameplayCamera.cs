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
        [SerializeField] private Camera _camera;

        private AnimationsConfig _animationsConfig;

        [Inject]
        private void Construct(IStaticDataService staticDataService)
        {
            _animationsConfig = staticDataService.AnimationsConfig;
        }

        public Camera Camera => _camera;

        public void MoveTo(Vector3 position, TweenCallback callback = null) =>
            transform.DOMove(position, _animationsConfig.CameraMoveDuration).onComplete += callback;

        public class Factory : PlaceholderFactory<string, UniTask<GameplayCamera>>
        {
        }
    }
}
