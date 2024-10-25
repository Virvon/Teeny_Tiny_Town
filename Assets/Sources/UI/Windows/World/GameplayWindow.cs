using Assets.Sources.Gameplay.Cameras;
using Assets.Sources.Gameplay.StateMachine;
using Assets.Sources.Gameplay.StateMachine.States;
using Assets.Sources.Gameplay.World.StateMachine;
using Assets.Sources.Gameplay.World.StateMachine.States;
using Assets.Sources.Gameplay.World.WorldInfrastructure.NextBuildingForPlacing;
using Assets.Sources.UI.Windows.World.Panels;
using Cysharp.Threading.Tasks;
using System;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Assets.Sources.UI.Windows.World
{
    public class GameplayWindow : ScreenSpaceCameraWindow
    {
        [SerializeField] private Button _hideButton;
        [SerializeField] private WorldChangingWindowPanel _worldChangingWindowPanel;
        [SerializeField] private WorldChangingWindowPanel _saveGameplayPanel;
        [SerializeField] private Button _questsWindowOpenButton;

        private NextBuildingForPlacingCreator _nextBuildingForPlacingCreator;
        private GameplayStateMachine _gameplayStateMachine;

        [Inject]
        private void Construct(WorldStateMachine worldStateMachine, NextBuildingForPlacingCreator nextBuildingForPlacingCreator, GameplayStateMachine gameplayStateMachine)
        {
            WorldStateMachine = worldStateMachine;
            _nextBuildingForPlacingCreator = nextBuildingForPlacingCreator;
            _gameplayStateMachine = gameplayStateMachine;

            Subscrube();
        }

        protected WorldStateMachine WorldStateMachine { get; private set; }

        private void OnDestroy() =>
            Unsubscruby();

        protected virtual void Subscrube()
        {
            _hideButton.onClick.AddListener(OnHideButtonClicked);
            _nextBuildingForPlacingCreator.NoMoreEmptyTiles += OnNoMoreEmptyTiles;
            _questsWindowOpenButton.onClick.AddListener(OnQuestsWindowOpenButtonClicked);
        }

        protected virtual void Unsubscruby()
        {
            _hideButton.onClick.RemoveListener(OnHideButtonClicked);
            _nextBuildingForPlacingCreator.NoMoreEmptyTiles -= OnNoMoreEmptyTiles;
            _questsWindowOpenButton.onClick.AddListener(OnQuestsWindowOpenButtonClicked);
        }

        private void OnQuestsWindowOpenButtonClicked() =>
            WorldStateMachine.Enter<QuestsState>().Forget();

        private void OnHideButtonClicked() =>
            WorldStateMachine.Enter<ExitWorldState, Action>(() => _gameplayStateMachine.Enter<MapSelectionState>().Forget()).Forget();

        private void OnNoMoreEmptyTiles() =>
            _worldChangingWindowPanel.Hide(callback: _saveGameplayPanel.Open);
    }
}
