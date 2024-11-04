using DG.Tweening;
using UnityEngine;

namespace Assets.Sources.UI.Windows.Sandbox
{
    public class SandboxPanel : SlidePanel
    {
        [SerializeField] private Transform _content;

        protected Transform Content => _content;

        public override void Open() =>
            SlideOpen();

        public void Hide(TweenCallback callback) =>
            SlideHide(callback);
    }
}
