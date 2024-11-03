using Assets.Sources.Services.StaticDataService;
using Assets.Sources.Services.StaticDataService.Configs;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using System;
using System.Linq;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Zenject;

namespace Assets.Sources.Gameplay.World
{
    public class World : MonoBehaviour, IWorldRotation
    {
        private const int FullRotation = 360;
        private const int SimpleRotation = 90;

        private AnimationsConfig _animationsConfig;

        private Sequence _aroundRotating;
        private Quaternion _startRotation;
        private Tween _rotation;
        private Tween _movement;

        [Inject]
        private void Construct(IStaticDataService staticDataService)
        {
            _animationsConfig = staticDataService.AnimationsConfig;

            RotationDegrees = 0;

            Debug.Log("start create " + IsCreated);
            IsCreated = false;
        }

        public event Action Entered;
        public event Action Cleaned;

        public int RotationDegrees { get; private set; }
        public bool IsCreated { get; private set; }

        private void Start() =>
            _startRotation = transform.rotation;

        private void OnDestroy()
        {
            TryStopRotating();

            _movement?.Kill();
            _rotation?.Kill();
        }

        public void EnterBootstrapState() =>
            Entered?.Invoke();

        public void OnCreated() =>
            IsCreated = true;

        public void StartRotating()
        {
            _aroundRotating = DOTween
                .Sequence()
                .Append(transform.DORotate(new Vector3(transform.rotation.x, transform.rotation.y + FullRotation / 2, transform.rotation.z), _animationsConfig.WorldRotateDuration / 2).From(new Vector3(transform.rotation.x, transform.rotation.y, transform.rotation.z)).SetEase(Ease.Linear))
                .Append(transform.DORotate(new Vector3(transform.rotation.x, transform.rotation.y + FullRotation, transform.rotation.z), _animationsConfig.WorldRotateDuration / 2).SetEase(Ease.Linear))
                .SetLoops(-1, LoopType.Restart);
        }

        public void TryStopRotating() =>
            _aroundRotating?.Kill();

        public void RotateToStart(TweenCallback callback) =>
            transform.DORotateQuaternion(_startRotation, _animationsConfig.WorldRotateToStarDuration).onComplete += callback;

        public void RotateСlockwise() =>
            Rotate(SimpleRotation);

        public void RotateСounterclockwise() =>
            Rotate(-SimpleRotation);

        public void MoveTo(Vector3 targetPosition, TweenCallback callback = null)
        {
            _movement = transform.DOMove(targetPosition, _animationsConfig.WorldMoveDuration);
            _movement.onComplete = callback;
        }

        public void Clean() =>
            Cleaned?.Invoke();

        private void Rotate(int degrees)
        {
            RotationDegrees += degrees;

            _rotation?.Kill();

            _rotation = transform.DORotateQuaternion(Quaternion.Euler(transform.rotation.x, RotationDegrees, transform.rotation.z), _animationsConfig.WorldSimpleRotateDuration);
        }

        public class Factory : PlaceholderFactory<AssetReferenceGameObject, Vector3, Transform, UniTask<World>>
        {
        }
    }
}
