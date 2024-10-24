using Assets.Sources.Gameplay.Cameras;
using UnityEngine;
using Zenject;

namespace Assets.Sources.UI
{
    public class ScreenSpaceCameraWindow : Window
    {
        [Inject]
        private void Construct(GameplayCamera gameplayCamera) =>
            Canvas.worldCamera = gameplayCamera.Camera;
    }
}
