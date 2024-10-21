using DG.Tweening;
using UnityEngine;

namespace Assets.Sources.UI.Panels
{
    public class PackagesPanel : Panel
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

        public override void Open()
        {
            ChangeOpeningState(_hidedMinAnchor, _hidedMaxAnchor, _hidedPivot, _hidedPosition, _openedMinAnchor, _openedMaxAnchor, _openedPivot, _openedPosition);
        }

        public override void Hide()
        {
            ChangeOpeningState(_openedMinAnchor, _openedMaxAnchor, _openedPivot, _openedPosition, _hidedMinAnchor, _hidedMaxAnchor, _hidedPivot, _hidedPosition);
        }

        private void ChangeOpeningState(Vector2 fromMinAnchor, Vector2 fromMaxAnchor, Vector2 fromPivot, Vector2 fromPosition, Vector2 toMinAnchor, Vector2 toMaxAnchor, Vector2 toPivot, Vector2 toPosition)
        {
            _rectTransform.DOAnchorMin(toMinAnchor, AnimationsConfig.PanelOpeningStateDuration).From(fromMinAnchor);
            _rectTransform.DOAnchorMax(toMaxAnchor, AnimationsConfig.PanelOpeningStateDuration).From(fromMaxAnchor);
            _rectTransform.DOPivot(toPivot, AnimationsConfig.PanelOpeningStateDuration).From(fromPivot);
            _rectTransform.DOAnchorPos(toPosition, AnimationsConfig.PanelOpeningStateDuration).From(fromPosition);
        }
    }
}
