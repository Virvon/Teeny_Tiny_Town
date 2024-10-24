using Assets.Sources.Gameplay.GameplayMover;
using Assets.Sources.Gameplay.World.StateMachine;
using Assets.Sources.Gameplay.World.StateMachine.States;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Assets.Sources.UI.Windows.World.Panels
{
    public class SaveGameplayPanel : WorldChangingWindowPanel
    {
        [SerializeField] private Button _undoButton;
        [SerializeField] private Button _completeButton;
        [SerializeField] private WorldChangingWindowPanel _worldChangingWindowPanel;
        [SerializeField] private Blur _blur;

        private IGameplayMover _gameplayMover;
        private WorldStateMachine _worldStateMachine;

        [Inject]
        private void Construct(IGameplayMover gameplayMover, WorldStateMachine worldStateMachine)
        {
            _gameplayMover = gameplayMover;
            _worldStateMachine = worldStateMachine;
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

        private void OnCompleteButtonClicked() =>
            _worldStateMachine.Enter<ResultState>().Forget();

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
