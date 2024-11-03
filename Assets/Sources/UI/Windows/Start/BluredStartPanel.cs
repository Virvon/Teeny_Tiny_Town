using DG.Tweening;
using UnityEngine;
using Zenject;

namespace Assets.Sources.UI.Windows.Start
{
    public class BluredStartPanel : StartWindowPanel
    {
        private Blur _blur;

        [Inject]
        private void Construct(Blur blur) =>
            _blur = blur;

        public override void Open()
        {
            base.Open();
            _blur.Open();
        }

        protected override void Hide(TweenCallback callback)
        {
            base.Hide(callback);
            _blur.Hide();
        }
    }
}
