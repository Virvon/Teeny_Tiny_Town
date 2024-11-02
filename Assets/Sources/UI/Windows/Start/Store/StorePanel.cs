using DG.Tweening;
using UnityEngine;

namespace Assets.Sources.UI.Windows.Start.Store
{
    public class StorePanel : BluredStartPanel
    {
        [SerializeField] private PackagesPanel _packagesPanel;

        public override void Open()
        {
            base.Open();
            _packagesPanel.Open();
        }

        protected override void Hide(TweenCallback callback)
        {
            base.Hide(callback);
            _packagesPanel.Hide();
        }
    }
}
