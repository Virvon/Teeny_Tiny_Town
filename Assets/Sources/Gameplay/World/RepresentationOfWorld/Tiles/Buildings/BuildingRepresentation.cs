﻿using Assets.Sources.Services.StaticDataService;
using Assets.Sources.Services.StaticDataService.Configs;
using Assets.Sources.Services.StaticDataService.Configs.Building;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Zenject;

namespace Assets.Sources.Gameplay.World.RepresentationOfWorld.Tiles.Buildings
{
    public class BuildingRepresentation : MonoBehaviour
    {
        private AnimationsConfig _animationsConfig;
        private Sequence _sequence;
        private Vector3 _startPosition;

        [Inject]
        private void Construct(IStaticDataService staticDataservice)
        {
            _animationsConfig = staticDataservice.AnimationsConfig;
            _startPosition = transform.position;
        }

        public BuildingType Type { get; private set; }

        private void OnDestroy() => 
            TryKillSequence();

        public void Init(BuildingType type) =>
            Type = type;

        public void StopShaking()
        {
            TryKillSequence();

            transform.position = _startPosition;
        }

        public void StopBlinking()
        {
            TryKillSequence();

            transform.localScale = Vector3.one;
        }

        public void Shake()
        {
            TryKillSequence();
            
            _sequence = DOTween
                .Sequence()
                .Append(transform.DOMove(_startPosition + _animationsConfig.BuildingShakeOffset, _animationsConfig.BuildingShakeTweenDuration).SetEase(_animationsConfig.BuildingShakeCurve))
                .SetLoops(_animationsConfig.BuildingShakesCount, LoopType.Restart);
        }

        public void Blink()
        {
            _sequence = DOTween
                .Sequence()
                .Append(transform.DOScale(_animationsConfig.BuildingBlinkingScale, _animationsConfig.BuildingBlinkingDuration).SetEase(Ease.OutSine))
                .Append(transform.DOScale(1, _animationsConfig.BuildingBlinkingDuration).SetEase(Ease.OutSine))
                .SetLoops(-1);
        }

        public void AnimateDestroy(Vector3 destroyPosition)
        {
            transform.DOJump(
                destroyPosition + _animationsConfig.BuildingJumpDestroyOffset,
                _animationsConfig.BuildingJumpDestroyPower,
                1,
                _animationsConfig.BuildingJumpDestroyDuration).onComplete += Destroy;
        }

        public void Destroy() =>
            Destroy(gameObject);

        public async UniTask AnimateDestroy()
        {
            await transform.DOScale(0, _animationsConfig.TileUpdatingDuration).AsyncWaitForCompletion();
            Destroy();
        }

        public async UniTask AnimatePut(bool waitForCompletion)
        {
            float tweenDuration = _animationsConfig.BuildingPutDuration / 3;

            Sequence sequence = DOTween
                .Sequence()
                .Append(transform.DOScale(_animationsConfig.BuildingPutMaxScale, tweenDuration))
                .Append(transform.DOScale(_animationsConfig.BuildingPutMinScale, tweenDuration))
                .Append(transform.DOScale(1f, tweenDuration));

            if (waitForCompletion)
                await sequence.AsyncWaitForCompletion();
            else
                await UniTask.WaitForSeconds(_animationsConfig.TileUpdatingDuration);
        }

        private void TryKillSequence()
        {
            if (_sequence != null)
                _sequence.Kill();
        }

        public class Factory : PlaceholderFactory<AssetReferenceGameObject, Vector3, Transform, UniTask<BuildingRepresentation>>
        {
        }
    }
}
