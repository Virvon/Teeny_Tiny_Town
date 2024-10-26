using Assets.Sources.Services.StaticDataService;
using Assets.Sources.Services.StaticDataService.Configs;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Zenject;

namespace Assets.Sources.UI
{
    public class Window : MonoBehaviour
    {
        [SerializeField] private CanvasGroup _canvasGroup;

        [Inject]
        private void Construct(IStaticDataService staticDataService)
        {
            AnimationsConfig = staticDataService.AnimationsConfig;
        }

        protected AnimationsConfig AnimationsConfig { get; private set; }

        public virtual void Open()
        {
            Fade(1, callback: () =>
            {
                _canvasGroup.interactable = true;
                _canvasGroup.blocksRaycasts = true;
            });
        }

        public virtual void Hide(TweenCallback callback)
        {
            _canvasGroup.interactable = false;
            _canvasGroup.blocksRaycasts = false;

            Fade(0, callback);
        }


        public void HideImmediately()
        {
            _canvasGroup.interactable = false;
            _canvasGroup.blocksRaycasts = false;
            _canvasGroup.alpha = 0;
        }

        public void Destroy() =>
           Destroy(gameObject);

        private void Fade(int targetValue, TweenCallback callback) =>
            _canvasGroup.DOFade(targetValue, AnimationsConfig.WindowOpeningStateDuration).onComplete += callback;

        public class Factory : PlaceholderFactory<AssetReferenceGameObject, UniTask<Window>>
        {
        }
    }
}
