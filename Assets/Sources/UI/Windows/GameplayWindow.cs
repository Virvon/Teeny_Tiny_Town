using Assets.Sources.Gameplay.Cameras;
using Assets.Sources.Gameplay.World.StateMachine;
using Assets.Sources.Gameplay.World.StateMachine.States;
using Assets.Sources.Gameplay.World.WorldInfrastructure.NextBuildingForPlacing;
using Assets.Sources.UI.Panels;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Assets.Sources.UI.Windows
{
    public class GameplayWindow : Window
    {
        [SerializeField] private Button _hideButton;
        [SerializeField] private WorldChangingWindowPanel _worldChangingWindowPanel;
        [SerializeField] private WorldChangingWindowPanel _saveGameplayPanel;

        private NextBuildingForPlacingCreator _nextBuildingForPlacingCreator;

        [Inject]
        private void Construct(WorldStateMachine worldStateMachine, NextBuildingForPlacingCreator nextBuildingForPlacingCreator, GameplayCamera gameplayCamera)
        {
            WorldStateMachine = worldStateMachine;
            _nextBuildingForPlacingCreator = nextBuildingForPlacingCreator;

            Canvas.worldCamera = gameplayCamera.Camera;
        }

        protected WorldStateMachine WorldStateMachine { get; private set; }

        protected virtual void OnEnable()
        {
            _hideButton.onClick.AddListener(OnHideButtonClicked);
            _nextBuildingForPlacingCreator.NoMoreEmptyTiles += OnNoMoreEmptyTiles;
        }

        protected virtual void OnDisable()
        {
            _hideButton.onClick.RemoveListener(OnHideButtonClicked);
            _nextBuildingForPlacingCreator.NoMoreEmptyTiles -= OnNoMoreEmptyTiles;
        }

        private void OnHideButtonClicked() =>
            WorldStateMachine.Enter<ExitWorldState>().Forget();

        private void OnNoMoreEmptyTiles()
        {
            _worldChangingWindowPanel.Hide(callback: _saveGameplayPanel.Open);
        }
    }
}
