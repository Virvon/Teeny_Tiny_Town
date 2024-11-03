using Assets.Sources.Gameplay.Cameras;
using DG.Tweening;
using Zenject;

namespace Assets.Sources.UI
{
    public class BluredBackgroundWindow : Window
    {
        private Blur _blur;

        [Inject]
        private void Construct(GameplayCamera gameplayCamera, Blur blur) =>
            _blur = blur;

        public override void Open()
        {
            base.Open();
            _blur.Open();
        }

        public override void Hide(TweenCallback callback)
        {
            base.Hide(callback);
            _blur.Hide();
        }
    }
}
