using Assets.Sources.Services.PersistentProgress;
using Assets.Sources.Services.SaveLoadProgress;
using DG.Tweening;
using UnityEngine;
using Zenject;

namespace Assets.Sources.UI.Windows.Education
{
    public class FinishEducationPanel : ContinueEducationPanel
    {
        [SerializeField] private CanvasGroup[] _showedUi;

        private IPersistentProgressService _persistentProgressService;
        private ISaveLoadService _saveLoadService;

        private uint _startMovesCount;

        [Inject]
        private void Construct(IPersistentProgressService presistentProgressService, ISaveLoadService saveLoadService)
        {
            _persistentProgressService = presistentProgressService;
            _saveLoadService = saveLoadService;

            _startMovesCount = _persistentProgressService.Progress.GameplayMovesCounter.RemainingMovesCount;
            _persistentProgressService.Progress.GameplayMovesCounter.SetCount(uint.MaxValue);
        }

        public override void Open()
        {
            foreach (CanvasGroup canvasGroup in _showedUi)
            {
                canvasGroup.DOFade(0, AnimationsConfig.WindowOpeningStateDuration).onComplete += () =>
                {
                    canvasGroup.blocksRaycasts = false;
                    canvasGroup.interactable = false;
                };
            }

            base.Open();
        }

        public override void Hide(TweenCallback callback)
        {
            Hide();
            base.Hide(callback);
        }

        public override void Hide()
        {
            _persistentProgressService.Progress.GameplayMovesCounter.SetCount(_startMovesCount);

            InputService.SetEnabled(true);
            ActionHandlerStateMachine.SetActive(true);
            MarkersVisibility.ChangeAllowedVisibility(true);

            _persistentProgressService.Progress.IsEducationCompleted = true;

            foreach (CanvasGroup canvasGroup in _showedUi)
            {
                canvasGroup.DOFade(1, AnimationsConfig.WindowOpeningStateDuration).onComplete += () =>
                {
                    canvasGroup.blocksRaycasts = true;
                    canvasGroup.interactable = true;
                };
            }

            _saveLoadService.SaveProgress();
        }
    }
}
