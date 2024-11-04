using Assets.Sources.Sandbox.ActionHandler;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using System;
using UnityEngine;
using Zenject;

namespace Assets.Sources.UI.Windows.Sandbox
{
    public class SandboxPanel : SlidePanel
    {
        [SerializeField] private Transform _content;

        protected Transform Content => _content;

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
