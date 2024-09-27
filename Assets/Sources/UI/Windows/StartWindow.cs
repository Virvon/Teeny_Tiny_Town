﻿using Assets.Sources.Gameplay.StateMachine;
using Assets.Sources.Gameplay.StateMachine.States;
using Cysharp.Threading.Tasks;
using System;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Assets.Sources.UI.Windows
{
    public class StartWindow : Window
    {
        [SerializeField] private Button _mapSelectionButton;
        [SerializeField] private Button _continueButton;

        private GameplayStateMachine _gameplayStateMachine;

        [Inject]
        private void Construct(GameplayStateMachine gameplayStateMachine)
        {
            _gameplayStateMachine = gameplayStateMachine;
        }

        public override void Open()
        {
            base.Open();

            _mapSelectionButton.onClick.AddListener(OnMapSelectionButtonClicked);
            _continueButton.onClick.AddListener(OnContinueButtonClicked);
        }

        public override void Hide()
        {
            base.Hide();

            _mapSelectionButton.onClick.RemoveListener(OnMapSelectionButtonClicked);
            _continueButton.onClick.RemoveListener(OnContinueButtonClicked);
        }

        private void OnMapSelectionButtonClicked()
        {
            _gameplayStateMachine.Enter<MapSelectionState>().Forget();
        }

        private void OnContinueButtonClicked()
        {
            _gameplayStateMachine.Enter<GameplayLoopState>().Forget();
        }
    }
}
