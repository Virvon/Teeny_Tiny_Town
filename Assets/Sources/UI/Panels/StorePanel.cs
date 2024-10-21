using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Sources.UI.Panels
{
    public class StorePanel : CanvasGroupPanel
    {
        private const string Blur = "_Blur";
        private const float MaxBlur = 0.015f;
        private const float MinBlur = 0;

        [SerializeField] private Image _blur;
        [SerializeField] private PackagesPanel _packagesPanel;

        public override void Open()
        {
            base.Open();
            ChangeBlured(MaxBlur);
            _packagesPanel.Open();
        }

        protected override void Hide(TweenCallback callback)
        {
            base.Hide(callback);
            ChangeBlured(MinBlur);
            _packagesPanel.Hide();
        }

        private void ChangeBlured(float targetValue) =>
            _blur.material.DOFloat(targetValue, Blur, AnimationsConfig.PanelOpeningStateDuration);
    }
}
