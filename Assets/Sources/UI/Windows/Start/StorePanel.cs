using DG.Tweening;
using UnityEngine;

namespace Assets.Sources.UI.Windows.Start
{
    public class StorePanel : StartWindowPanel
    {
        [SerializeField] private PackagesPanel _packagesPanel;
        [SerializeField] private Blur _blur;

        public override void Open()
        {
            base.Open();
            _blur.Show(AnimationsConfig.WindowOpeningStateDuration);
            _packagesPanel.Open();
        }

        protected override void Hide(TweenCallback callback)
        {
            base.Hide(callback);
            _blur.Hide(AnimationsConfig.WindowOpeningStateDuration);
            _packagesPanel.Hide();
        }
    }
}
