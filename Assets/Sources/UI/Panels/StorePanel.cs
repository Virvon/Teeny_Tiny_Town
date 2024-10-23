using DG.Tweening;
using UnityEngine;

namespace Assets.Sources.UI.Panels
{
    public class StorePanel : StartWindowPanel
    {
        [SerializeField] private PackagesPanel _packagesPanel;
        [SerializeField] private Blur _blur;

        public override void Open()
        {
            base.Open();
            _blur.Show(AnimationsConfig.PanelOpeningStateDuration);
            _packagesPanel.Open();
        }

        protected override void Hide(TweenCallback callback)
        {
            base.Hide(callback);
            _blur.Hide(AnimationsConfig.PanelOpeningStateDuration);
            _packagesPanel.Hide();
        }
    }
}
