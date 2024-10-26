using Assets.Sources.Gameplay.Cameras;
using DG.Tweening;
using UnityEngine;
using Zenject;

namespace Assets.Sources.UI
{
    public class BluredBackgroundWindow : Window
    {
        private const int PlaneDistance = 1;

        [SerializeField] private Canvas _canvas;
        [SerializeField] private Blur _blur;

        [Inject]
        private void Construct(GameplayCamera gameplayCamera)
        {
            _canvas.worldCamera = gameplayCamera.Camera;
            _canvas.planeDistance = PlaneDistance;
        }

        public override void Open()
        {
            base.Open();
            _blur.Show(AnimationsConfig.WindowOpeningStateDuration);
        }

        public override void Hide(TweenCallback callback)
        {
            base.Hide(callback);
            _blur.Hide(AnimationsConfig.WindowOpeningStateDuration);
        }
    }
}
