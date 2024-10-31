using DG.Tweening;

namespace Assets.Sources.UI.Windows.Sandbox
{
    public class SandboxPanel : SlidePanel
    {
        public override void Open()
        {
            SlideOpen();
        }

        public void Hide(TweenCallback callback)
        {
            SlideHide(callback);
        }
    }
}
