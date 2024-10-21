using Assets.Sources.Services.StaticDataService.Configs;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Sources.UI.Panels
{
    public class CanvasGroupPanel : Panel
    {
        [SerializeField] private CanvasGroup _canvasGroup;
        [SerializeField] private Button _openNextPanelButton;
        [SerializeField] private CanvasGroupPanel _nextPanel;

        private void OnEnable() =>
            _openNextPanelButton.onClick.AddListener(OpenNextPanel);

        private void OnDisable() =>
            _openNextPanelButton.onClick.RemoveListener(OpenNextPanel);

        public override void Open()
        {
            ChangeCanvasGroupAlpha(1, callback: () =>
            {
                _canvasGroup.blocksRaycasts = true;
                _canvasGroup.interactable = true;
            });
        }

        protected virtual void Hide(TweenCallback callback)
        {
            _canvasGroup.blocksRaycasts = false;
            _canvasGroup.interactable = false;
            ChangeCanvasGroupAlpha(0, callback);
        }


        private void OpenNextPanel() =>
            Hide(callback: _nextPanel.Open);

        private void ChangeCanvasGroupAlpha(float targetValue, TweenCallback callback = null) =>
            _canvasGroup.DOFade(targetValue, AnimationsConfig.PanelOpeningStateDuration).onComplete += callback;
    }
}
