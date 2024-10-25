using Assets.Sources.Services.StaticDataService;
using Assets.Sources.Services.StaticDataService.Configs;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Zenject;

namespace Assets.Sources.Gameplay.World
{
    public class World : MonoBehaviour
    {
        private const int FullRotation = 360;
        private const int SimpleRotation = 90;

        private AnimationsConfig _animationsConfig;

        private Sequence _aroundRotating;
        private Quaternion _startRotation;
        private int _rotationDegrees;
        private Tween _rotation;

        [Inject]
        private void Construct(IStaticDataService staticDataService)
        {
            _animationsConfig = staticDataService.AnimationsConfig;

            _rotationDegrees = 0;
        }

        public event Action Entered;

        private void Start()
        {
            _startRotation = transform.rotation;
        }

        private void OnDestroy() =>
            TryStopRotating();


        public void EnterBootstrapState() =>
            Entered?.Invoke();

        public void StartRotating()
        {
            _aroundRotating = DOTween
                .Sequence()
                .Append(transform.DORotate(new Vector3(transform.rotation.x, transform.rotation.y + FullRotation / 2, transform.rotation.z), _animationsConfig.WorldRotateDuration / 2).From(new Vector3(transform.rotation.x, transform.rotation.y, transform.rotation.z)).SetEase(Ease.Linear))
                .Append(transform.DORotate(new Vector3(transform.rotation.x, transform.rotation.y + FullRotation, transform.rotation.z), _animationsConfig.WorldRotateDuration / 2).SetEase(Ease.Linear))
                .SetLoops(-1, LoopType.Restart);
        }

        public void TryStopRotating()
        {
            if (_aroundRotating != null)
                _aroundRotating.Kill();
        }

        public void RotateToStart(TweenCallback callback) =>
            transform.DORotateQuaternion(_startRotation, _animationsConfig.WorldRotateToStarDuration).onComplete += callback;

        public void RotateСlockwise() =>
            Rotate(SimpleRotation);

        public void RotateСounterclockwise() =>
            Rotate(-SimpleRotation);

        private void Rotate(int degrees)
        {
            _rotationDegrees += degrees;

            if (_rotation != null)
                _rotation.Kill();

            _rotation = transform.DORotateQuaternion(Quaternion.Euler(transform.rotation.x, _rotationDegrees, transform.rotation.z), _animationsConfig.WorldSimpleRotateDuration);
        }

        public class Factory : PlaceholderFactory<AssetReferenceGameObject, Vector3, Transform, UniTask<World>>
        {
        }
    }
}
