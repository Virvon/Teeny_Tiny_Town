using Assets.Sources.Services.StaticDataService;
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

        [Inject]
        private void Construct(IStaticDataService staticDataservice) =>
            _animationsConfig = staticDataservice.AnimationsConfig;

        public BuildingType Type { get; private set; }

        public void Init(BuildingType type) =>
            Type = type;

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
            Sequence sequence = DOTween.Sequence();
            float tweenDuration = _animationsConfig.BuildingPutDuration / 3;

            sequence.Append(transform.DOScale(_animationsConfig.BuildingPutMaxScale, tweenDuration));
            sequence.Append(transform.DOScale(_animationsConfig.BuildingPutMinScale, tweenDuration));
            sequence.Append(transform.DOScale(1f, tweenDuration));

            if (waitForCompletion)
                await sequence.AsyncWaitForCompletion();
            else
                await UniTask.WaitForSeconds(_animationsConfig.TileUpdatingDuration);
        }

        public class Factory : PlaceholderFactory<AssetReferenceGameObject, Vector3, Transform, UniTask<BuildingRepresentation>>
        {
        }
    }
}
