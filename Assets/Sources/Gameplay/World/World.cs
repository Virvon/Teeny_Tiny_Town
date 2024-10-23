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
        private const float FullRotation = 360;

        private AnimationsConfig _animationsConfig;

        private Sequence _rotating;
        private Quaternion _startRotation;

        [Inject]
        private void Construct(IStaticDataService staticDataService)
        {
            _animationsConfig = staticDataService.AnimationsConfig;
        }

        private void Start()
        {
            _startRotation = transform.rotation;
        }

        private void OnDestroy() =>
            TryStopRotating();

        public event Action Entered;

        public void EnterBootstrapState() =>
            Entered?.Invoke();

        public void StartRotating()
        {
            _rotating = DOTween
                .Sequence()
                .Append(transform.DORotate(new Vector3(transform.rotation.x, transform.rotation.y + FullRotation / 2, transform.rotation.z), _animationsConfig.WorldRotateDuration / 2).From(new Vector3(transform.rotation.x, transform.rotation.y, transform.rotation.z)).SetEase(Ease.Linear))
                .Append(transform.DORotate(new Vector3(transform.rotation.x, transform.rotation.y + FullRotation, transform.rotation.z), _animationsConfig.WorldRotateDuration / 2).SetEase(Ease.Linear))
                .SetLoops(-1, LoopType.Restart);
        }

        public void TryStopRotating()
        {
            if (_rotating != null)
                _rotating.Kill();
        }

        public void RotateToStart(TweenCallback callback)
        {
            transform.DORotateQuaternion(_startRotation, _animationsConfig.WorldRotateToStarDuration).onComplete += callback;
        }

        public class Factory : PlaceholderFactory<AssetReferenceGameObject, Vector3, Transform, UniTask<World>>
        {
        }
    }
}
