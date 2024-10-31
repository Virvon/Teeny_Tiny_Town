using DG.Tweening;
using UnityEngine;

namespace Assets.Sources.UI
{
    public abstract class SlidePanel : Panel
    {
        [SerializeField] private RectTransform _rectTransform;

        [SerializeField] private Vector2 _openedMinAnchor;
        [SerializeField] private Vector2 _openedMaxAnchor;
        [SerializeField] private Vector2 _openedPivot;
        [SerializeField] private Vector2 _openedPosition;

        [SerializeField] private Vector2 _hidedMinAnchor;
        [SerializeField] private Vector2 _hidedMaxAnchor;
        [SerializeField] private Vector2 _hidedPivot;
        [SerializeField] private Vector2 _hidedPosition;

        protected void SlideOpen(TweenCallback callback = null) =>
            ChangeOpeningState(_hidedMinAnchor, _hidedMaxAnchor, _hidedPivot, _hidedPosition, _openedMinAnchor, _openedMaxAnchor, _openedPivot, _openedPosition, callback);

        protected void SlideHide(TweenCallback callback = null) =>
            ChangeOpeningState(_openedMinAnchor, _openedMaxAnchor, _openedPivot, _openedPosition, _hidedMinAnchor, _hidedMaxAnchor, _hidedPivot, _hidedPosition, callback);

        protected void ChangeOpeningState(
            Vector2 fromMinAnchor,
            Vector2 fromMaxAnchor,
            Vector2 fromPivot,
            Vector2 fromPosition,
            Vector2 toMinAnchor,
            Vector2 toMaxAnchor,
            Vector2 toPivot,
            Vector2 toPosition,
            TweenCallback callback)
        {
            _rectTransform.DOAnchorMin(toMinAnchor, AnimationsConfig.WindowOpeningStateDuration).From(fromMinAnchor);
            _rectTransform.DOAnchorMax(toMaxAnchor, AnimationsConfig.WindowOpeningStateDuration).From(fromMaxAnchor);
            _rectTransform.DOPivot(toPivot, AnimationsConfig.WindowOpeningStateDuration).From(fromPivot);
            _rectTransform.DOAnchorPos(toPosition, AnimationsConfig.WindowOpeningStateDuration).From(fromPosition).onComplete = callback;
        }
    }
}
