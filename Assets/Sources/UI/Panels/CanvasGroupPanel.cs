using Assets.Sources.Services.StaticDataService.Configs;
using DG.Tweening;
using UnityEngine;

namespace Assets.Sources.UI.Panels
{
    public abstract class CanvasGroupPanel : Panel
    {
        [SerializeField] private CanvasGroup _canvasGroup;
        
        public CanvasGroup CanvasGroup => _canvasGroup;

        public override abstract void Open();

        protected void ChangeCanvasGroupAlpha(float targetValue, TweenCallback callback = null) =>
            _canvasGroup.DOFade(targetValue, AnimationsConfig.PanelOpeningStateDuration).onComplete += callback;
    }
}
