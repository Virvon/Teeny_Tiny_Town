﻿using Assets.Sources.Data.World;
using Assets.Sources.Gameplay.StateMachine;
using Assets.Sources.Gameplay.StateMachine.States;
using Assets.Sources.Gameplay.World;
using Assets.Sources.Services.PersistentProgress;
using Assets.Sources.Services.StaticDataService;
using Assets.Sources.Services.StaticDataService.Configs.World;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Assets.Sources.UI.Windows.MapSelection
{
    public class MapSelectionWindow : Window
    {
        [SerializeField] private Button _nextMapButton;
        [SerializeField] private Button _previousMapButton;
        [SerializeField] private Button _hideButton;
        [SerializeField] private MapSelectionPanel _unlockedMapPanel;
        [SerializeField] private MapSelectionPanel _lcockedMapPanel;
        [SerializeField] private TMP_Text _sizeValue;
        [SerializeField] private TMP_Text _name;

        private WorldsList _worldsList;
        private GameplayStateMachine _gameplayStateMachine;
        private IPersistentProgressService _persistentProgressService;
        private IStaticDataService _staticDataService;

        [Inject]
        private void Construct(
            WorldsList worldsList,
            GameplayStateMachine gameplayStateMachine,
            IPersistentProgressService persistentProgressService,
            IStaticDataService staticDataService)
        {
            _worldsList = worldsList;
            _gameplayStateMachine = gameplayStateMachine;
            _persistentProgressService = persistentProgressService;
            _staticDataService = staticDataService;

            _nextMapButton.onClick.AddListener(OnNextMapButtonClicked);
            _previousMapButton.onClick.AddListener(OnPreviousMapButtonClicked);
            _hideButton.onClick.AddListener(OnHideButtonClicked);
            _worldsList.CurrentWorldChanged += ChangeCurrentWorldInfo;

        }

        private void OnDestroy()
        {
            _nextMapButton.onClick.RemoveListener(OnNextMapButtonClicked);
            _previousMapButton.onClick.RemoveListener(OnPreviousMapButtonClicked);          
            _hideButton.onClick.RemoveListener(OnHideButtonClicked);
            _worldsList.CurrentWorldChanged -= ChangeCurrentWorldInfo;
        }

        public override void Open()
        {
            base.Open();
            ChangeCurrentWorldInfo(_worldsList.CurrentWorldData);
        }

        public void ChangeCurrentWorldInfo(IWorldData worldData)
        {
            WorldConfig worldConfig = _staticDataService.GetWorld<WorldConfig>(worldData.Id);
            Vector2Int size = worldData.Size;

            _sizeValue.text = $"{size.x}x{size.y}";
            _name.text = worldConfig.Name;

            if(worldData.IsUnlocked)
            {
                _unlockedMapPanel.Open();
                _lcockedMapPanel.Hide();
            }
            else
            {
                _unlockedMapPanel.Hide();
                _lcockedMapPanel.Open();
            }
        }

        private async void OnPreviousMapButtonClicked() =>
            await _worldsList.ShowPreviousWorld();

        private async void OnNextMapButtonClicked() =>
            await _worldsList.ShowNextWorld();

        private void OnHideButtonClicked() =>
            _gameplayStateMachine.Enter<GameStartState>().Forget();
    }
}