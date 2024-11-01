using DG.Tweening;
using System;
using UnityEngine;

namespace Assets.Sources.UI.Windows.Education
{
    public abstract class EducationPanel : CanvasGroupPanel
    {
        public event Action Handled;

        public override void Open()
        {
            ChangeCanvasGroupAlpha(1, callback: () =>
            {
                CanvasGroup.blocksRaycasts = true;
                CanvasGroup.interactable = true;
            });
        }

        public virtual void Hide(TweenCallback callback)
        {
            CanvasGroup.blocksRaycasts = false;
            CanvasGroup.interactable = false;
            ChangeCanvasGroupAlpha(0, callback);
        }

        protected void OnHandled() =>
            Handled?.Invoke();
    }
}
