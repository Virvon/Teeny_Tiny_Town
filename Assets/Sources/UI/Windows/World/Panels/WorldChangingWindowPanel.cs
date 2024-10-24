using DG.Tweening;
using UnityEngine;

namespace Assets.Sources.UI.Windows.World.Panels
{
    public class WorldChangingWindowPanel : CanvasGroupPanel
    {
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
    }
}
