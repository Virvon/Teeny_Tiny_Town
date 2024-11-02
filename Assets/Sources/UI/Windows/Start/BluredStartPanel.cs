using DG.Tweening;
using UnityEngine;

namespace Assets.Sources.UI.Windows.Start
{
    public class BluredStartPanel : StartWindowPanel
    {
        [SerializeField] private Blur _blur;

        public override void Open()
        {
            base.Open();
            _blur.Show(AnimationsConfig.WindowOpeningStateDuration);
        }

        protected override void Hide(TweenCallback callback)
        {
            base.Hide(callback);
            _blur.Hide(AnimationsConfig.WindowOpeningStateDuration);
        }
    }
}
