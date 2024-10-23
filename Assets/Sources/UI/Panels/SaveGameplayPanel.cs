using Assets.Sources.Gameplay.GameplayMover;
using DG.Tweening;
using System;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Assets.Sources.UI.Panels
{
    public class SaveGameplayPanel : WorldChangingWindowPanel
    {
        [SerializeField] private Button _undoButton;
        [SerializeField] private Button _completeButton;
        [SerializeField] private WorldChangingWindowPanel _worldChangingWindowPanel;
        [SerializeField] private Blur _blur;

        private IGameplayMover _gameplayMover;

        [Inject]
        private void Construct(IGameplayMover gameplayMover)
        {
            _gameplayMover = gameplayMover;
        }

        private void OnEnable()
        {
            _undoButton.onClick.AddListener(OnUndoButtonClicked);
            _completeButton.onClick.AddListener(OnCompleteButtonClicked);
        }

        private void OnDisable()
        {
            _undoButton.onClick.RemoveListener(OnUndoButtonClicked);
            _completeButton.onClick.RemoveListener(OnCompleteButtonClicked);
        }

        public override void Open()
        {
            base.Open();
            _blur.Show(AnimationsConfig.PanelOpeningStateDuration);
        }

        public override void Hide(TweenCallback callback)
        {
            base.Hide(callback);
            _blur.Hide(AnimationsConfig.PanelOpeningStateDuration);
        }

        private void OnCompleteButtonClicked()
        {
            throw new NotImplementedException();
        }

        private void OnUndoButtonClicked()
        {
            Hide(callback: () =>
            {
                _worldChangingWindowPanel.Open();
                _gameplayMover.TryUndoCommand();
            });
        }
    }
}
