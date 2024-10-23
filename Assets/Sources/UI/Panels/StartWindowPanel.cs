using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Sources.UI.Panels
{
    public class StartWindowPanel : CanvasGroupPanel
    {
        [SerializeField] private CanvasGroupPanel _nextPanel;
        [SerializeField] private Button _openNextPanelButton;

        private void OnEnable() =>
            _openNextPanelButton.onClick.AddListener(OpenNextPanel);

        private void OnDisable() =>
            _openNextPanelButton.onClick.RemoveListener(OpenNextPanel);

        public override void Open()
        {
            ChangeCanvasGroupAlpha(1, callback: () =>
            {
                CanvasGroup.blocksRaycasts = true;
                CanvasGroup.interactable = true;
            });
        }

        protected virtual void Hide(TweenCallback callback)
        {
            CanvasGroup.blocksRaycasts = false;
            CanvasGroup.interactable = false;
            ChangeCanvasGroupAlpha(0, callback);
        }

        private void OpenNextPanel() =>
            Hide(callback: _nextPanel.Open);
    }
}
